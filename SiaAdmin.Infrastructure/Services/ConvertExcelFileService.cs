using SiaAdmin.Application.Interfaces.ConvertExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.DTOs.Excel;
using SiaAdmin.Application.Exceptions;
using SiaAdmin.Application.Enums;

namespace SiaAdmin.Infrastructure.Services
{
    public class ConvertExcelFileService:IConvertExcelFile  
    {

        public UserGuidDTO convertedUserGuidDTO(DataTable excelTable)
        {
            UserGuidDTO convertedData = new UserGuidDTO();
            var table = excelTable;
            try
            {
                table.Rows.RemoveAt(0);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        convertedData.Guids.Add(Guid.Parse(table.Rows[i][j].ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApiException("Beklenmedik bir hata!");
            }

            return convertedData;
        }

        public PointDTO convertedPointDTO(DataTable excelTable)
        {
            PointDTO convertedData = new PointDTO();
            convertedData.Guids.Clear();
            convertedData.SurveyID.Clear();
           
            var table = excelTable;
            convertedData.CountData = excelTable.Rows.Count;
            try
            {
                table.Rows.RemoveAt(0); 

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        
                        convertedData.Guids.Add(Guid.Parse(table.Rows[i][j].ToString()));
                        j++;
                        convertedData.SurveyID.Add(Convert.ToInt32(table.Rows[i][j]));
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApiException("Beklenmedik bir hata!");
            }

            return convertedData;
        }

        public InternalGuidDTO convertedInternalGuidDTO(DataTable excelTable)
        {
            var convertedData = new InternalGuidDTO();
            convertedData.Guids.Clear();
            var table = excelTable;
            try
            {
                table.Rows.RemoveAt(0);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    for (int j = 0; j < table.Columns.Count; j++)
                    {
                        convertedData.Guids.Add(Guid.Parse(table.Rows[i][j].ToString()));
                    }
                }
            }
            catch (Exception e)
            {
                throw new ApiException("Beklenmedik bir hata!");
            }
            return convertedData;
        }

     
    }
}
