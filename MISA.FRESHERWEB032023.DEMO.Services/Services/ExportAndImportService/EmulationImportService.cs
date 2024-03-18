using Aspose.Cells;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using MISA.FRESHER032023.COMMON.Models.Base;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using MISA.FRESHERWEB032023.DL.Repository.ExportImportRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.ExportAndImportService
{
    public class EmulationImportService : BaseImportService<EmulationMapping>, IEmulationImportService
    {
    
        protected readonly IImportRepository _importRepository;
        public EmulationImportService(IImportRepository importRepository, IMapper mapper) : base(importRepository, mapper)
        {
            _importRepository = importRepository;
        }

        public async Task<List<string>> GetListEmulationCode()
        {
            var listCode = await _importRepository.GetListEmulationCode();
            return listCode;
        }

        public async Task MultipleAddEmlation(List<EmulationExcel> list)
        {
           await _importRepository.MultipleAddEmlation(list);
        }
    }
}
