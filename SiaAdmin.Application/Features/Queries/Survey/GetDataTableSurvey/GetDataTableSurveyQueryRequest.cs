using MediatR; 

namespace SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey
{
    public class GetDataTableSurveyQueryRequest:IRequest<GetDataTableSurveyQueryResponse>
    {

        public int draw { get; set; }

        public int Length { get; set; }
        public int Start { get; set; }

        public string? orderColumnIndex { get; set; }
        public string? orderDir { get; set; }
        public string? orderColumnName { get; set; }
        public string? searchValue { get; set; }
    }
}
