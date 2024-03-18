using MISA.FRESHER032023.COMMON.Models.Base;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.ExportAndImportService
{
    public interface IEmulationImportService : IBaseImportService<EmulationMapping>
    {
        Task<List<string>> GetListEmulationCode();

        Task MultipleAddEmlation(List<EmulationExcel> list);
    }


}
