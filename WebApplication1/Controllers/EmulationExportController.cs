using Microsoft.AspNetCore.Mvc;
using MISA.FresherWeb032023.Controllers.BaseExportAndImport;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Services.ExportAndImportService;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;

namespace MISA.FresherWeb032023.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class EmulationExportController : BaseExportController<EmulationMapping, EmulationExcelDto>
    {
        public EmulationExportController(IEmulationExportService emulationExportService) : base(emulationExportService)
        {
        }
    }
}
