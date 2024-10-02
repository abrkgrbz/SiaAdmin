using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Commands.Employee.ApproveEmployee;
using SiaAdmin.Application.Features.Queries.SiaRole.GetSiaRoles;
using SiaAdmin.Application.Features.Queries.User.GetUserList;
using SiaAdmin.WebUI.Models;

namespace SiaAdmin.WebUI.Controllers
{
	[Authorize("AdminOnly")]
	public class EmployeeController : BaseController
	{
		[HttpGet("kullanici-listesi")]
		public async Task<IActionResult> EmployeeList()
		{

			var userListResponse = await Mediator.Send(new GetUserListRequest());
			var model = new EmployeeListViewModel() { UserListViewModelsList = userListResponse.UserListViewModels };
			ViewBag.RolesList = await getRolesList();
			return View(model);
		}

		[HttpPost("kullanici-onayla")]
		public async Task<IActionResult> ApproveUser(ApproveEmployeeRequest approveEmployeeRequest)
		{
			var response = await Mediator.Send(approveEmployeeRequest);
			if (response.Succeded)
			{
				return RedirectToAction("EmployeeList");
			}

			return Ok(response);
		}

		private async Task<List<SelectListItem>> getRolesList()
		{
			var roles = await Mediator.Send(new GetSiaRolesRequest());
			List<SelectListItem> rolesList = new List<SelectListItem>();
			foreach (var item in roles.SiaRolesViewModels)
			{
				rolesList.Add(new SelectListItem()
				{
					Value = item.Id.ToString(),
					Text = item.RoleType
				});
			}

			return rolesList;
		}

	}
}
