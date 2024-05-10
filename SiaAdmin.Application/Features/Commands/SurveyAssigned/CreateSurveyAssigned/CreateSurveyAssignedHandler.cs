using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Enums;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Interfaces.ConvertExcel;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Commands.SurveyAssigned.CreateSurveyAssigned
{
    public class CreateSurveyAssignedHandler : IRequestHandler<CreateSurveyAssignedRequest, CreateSurveyAssignedResponse>
    {
        private IMapper _mapper;
        private ISurveyAssignedWriteRepository _surveyAssignedWriteRepository;
        private ISurveyReadRepository _surveyReadRepository;
        private IConvertExcelFile _convertExcelFile;
        private IUserReadRepository _userReadRepository;
        private IExcelService _excelService;
        public CreateSurveyAssignedHandler(IMapper mapper, ISurveyAssignedWriteRepository surveyAssignedWriteRepository, IExcelService excelService, ISurveyReadRepository surveyReadRepository, IConvertExcelFile convertExcelFile, IUserReadRepository userReadRepository)
        {
            _mapper = mapper;
            _surveyAssignedWriteRepository = surveyAssignedWriteRepository;
            _excelService = excelService;
            _surveyReadRepository = surveyReadRepository;
            _convertExcelFile = convertExcelFile;
            _userReadRepository = userReadRepository;
        }

        public async Task<CreateSurveyAssignedResponse> Handle(CreateSurveyAssignedRequest request, CancellationToken cancellationToken)
        {
            
            var excelList = _excelService.readExcel(request.ExcelFile);
            var survey = _surveyReadRepository.GetByIdAsync(request.SurveyId);
            var mappingMapSurveyAssigned = _mapper.Map<DTOs.SurveyAssigned.MapSurveyAssigned>(survey.Result);

            if (excelList.TableName.Equals(ExcelTable.InternalGUID.ToString()))
            {
                var guids = _convertExcelFile.convertedInternalGuidDTO(excelList);
               
                
                foreach (var item in guids.Guids)
                {
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
                    await _surveyAssignedWriteRepository.SaveAsync();

                }
            } 

            if (excelList.TableName.Equals(ExcelTable.SurveyUserGUID.ToString()))
            {
                var guids = _convertExcelFile.convertedUserGuidDTO(excelList);
                var convertedGuids = _userReadRepository.ConvertInternalGuid(guids.Guids); 
                foreach (var item in convertedGuids)
                {
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
                    await _surveyAssignedWriteRepository.SaveAsync();

                }
            }

            return new CreateSurveyAssignedResponse() { Succeeded = true, Message = "Anket Atama İşlemi Başarıyla Gerçekleştirildi." };
        }


    }
}
