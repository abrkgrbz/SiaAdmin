﻿using System.Security.Claims;
using System.Text;
using ClosedXML.Excel;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SiaAdmin.Application.Features.Commands.Survey.CloseSurvey;
using SiaAdmin.Application.Features.Commands.Survey.CreateSurvey;
using SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokenList;
using SiaAdmin.Application.Features.Queries.Survey.GetAllSurveyData;
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

        [HttpGet("export-survey-excel")]
        public async Task<IActionResult> ExportSurveyToExcel()
        {
            try
            {

                var data = await Mediator.Send(new GetAllSurveyDataRequest());

                using (var workbook = new XLWorkbook())
                { 
                    var worksheet = workbook.Worksheets.Add("Proje Listesi"); 

                    worksheet.Cell(1, 1).Value = "SurveyId";
                    worksheet.Cell(1, 2).Value = "SurveyText";
                    worksheet.Cell(1, 3).Value = "SurveyDescription";
                    worksheet.Cell(1, 4).Value = "SurveyLink";
                    worksheet.Cell(1, 5).Value = "SurveyLinkText";
                    worksheet.Cell(1, 6).Value = "SurveyValidity";
                    worksheet.Cell(1, 7).Value = "SurveyActive";
                    worksheet.Cell(1, 8).Value = "SurveyStartDate";
                    worksheet.Cell(1, 9).Value = "SurveyPoints";
                    worksheet.Cell(1, 10).Value = "Mandatory";
                    worksheet.Cell(1, 11).Value = "Timestamp";
                    worksheet.Cell(1, 12).Value = "DBAdress"; 

                    var headerRange = worksheet.Range(1, 1, 1, 6);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Fill.BackgroundColor = XLColor.LightGray;
                    headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

                    // Verileri ekle
                    for (int i = 0; i < data.GetAllSurveyModels.Count; i++)
                    {
                        var survey = data.GetAllSurveyModels[i];
                        worksheet.Cell(i + 2, 1).Value = survey.Id;
                        worksheet.Cell(i + 2, 2).Value = survey.SurveyText;
                        worksheet.Cell(i + 2, 3).Value = survey.SurveyDescription;
                        worksheet.Cell(i + 2, 4).Value = survey.SurveyLink;
                        worksheet.Cell(i + 2, 5).Value = survey.SurveyLinkText;
                        worksheet.Cell(i + 2, 6).Value = survey.SurveyValidity;
                        worksheet.Cell(i + 2, 7).Value = survey.SurveyActive;
                        worksheet.Cell(i + 2, 8).Value = survey.SurveyStartDate;
                        worksheet.Cell(i + 2, 9).Value = survey.SurveyPoints;
                        worksheet.Cell(i + 2, 10).Value = survey.Mandotory;
                        worksheet.Cell(i + 2, 11).Value = survey.Timestamp;
                        worksheet.Cell(i + 2, 12).Value = survey.DBAdress;
                    }
                     
                    worksheet.Columns().AdjustToContents();
                     
                    using (var stream = new MemoryStream())
                    {
                        workbook.SaveAs(stream);
                        stream.Position = 0;
                         
                        string fileName = $"Projeler_Listesi_{DateTime.Now.ToString("yyyy-MM-dd")}.xlsx";
                        return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                    }
                }
            }
            catch (Exception ex)
            { 
                return StatusCode(500, "Excel dosyası oluşturulurken bir hata oluştu");
            }
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
