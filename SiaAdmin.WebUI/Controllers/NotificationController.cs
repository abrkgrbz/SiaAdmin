using Microsoft.AspNetCore.Mvc;
using SiaAdmin.Application.Features.Commands.NotificationHistory.CreateNotificationHistory;
using SiaAdmin.Application.Features.Queries.DeviceRegistrations.GetUserDeviceTokensCount;
using SiaAdmin.Application.Features.Queries.NotificationHistory.CheckNotificationCooldown;
using SiaAdmin.Application.Features.Queries.NotificationHistory.GetAllNotificationHistory;
using SiaAdmin.Application.Features.Queries.NotificationHistory.GetNotificationDetail;
using SiaAdmin.Application.Features.Queries.Survey.GetSurvey;

namespace SiaAdmin.WebUI.Controllers
{
    public class NotificationController : BaseController
    {

        public IActionResult NotificationList()
        {
            return View();
        }

        [HttpGet("project-notification-info/{id}")]
        public async Task<IActionResult> NotificationDetail(string id)
        {
            var response = await Mediator.Send(new CheckNotificationCooldownRequest() { SurveyId = int.Parse(id) });
            return Ok(response);
        }

        [HttpGet("project-notification-info-count-user/{id}")]
        public async Task<IActionResult> NotificationCountUser(string id)
        {
            var response = await Mediator.Send(new GetUserDeviceTokensCountRequest() { SurveyId = int.Parse(id) });
            return Ok(response);
        }

        [HttpPost("send-mobile-notification")]
        public async Task<IActionResult> SendMobileNotification(NotificationHistoryRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok();
        }

        [HttpPost("get-all-notifications")]
        public async Task<IActionResult> GetNotifications(GetAllNotificationHistoryRequest request)
        {
            request.orderColumnIndex = Request.Form["order[0][column]"].FirstOrDefault();
            request.orderDir = Request.Form["order[0][dir]"].FirstOrDefault();
            request.orderColumnName = Request.Form[$"columns[{request.orderColumnIndex}][name]"].FirstOrDefault();
            request.searchValue = Request.Form["search[value]"].FirstOrDefault();
            var response = await Mediator.Send(request);
            return Ok(response);
            
        }

        [HttpGet("get-notification-details")]
        public async Task<IActionResult> GetNotificationHistoryDetails(GetNotificationDetailRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }

        [HttpGet("get-surveys")]
        public async Task<IActionResult> GetProjects(GetSurveyRequest request)
        {
            var response = await Mediator.Send(request);
            return Ok(response);
        }
    }
}
