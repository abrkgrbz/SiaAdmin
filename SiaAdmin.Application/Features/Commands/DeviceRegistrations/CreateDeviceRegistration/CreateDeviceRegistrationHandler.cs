using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories.DeviceRegistrations;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.DeviceRegistrations.CreateDeviceRegistration
{
    public class CreateDeviceRegistrationHandler:IRequestHandler<CreateDeviceRegistrationRequest,Response<bool>>
    {
        private IDeviceRegistrationsWriteRepository _deviceRegistrationsWriteRepository; 
        private IDeviceRegistrationsReadRepository _deviceRegistrationsReadRepository;
        public CreateDeviceRegistrationHandler(IDeviceRegistrationsWriteRepository deviceRegistrationsWriteRepository, IDeviceRegistrationsReadRepository deviceRegistrationsReadRepository)
        {
            _deviceRegistrationsWriteRepository = deviceRegistrationsWriteRepository;
            _deviceRegistrationsReadRepository = deviceRegistrationsReadRepository;
        }

        public async Task<Response<bool>> Handle(CreateDeviceRegistrationRequest request, CancellationToken cancellationToken)
        {
            //var isTokenExist = _deviceRegistrationsReadRepository.GetAll(false)
            //    .Any(x => x.DeviceIdToken == request.DeviceIdToken);
            //if (isTokenExist)
            //{
            //    throw new ApiException("Bu cihaz token bilgisi bulunmakta");
            //}
             
            var result =await _deviceRegistrationsWriteRepository.AddAsync(new Domain.Entities.Models.DeviceRegistrations()
            {
                InternalGUID = request.InternalGUID,
                TimeStamp = request.TimeStamp,
                DeviceIdToken = request.DeviceIdToken,
                Active = true
            });
            if (result)
            {
                await _deviceRegistrationsWriteRepository.SaveAsync(project: false);
                return new Response<bool>(true,"Token bilgisi kaydedildi");
            }

            throw new ApiException("Beklenmedik hata!");
        }


    }
}
