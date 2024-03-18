using AutoMapper;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.DL;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using MISA.FRESHERWEB032023.DL.Repository.ExportImportRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.ExportAndImportService
{
    public class EmulationExportService : BaseExportService<EmulationMapping, EmulationExcelDto>, IEmulationExportService
    {
        public EmulationExportService(IExportRepository exportRepository, IMapper mapper) : base(exportRepository, mapper)
        {
        }
    }
}
