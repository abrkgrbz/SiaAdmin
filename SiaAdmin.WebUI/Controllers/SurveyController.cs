using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiaAdmin.Application.Features.Commands.Survey.CloseSurvey;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokenList;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;
using SiaAdmin.Application.Features.Queries.Survey.GetLastSurveyId;
using SiaAdmin.Application.Features.Queries.SurveyAssigned.GetUserGuidBySurveyAssigned;


namespace SiaAdmin.WebUI.Controllers
{
    [Authorize]

    public class SurveyController : BaseController
    {


        [HttpGet("proje-listesi")]
        public IActionResult List()
        {
            ViewBag.LastSurveyId = GetLastSurveyId().Result;
            return View();
        }

        [HttpPost("proje-listesi/proje-ekle")]
        public async Task<IActionResult> SurveyAdd(CreateSurveyRequest createSurveyRequest)
        {
            createSurveyRequest.UserId = User?.FindFirst(ClaimTypes.NameIdentifier).Value;
            var response = await Mediator.Send(createSurveyRequest);
            return Ok(response);
        }

        [HttpPost("proje-listesi/LoadTable")]
        public async Task<IActionResult> LoadTable(GetDataTableSurveyQueryRequest getDataTableSurvey)
        {

            getDataTableSurvey.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            getDataTableSurvey.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            getDataTableSurvey.orderColumnName = Request.Form[$"columns[{getDataTableSurvey.orderColumnIndex}][name]"].FirstOrDefault();
            getDataTableSurvey.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(getDataTableSurvey);
            return Ok(response);
        }

        private async Task<int> GetLastSurveyId()
        {
            var response = await Mediator.Send(new GetLastSurveyIdRequest());
            return response.SurveyId;
        }


        [Authorize("AdminOnly")]
        [HttpPost("proje-kapat")]
        public async Task<IActionResult> CloseProject(CloseSurveyRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
            return Forbid();
        }

        [HttpPost("send-notification")]
        public async Task<IActionResult> SendNotification(SendNotificationMobile model)
        {
            
            var tokens = await Mediator.Send(new GetUserDeviceTokenListRequest() { SurveyId = model.Id});
             
            if (tokens.Data == null || !tokens.Data.Any())
            {
                return BadRequest("Bildirim gönderilebilecek kayıtlı cihaz bulunamadı.");
            }
             
            var allTokens = tokens.Data.Select(x => x.DeviceIdToken).ToList();
            int totalTokens = allTokens.Count;
             
            const int batchSize = 350;
            var successCount = 0;
            var failureCount = 0;
            var batchResults = new List<object>();

            using (var client = new HttpClient())
            {
                client.Timeout = TimeSpan.FromMinutes(5);
                 
                for (int i = 0; i < allTokens.Count; i += batchSize)
                { 
                    var batchTokens = allTokens.Skip(i).Take(batchSize).ToList();
                    int batchNumber = (i / batchSize) + 1;
                    int totalBatches = (int)Math.Ceiling((double)totalTokens / batchSize);

                    try
                    {
                        var request = new DeviceRegistrationRequest
                        {
                            TokenList = batchTokens,
                            Title = model.NotificationTitle.ToUpper(),
                            Body = model.NotificationDesc.ToUpper(),
                            Datas = new
                            {
                                AdditionalProp1 = "null"
                            }
                        };

                        Console.WriteLine($"Batch {batchNumber}/{totalBatches}: Gönderilecek token sayısı: {batchTokens.Count}");

                        var json = JsonConvert.SerializeObject(request);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        var response = await client.PostAsync(
                            "https://sialive.siapanel.com/svc/api/FirebaseAuth/SendNotificationUsers", content);

                        if (response.IsSuccessStatusCode)
                        {
                            var result = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Batch {batchNumber}: Başarılı API yanıtı: {result}");
                             
                            batchResults.Add(new
                            {
                                BatchNumber = batchNumber,
                                TokenCount = batchTokens.Count,
                                IsSuccess = true,
                                Result = result
                            });
                             
                            successCount += batchTokens.Count;
                        }
                        else
                        {
                            var errorContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine($"Batch {batchNumber}: API yanıt kodu: {response.StatusCode}, Hata: {errorContent}");

                            batchResults.Add(new
                            {
                                BatchNumber = batchNumber,
                                TokenCount = batchTokens.Count,
                                IsSuccess = false,
                                Error = $"HTTP Error: {response.StatusCode}",
                                ErrorDetails = errorContent
                            }); 
                            failureCount += batchTokens.Count;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Batch {batchNumber}: Hata: {ex.Message}");

                        batchResults.Add(new
                        {
                            BatchNumber = batchNumber,
                            TokenCount = batchTokens.Count,
                            IsSuccess = false,
                            Error = ex.GetType().Name,
                            ErrorDetails = ex.Message
                        }); 
                        failureCount += batchTokens.Count;
                    }
                }
            }
             
            var finalResult = new
            {
                TotalTokens = totalTokens,
                SuccessCount = successCount,
                FailureCount = failureCount,
                TotalBatches = batchResults.Count,
                BatchResults = batchResults
            };

            return Ok(finalResult);
        }

    }

    public class DeviceRegistrationRequest
    {
        public List<string> TokenList { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public object Datas { get; set; }
    }

    public class SendNotificationMobile
    {
        public int Id { get; set; }
        public string NotificationTitle { get; set; }
        public string NotificationDesc { get; set; }
    }
}
