using MediatR;

namespace SiaAdmin.Application.Features.Commands.Survey.CreateSurvey
{
    public class CreateSurveyRequest:IRequest<CreateSurveyResponse>
    {
        public DTOs.Survey.CreateSurvey createSurvey { get; set; }
    }
}
