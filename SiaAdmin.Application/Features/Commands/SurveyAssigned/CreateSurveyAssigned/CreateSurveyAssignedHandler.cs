
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Enums;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.ConvertExcel;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned
{
    public class CreateSurveyAssignedHandler : IRequestHandler<CreateSurveyAssignedRequest, CreateSurveyAssignedResponse>
    {
        private readonly IMapper _mapper;
        private readonly ISurveyAssignedWriteRepository _surveyAssignedWriteRepository;
        private readonly ISurveyAssignedReadRepository _surveyAssignedReadRepository;
        private readonly ISurveyReadRepository _surveyReadRepository;
        private readonly IConvertExcelFile _convertExcelFile;
        private readonly IUserReadRepository _userReadRepository;
        private readonly IExcelService _excelService;
        private static DateTime _lastOperationTime = DateTime.MinValue;
        private static readonly object _lock = new object();
        public CreateSurveyAssignedHandler(IMapper mapper, ISurveyAssignedWriteRepository surveyAssignedWriteRepository, IExcelService excelService, ISurveyReadRepository surveyReadRepository, IConvertExcelFile convertExcelFile, IUserReadRepository userReadRepository, ISurveyAssignedReadRepository surveyAssignedReadRepository)
        {
            _mapper = mapper;
            _surveyAssignedWriteRepository = surveyAssignedWriteRepository;
            _excelService = excelService;
            _surveyReadRepository = surveyReadRepository;
            _convertExcelFile = convertExcelFile;
            _userReadRepository = userReadRepository;
            _surveyAssignedReadRepository = surveyAssignedReadRepository;
        }

        //Bu kod revize edilmiştir
        public async Task<CreateSurveyAssignedResponse> Handle(CreateSurveyAssignedRequest request, CancellationToken cancellationToken)
        {
            lock (_lock)
            {
                if ((DateTime.Now - _lastOperationTime).TotalSeconds < 30)
                {
                    throw new ApiException("30 saniye içinde tekrar kayıt atamazsın!");
                }
                _lastOperationTime = DateTime.Now;
            }
            var excelList = _excelService.readExcel(request.ExcelFile);
            var survey = await _surveyReadRepository.GetByIdAsync(request.SurveyId);
            var mappingMapSurveyAssigned = _mapper.Map<DTOs.SurveyAssigned.MapSurveyAssigned>(survey);
            var isInternalGuid = excelList.TableName.Equals(ExcelTable.InternalGUID.ToString());

            if (isInternalGuid)
            {
                var guids = _convertExcelFile.convertedInternalGuidDTO(excelList);
                var duplicatedGuids = guids.Guids
                    .GroupBy(g => g)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                if (duplicatedGuids.Any())
                {
                    throw new ApiException($"Excel'de Mükerrer Kayıt  : {string.Join(",", duplicatedGuids)}");
                }
                bool checkDuplicate = _surveyAssignedReadRepository.CheckDuplicatedRecordByUserGUID(request.SurveyId, guids.Guids);
                if (checkDuplicate)
                {
                    throw new ApiException("Mükerrer kayıt bulunmakta!");
                }
                var surveyAssignedList = guids.Guids.Select(item => new Domain.Entities.Models.SurveyAssigned()
                {
                    SurveyText = mappingMapSurveyAssigned.SurveyText,
                    SurveyDescription = mappingMapSurveyAssigned.SurveyDescription,
                    SurveyLink = mappingMapSurveyAssigned.SurveyLink,
                    SurveyPoints = request.SurveyPoints,
                    InternalGuid = item,
                    SurveyId = request.SurveyId,
                    SurveyLinkText = mappingMapSurveyAssigned.SurveyLinkText,
                    SurveyStartDate = request.SurveyStartDate,
                    SurveyValidity = request.SurveyValidity,
                    Timestamp = DateTime.Now,
                    SurveyActive = mappingMapSurveyAssigned.SurveyActive
                }).ToList();

                await _surveyAssignedWriteRepository.AddRangeAsync(surveyAssignedList);
                await _surveyAssignedWriteRepository.SaveAsync(request.userId, true);
            }
            else
            {
                var guids = _convertExcelFile.convertedUserGuidDTO(excelList);
                var convertedGuids = _userReadRepository.ConvertInternalGuid(guids.Guids);
                var duplicatedGuids=convertedGuids
                    .GroupBy(g => g)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                if (duplicatedGuids.Any())
                {
                    throw new ApiException($"Excel'de Mükerrer Kayıt : {string.Join(",", duplicatedGuids)}");
                }
                bool checkDuplicate=_surveyAssignedReadRepository.CheckDuplicatedRecordByUserGUID(request.SurveyId, convertedGuids);
                if (checkDuplicate)
                {
                    throw new ApiException("Mükerrer kayıt bulunmakta!");
                }

                var surveyAssignedList = convertedGuids.Select(item => new Domain.Entities.Models.SurveyAssigned()
                {
                    SurveyText = mappingMapSurveyAssigned.SurveyText,
                    SurveyDescription = mappingMapSurveyAssigned.SurveyDescription,
                    SurveyLink = mappingMapSurveyAssigned.SurveyLink,
                    SurveyPoints = request.SurveyPoints,
                    InternalGuid = item,
                    SurveyId = request.SurveyId,
                    SurveyLinkText = mappingMapSurveyAssigned.SurveyLinkText,
                    SurveyStartDate = request.SurveyStartDate,
                    SurveyValidity = request.SurveyValidity,
                    Timestamp = DateTime.Now,
                    SurveyActive = mappingMapSurveyAssigned.SurveyActive
                }).ToList();

                await _surveyAssignedWriteRepository.AddRangeAsync(surveyAssignedList);
                await _surveyAssignedWriteRepository.SaveAsync(request.userId, true);
            }

            return new CreateSurveyAssignedResponse() { Succeeded = true, Message = "Anket Atama İşlemi Başarıyla Gerçekleştirildi." };
        }
    }
}
