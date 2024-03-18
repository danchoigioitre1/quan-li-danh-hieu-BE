using MISA.FRESHER032023.COMMON.Models.Base;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL.Repository.ExportImportRepo
{
    public interface IImportRepository : IBaseImportRepository<EmulationMapping>
    {
        Task<List<string>> GetListEmulationCode();
        Task MultipleAddEmlation(List<EmulationExcel> list);
    }
}
