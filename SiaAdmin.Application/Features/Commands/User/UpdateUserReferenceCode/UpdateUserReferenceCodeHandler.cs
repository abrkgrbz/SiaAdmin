using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Application.Wrappers;

namespace SiaAdmin.Application.Features.Commands.User.UpdateUserReferenceCode
{
    public class UpdateUserReferenceCodeHandler : IRequestHandler<UpdateUserReferenceCodeRequest, Response<bool>>
    {
        private IUserWriteRepository _userWriteRepository;
        private IUserReadRepository _userReadReadRepository;
        private ISurveyLogWriteRepository _surveyLogWriteRepository;
        private ISurveyLogReadRepository _surveyLogReadRepository;

        public UpdateUserReferenceCodeHandler(IUserWriteRepository userWriteRepository, IUserReadRepository userReadReadRepository, ISurveyLogWriteRepository surveyLogWriteRepository, ISurveyLogReadRepository surveyLogReadRepository)
        {
            _userWriteRepository = userWriteRepository;
            _userReadReadRepository = userReadReadRepository;
            _surveyLogWriteRepository = surveyLogWriteRepository;
            _surveyLogReadRepository = surveyLogReadRepository;
        }

        public async Task<Response<bool>> Handle(UpdateUserReferenceCodeRequest request, CancellationToken cancellationToken)
        {
            int result = await UpdateUserReferenceCode(request.ReferenceCode, request.InternalGUID);
            if (result > 0)
            {
                return new Response<bool>(true, "Referans ödülünüz sistem tarafından verilmiştir.");
            }

            return new Response<bool>(false, "Başarısız İşlem.");
        }

        private async Task<int> UpdateUserReferenceCode(string referenceCode,Guid internalGUID)
        {
            try
            {
                var user = await _userReadReadRepository
                    .GetWhere(u => u.InternalGuid == internalGUID)
                    .FirstOrDefaultAsync();

                if (user == null)
                {
                    return 0; // Kullanıcı bulunamadı
                }

                var referredByOnceki = user.ReferredBy ?? "X";

                var durum = await _userReadReadRepository
                    .GetWhere(u => u.MyReference == referenceCode && u.Active == 1 && u.InternalGuid != internalGUID)
                    .CountAsync();

                if (durum == 1)
                {
                    var kisidatetime = user.RegistrationDate.AddDays(2);

                    if (kisidatetime >= DateTime.Now && referredByOnceki == "X")
                    {
                        user.ReferredBy = referenceCode;
                        await _userWriteRepository.SaveAsync(null, false);

                        var refpuanikisisi = await _userReadReadRepository
                            .GetWhere(u => u.MyReference == referenceCode, false)
                            .Select(u => u.SurveyUserGuid)
                            .FirstOrDefaultAsync();

                        if (refpuanikisisi != Guid.Empty)
                        {
                            var antifraud = await _surveyLogReadRepository
                                .GetWhere(s =>
                                    s.SurveyId == 0 && s.SurveyUserGuid == refpuanikisisi &&
                                    s.Text == "Arkadaşını Getir Referans Ödülü")
                                .CountAsync();

                            if (antifraud <= 8)
                            {
                                var surveyLog = new Domain.Entities.Models.SurveyLog()
                                {
                                    SurveyUserGuid = refpuanikisisi,
                                    SurveyId = 0,
                                    SurveyPoints = 50,
                                    Active = 1,
                                    TimeStamp = DateTime.Now,
                                    Approved = 1,
                                    Text = "Arkadaşını Getir Referans Ödülü",
                                    Extended = internalGUID
                                };

                                await _surveyLogWriteRepository.AddAsync(surveyLog);
                                await _surveyLogWriteRepository.SaveAsync(null, false);

                                return 1; // Başarılı işlem
                            }
                        }
                    }
                }

                return 0; // Başarısız işlem
            }
            catch (Exception e)
            {
                throw new ApiException("Beklenmedik bir hata");
            }
        }
    }
}
