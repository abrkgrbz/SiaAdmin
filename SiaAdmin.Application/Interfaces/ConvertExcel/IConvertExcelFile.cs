using SiaAdmin.Application.DTOs.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiaAdmin.Application.Interfaces.ConvertExcel
{
    public interface IConvertExcelFile 
    { 
        UserGuidDTO convertedUserGuidDTO(DataTable excelTable);
        PointDTO convertedPointDTO(DataTable excelTable);
        InternalGuidDTO convertedInternalGuidDTO(DataTable excelTable); 
    }
}
