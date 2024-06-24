using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiaAdmin.Application.Repositories;

namespace SiaAdmin.Application.ProcedureRepositories.ToplamAnketBilgisi
{
    public interface IGetTotalSurveyInformation:IStoredProcedureRepository<Domain.Entities.Procedure.ToplamAnketBilgisi>
    {
    }
}
