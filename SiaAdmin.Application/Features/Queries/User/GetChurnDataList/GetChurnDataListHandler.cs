using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using SiaAdmin.Application.Interfaces.User;
using SiaAdmin.Application.Repositories;
using SiaAdmin.Domain.Entities.Models;

namespace SiaAdmin.Application.Features.Queries.User.GetChurnDataList
{
    public class GetChurnDataListHandler:IRequestHandler<GetChurnDataListRequest,GetChurnDataListResponse>
    {
        private readonly IMapper _mapper;
        private readonly IUserReadRepository _userService;


        public GetChurnDataListHandler(IMapper mapper, IUserReadRepository userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<GetChurnDataListResponse> Handle(GetChurnDataListRequest request, CancellationToken cancellationToken)
        {
            var data =_userService.GetListChurnData(); 
            int recordsFiltered = 0, recordTotal = 0;
            if (!string.IsNullOrEmpty(request.searchValue))
            {
                data = data.Where(x => x.InternalGUID.ToString().Contains(request.searchValue.ToLower()));
            }

            if (!string.IsNullOrEmpty(request.orderColumnName) && !string.IsNullOrEmpty(request.orderDir))
            {
                data = await OrderByField(data, request.orderColumnName, request.orderDir == "asc");
                recordsFiltered = data.Count();
                recordTotal = data.Count();
            }
            var reponseData = await data.Skip(request.Start).Take(request.Length).ToListAsync(); 
            return new GetChurnDataListResponse() {
                recordTotal = recordTotal,
                data = reponseData,
                recordsFiltered = recordsFiltered
            };
        }

        private async Task<IQueryable<T1>> OrderByField<T1>(IQueryable<T1> q, string sortField, bool ascending)
        {
            var param = Expression.Parameter(typeof(T1), "p");
            var prop = Expression.Property(param, sortField);
            var exp = Expression.Lambda(prop, param);
            string method = ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return (IQueryable<T1>)q.Provider.CreateQuery<T1>(mce);
        }
    }
}
