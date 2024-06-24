using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Wrappers
{
    public class DataTableReponse<T>:Response<T>
    {
        public int RecordsFiltered { get; set; }
        public int RecordTotal { get; set; }
        public DataTableReponse(T data,int recordTotal,int recordsFiltered)
        {
            this.RecordTotal = recordTotal;
            this.RecordsFiltered = recordsFiltered;
            this.Data = data;
            this.Message = null;
            this.Succeeded = true;
            this.Errors = null;
        }
    }
}
