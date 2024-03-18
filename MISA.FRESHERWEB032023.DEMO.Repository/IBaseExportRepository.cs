using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL
{
    public interface IBaseExportRepository<TEntity>
    {
        /// <summary>
        /// Hàm mở kết nối tới DB
        /// </summary>
        /// <returns> Mở kết nối tới DB </returns>
        /// createdBy: NVAnh - MF1618
        Task<DbConnection> GetOpenConnectionAsync();

        /// <summary>
        /// Hàm lấy dữ liệu từ bảng mapping
        /// </summary>
        /// <returns></returns>
        Task<List<TEntity>> GetMappingTable(string subSystemCode);

    }
}
