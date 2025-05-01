using MediatR;
using System.ComponentModel.DataAnnotations;

namespace SiaAdmin.Application.Features.Commands.Survey.CreateSurvey
{
    public class CreateSurveyRequest:IRequest<CreateSurveyResponse>
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string SurveyText { get; set; }
        [Required]
        public string SurveyDescription { get; set; }
        [Required]
        public string SurveyLink { get; set; } 
        public string SurveyLinkText { get; set; }
        public DateTime? SurveyValidity { get; set; }
        public int SurveyActive { get; set; } = 1;
        public DateTime? SurveyStartDate { get; set; }
        [Required]
        public int SurveyPoints { get; set; }
        [Required]
        public int Mandatory { get; set; } 
        [Required]
        public string DBAddress { get; set; }
        [Required]
        public string DPPass { get; set; }
        public string? DPUser { get; set; }
        public string UserId { get; set; }
    }
}
