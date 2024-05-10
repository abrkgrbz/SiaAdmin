using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.BlockList.GetAllBlockList
{
    public class GetAllBlockListQueryHandler:IRequestHandler<GetAllBlockListQueryRequest,GetAllBlockListQueryResponse>
    {
        private IBlockListReadRepository _blockListReadRepository;

        public GetAllBlockListQueryHandler(IBlockListReadRepository blockListReadRepository)
        {
            _blockListReadRepository = blockListReadRepository;
        }

        public async Task<GetAllBlockListQueryResponse> Handle(GetAllBlockListQueryRequest request, CancellationToken cancellationToken)
        {
            var blockList=await _blockListReadRepository.GetAll(false).ToListAsync();
            return new()
            {
                BlockList = blockList
            };
        }
    }
}
