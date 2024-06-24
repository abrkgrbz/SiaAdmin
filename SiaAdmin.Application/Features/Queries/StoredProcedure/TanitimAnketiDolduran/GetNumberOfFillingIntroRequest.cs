using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace SiaAdmin.Application.Features.Queries.StoredProcedure.TanitimAnketiDolduran
{
    public class GetNumberOfFillingIntroRequest:IRequest<GetNumberOfFillingIntroResponse>
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
