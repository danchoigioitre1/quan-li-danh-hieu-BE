using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.ExportAndImportService
{
    public interface IBaseImportService<TEntity>
    {
       
            /// <summary>
            /// Service lấy dữ liệu của bảng mapping
            /// </summary>
            /// <returns>danh sách dữ liệu của bảng mapping</returns>
            Task<List<TEntity>> GetMappingTable(string subSystemCode);
        
    }
}
