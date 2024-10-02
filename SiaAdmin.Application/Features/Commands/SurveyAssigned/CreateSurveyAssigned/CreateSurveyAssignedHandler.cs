
using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
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

        public async Task<CreateSurveyAssignedResponse> Handle(CreateSurveyAssignedRequest request, CancellationToken cancellationToken)
        {

            var excelList = _excelService.readExcel(request.ExcelFile);
            var survey = await _surveyReadRepository.GetByIdAsync(request.SurveyId);
            var mappingMapSurveyAssigned = _mapper.Map<DTOs.SurveyAssigned.MapSurveyAssigned>(survey);
            if (excelList.TableName.Equals(ExcelTable.InternalGUID.ToString()))
            {
                var guids = _convertExcelFile.convertedInternalGuidDTO(excelList);


                foreach (var item in guids.Guids)
                {
                    var isExist = await _surveyAssignedReadRepository.GetSingleAsync(x => x.InternalGuid == item && x.SurveyId == request.SurveyId, false);
                    if (isExist != null)
                        throw new ApiException("Mükerrer Kayıt bulunmakta");
                    await _surveyAssignedWriteRepository.AddAsync(new Domain.Entities.Models.SurveyAssigned()
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
                        Timestamp = mappingMapSurveyAssigned.Timestamp,
                        SurveyActive = mappingMapSurveyAssigned.SurveyActive
                    });
               
                }
                await _surveyAssignedWriteRepository.SaveAsync(request.userId, true);

            }

            if (excelList.TableName.Equals(ExcelTable.SurveyUserGUID.ToString()))
            {
                var guids = _convertExcelFile.convertedUserGuidDTO(excelList);
                var convertedGuids = _userReadRepository.ConvertInternalGuid(guids.Guids);
                foreach (var item in convertedGuids)
                {

                    var isExist = await _surveyAssignedReadRepository.GetSingleAsync(x => x.InternalGuid == item && x.SurveyId == request.SurveyId, false);
                    if (isExist != null)
                        throw new ApiException("Mükerrer Kayıt bulunmakta");
                    await _surveyAssignedWriteRepository.AddAsync(new Domain.Entities.Models.SurveyAssigned()
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
                        Timestamp = mappingMapSurveyAssigned.Timestamp,
                        SurveyActive = mappingMapSurveyAssigned.SurveyActive
                    });


                }
                await _surveyAssignedWriteRepository.SaveAsync(request.userId,true);
            }

            return new CreateSurveyAssignedResponse() { Succeeded = true, Message = "Anket Atama İşlemi Başarıyla Gerçekleştirildi." };
        }


    }
}
