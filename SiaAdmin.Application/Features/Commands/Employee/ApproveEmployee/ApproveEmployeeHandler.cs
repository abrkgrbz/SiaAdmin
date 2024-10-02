using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Commands.Employee.ApproveEmployee
{
	public class ApproveEmployeeHandler : IRequestHandler<ApproveEmployeeRequest, ApproveEmployeeResponse>
	{
		private ISiaUserReadRepository _siaUserReadRepository;
		private ISiaUserWriteRepository _siaUserWriteRepository;
		private ISiaUserRoleWriteRepository _siaUserRoleWriteRepository;

		public ApproveEmployeeHandler(ISiaUserWriteRepository siaUserWriteRepository, ISiaUserReadRepository siaUserReadRepository, ISiaUserRoleWriteRepository siaUserRoleWriteRepository)
		{
			_siaUserWriteRepository = siaUserWriteRepository;
			_siaUserReadRepository = siaUserReadRepository;
			_siaUserRoleWriteRepository = siaUserRoleWriteRepository;
		}

		public async Task<ApproveEmployeeResponse> Handle(ApproveEmployeeRequest request, CancellationToken cancellationToken)
		{
			var user = await _siaUserReadRepository.GetByIdAsync(request.userId);
			var createRole = await _siaUserRoleWriteRepository.AddAsync(new SiaUserRole()
			{ UserId = request.userId, RoleId = request.roleId });
			await _siaUserRoleWriteRepository.SaveAsync();
			if (!createRole)
				throw new ApiException("Rol Eklenirken beklenmedik bir hata meydana geldi!");
			if (user == null)
				throw new ApiException("Böyle bir kullanıcı bulunamadı!");
			user.Approved = true;
			var result = _siaUserWriteRepository.Update(user);
			await _siaUserWriteRepository.SaveAsync();
			if (!result)
				throw new ApiException("Beklenmedik bir hata meydana geldi");
			return new ApproveEmployeeResponse() { Succeded = true };

		}
	}
}
