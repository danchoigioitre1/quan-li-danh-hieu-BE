using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL.Repository
{
   public interface IBaseRepository<TEntity, TEntityCreate>
    {
        /// <summary>
        /// hàm mở kết nối tới DB
        /// </summary>
        /// <returns> Mở kết nối tới DB </returns>
        /// createdBy: NVAnh - MF1618
        Task<DbConnection> GetOpenConnectionAsync();

        /// <summary>
        /// hàm lấy danh sách data
        /// </summary>
        /// <returns>Danh sách bản ghi</returns>
        /// createdBy: NVAnh - MF1618
        Task<List<TEntity>?> GetAllAsync();

        /// <summary>
        /// hàm lấy data theo id
        /// </summary>
        /// <param name="id">id của bản ghi</param>
        /// <returns>1 bản ghi</returns>
        /// createdBy: NVAnh - MF1618
        Task<TEntity?> GetAsync(Guid id);

        /// <summary>
        /// hàm update data
        /// </summary>
        /// <param name="id"> id của bản ghi muốn cập nhật</param>
        /// <param name="entity"> các field mới của entity </param>
        /// createdBy: NVAnh - MF1618
        Task UpdateAsync(Guid id, TEntity entity);

        /// <summary>
        /// hàm xóa bản ghi theo id
        /// </summary>
        /// <param name="id">id của bản ghi muốn xóa</param>
        /// <returns></returns>
        /// createdBy: NVAnh - MF1618
        Task<int> DeleteAsync(Guid id);
        
        /// <summary>
        /// hàm tạo bản ghi mới
        /// </summary>
        /// <param name="entityCreate">các field của entity mới tạo</param>
        /// <returns></returns>
        Task CreateAsync(TEntityCreate entityCreate);
    }
}
