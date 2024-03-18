using Microsoft.Extensions.Configuration;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL.Repository.ExportImportRepo
{
    public class ExportRepository : BaseExportRepository<EmulationMapping>, IExportRepository
    {
        public ExportRepository(IConfiguration configuration) : base(configuration)
        {
        }
    }
}
