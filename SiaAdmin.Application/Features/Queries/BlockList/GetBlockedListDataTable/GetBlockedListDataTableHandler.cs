using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Features.Queries.Survey.GetDataTableSurvey;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.Features.Queries.BlockList.GetBlockedListDataTable
{
    public class GetBlockedListDataTableHandler:IRequestHandler<GetBlockedListDataTableRequest,GetBlockedListDataTableResponse>
    {
        private IBlockListReadRepository _blockListReadRepository;

        public GetBlockedListDataTableHandler(IBlockListReadRepository blockListReadRepository)
        {
            _blockListReadRepository = blockListReadRepository;
        }

        public async Task<GetBlockedListDataTableResponse> Handle(GetBlockedListDataTableRequest request, CancellationToken cancellationToken)
        {
            var blockedUserList = _blockListReadRepository.GetAll(false);
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                blockedUserList = blockedUserList.Where(x => x.Data.ToLower().Contains(request.searchValue.ToLower())
                                                             || x.Note.ToLower().Contains(request.searchValue.ToLower())
                                                              );
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                blockedUserList = await _blockListReadRepository.OrderByField(blockedUserList, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = blockedUserList.Count();
                recordTotal = blockedUserList.Count();
            }

            var blockList = await blockedUserList.Skip(request.Start).Take(request.Length).ToListAsync();
            if (blockList == null) throw new Exception("Anket bulunamadı");
            return new GetBlockedListDataTableResponse()
            {
                recordTotal = recordTotal,
                recordsFiltered = recordsFiltered,
                data = blockList
            }; 
        }
    }
}
