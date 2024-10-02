using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Commands.Survey.CreateSurvey
{
    public class CreateSurveyHandler:IRequestHandler<CreateSurveyRequest,CreateSurveyResponse>
    {
        private readonly ISurveyWriteRepository _surveyWriteRepository;
        private IMapper _mapper;
        public CreateSurveyHandler(ISurveyWriteRepository surveyWriteRepository, IMapper mapper)
        {
            _surveyWriteRepository = surveyWriteRepository;
            _mapper = mapper;
        }

        public async Task<CreateSurveyResponse> Handle(CreateSurveyRequest request, CancellationToken cancellationToken)
        {
            try
            {
                
                var mappingProfile = _mapper.Map<Domain.Entities.Models.Survey>(request);
                await _surveyWriteRepository.AddAsync(mappingProfile);
                await _surveyWriteRepository.SaveAsync(request.UserId,true);
                return new() { Succeeded = true,Message = "Anket Ekleme İşlemi Başarıyla Gerçekleştirildi."};
            }
            catch (Exception e)
            {                                         
                return new() { Succeeded = false, Message = "Anket Ekleme İşlemi Sırasında Bir Hatayla Karşılaşıldı." };
            }
            
        }
    }
}
