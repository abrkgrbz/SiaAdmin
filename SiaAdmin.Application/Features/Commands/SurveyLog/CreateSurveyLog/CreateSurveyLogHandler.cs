using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Interfaces.ConvertExcel;
using SiaAdmin.Application.Interfaces.Excel;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Commands.SurveyLog.CreateSurveyLog
{
    public class CreateSurveyLogHandler : IRequestHandler<CreateSurveyLogRequest, CreateSurveyLogResponse>
    {
        private ISurveyLogWriteRepository _surveyLogWriteRepository;
        private IExcelService _excelService;
        private IConvertExcelFile _convertExcelFile;
        private ISurveyReadRepository _surveyReadRepository;
        public CreateSurveyLogHandler(ISurveyLogWriteRepository surveyLogWriteRepository, IExcelService excelService, IConvertExcelFile convertExcelFile, ISurveyReadRepository surveyReadRepository)
        {
            _surveyLogWriteRepository = surveyLogWriteRepository;
            _excelService = excelService;
            _convertExcelFile = convertExcelFile;
            _surveyReadRepository = surveyReadRepository;
        }

        public async Task<CreateSurveyLogResponse> Handle(CreateSurveyLogRequest request, CancellationToken cancellationToken)
        {
            var excelFile = _excelService.readExcel(request.ExcelFile);
            var convertedExcelFile = _convertExcelFile.convertedPointDTO(excelFile);
            var list = new List<Domain.Entities.Models.SurveyLog>();
            for (int i = 0; i < convertedExcelFile.CountData - 1; i++)
            {
                int surveyPoints = _surveyReadRepository.GetByIdAsync(convertedExcelFile.SurveyID[i], false).Result.SurveyPoints;
                list.Add(new Domain.Entities.Models.SurveyLog()
                {
                    Active = 1,
                    Approved = 1,
                    SurveyPoints = surveyPoints,
                    TimeStamp = DateTime.Now,
                    SurveyId = convertedExcelFile.SurveyID[i],
                    SurveyUserGuid = convertedExcelFile.Guids[i],

                });

            }
            await _surveyLogWriteRepository.AddOrUpdateAsync(list);
            await _surveyLogWriteRepository.SaveAsync(request.UserId,true);
            return new CreateSurveyLogResponse() { Message = "Puan Ekleme İşlemi Başarıyla Gerçekleştirildi", Succeeded = true };
        }
    }
}
