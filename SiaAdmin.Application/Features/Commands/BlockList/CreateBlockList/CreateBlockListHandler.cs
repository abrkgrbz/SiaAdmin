using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Commands.BlockList.CreateBlockList
{
    public class CreateBlockListHandler:IRequestHandler<CreateBlockListRequest,CreateBlockListResponse>
    {
        private IBlockListWriteRepository _blockListWriteRepository;

        public CreateBlockListHandler(IBlockListWriteRepository blockListWriteRepository)
        {
            _blockListWriteRepository = blockListWriteRepository;
        }

        public Task<CreateBlockListResponse> Handle(CreateBlockListRequest request, CancellationToken cancellationToken)
        {
            
            throw new NotImplementedException();
        }
    }
}
