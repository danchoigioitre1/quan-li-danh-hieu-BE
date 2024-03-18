using AutoMapper;
using MISA.FRESHERWEB032023.DL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.ExportAndImportService
{
    public class BaseImportService<TEntity> : IBaseImportService<TEntity>
    {
        protected readonly IBaseImportRepository<TEntity> _baseImportRepository;
        protected readonly IMapper _mapper;
        public BaseImportService(IBaseImportRepository<TEntity> baseImportRepository, IMapper mapper)
        {
            _baseImportRepository = baseImportRepository;
            _mapper = mapper;
        }
        public async Task<List<TEntity>> GetMappingTable(string subSystemCode)
        {
            var entity = await _baseImportRepository.GetMappingTable(subSystemCode);
            return entity;
        }

    }
}
