using AutoMapper;
using MediatR;
using SiaAdmin.Application.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SiaAdmin.Application.Features.Commands.User.CreateBlockUser
{
    public class CreateBlockUserHandler:IRequestHandler<CreateBlockUserRequest,CreateBlockUserResponse>
    {
        private IBlockListWriteRepository _blockListWriteRepository;
        private readonly IMapper _mapper;
        public CreateBlockUserHandler(  IMapper mapper, IBlockListWriteRepository blockListWriteRepository)
        {
            _mapper = mapper;
            _blockListWriteRepository = blockListWriteRepository;
        }

        public async Task<CreateBlockUserResponse> Handle(CreateBlockUserRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var mappingProfile = _mapper.Map<Domain.Entities.Models.BlockList>(request);
                var data = await _blockListWriteRepository.AddAsync(mappingProfile);
                return new CreateBlockUserResponse() { isAdded = data,message = "Kullanıcı başarıyla blocklandı!"};
            }
            catch (Exception e)
            {
                return new CreateBlockUserResponse() { isAdded = false, message = "Bir hata oluştu!" };
            }
           
        }
    }
}
