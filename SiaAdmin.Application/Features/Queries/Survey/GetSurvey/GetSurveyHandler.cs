using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.DTOs.Survey;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.Survey.GetSurvey
{
    public class GetSurveyHandler : IRequestHandler<GetSurveyRequest, GetSurveyResponse>
    {
        private readonly ISurveyReadRepository _surveyReadRepository;

        public GetSurveyHandler(ISurveyReadRepository surveyReadRepository)
        {
            _surveyReadRepository = surveyReadRepository;
        }

        public async Task<GetSurveyResponse> Handle(GetSurveyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                 
                var projects =await  _surveyReadRepository.GetAll(false)
                    .Where(p => p.SurveyActive==1)
                    .OrderBy(p => p.Id)
                    .Select(p => new SurveyDto()
                    {
                        id = p.Id,
                        name = p.Id.ToString()
                    })
                    .ToListAsync(cancellationToken);

                return new GetSurveyResponse()
                {
                    success = true,
                    data = projects
                };
            }
            catch (Exception ex)
            { 
                return new GetSurveyResponse
                {
                    success = false,
                    data = new List<SurveyDto>(),
                    message = "Projeler yüklenirken bir hata oluştu"
                };
            }
        }
    }

}
