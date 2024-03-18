using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Params;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services
{
    /// <summary>
    /// Base service để tái sử dụng
    /// </summary>
    /// <typeparam name="TEntityDto"></typeparam>
    /// <typeparam name="TEntityCreateParam"></typeparam>
    /// <typeparam name="TEntityUpdateParam"></typeparam>
    public interface IBaseService<TEntity, TEntityCreate, TEntityDto, TEntityUpdateParam>
    {
        /// <summary>
        /// Service lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="id">mã id của bản ghi</param>
        /// <returns>TEntityDto</returns>
        /// CreatedBy : NVAnh - MF1618
        Task<TEntityDto?> GetAsync(Guid id);

        /// <summary>
        /// Service cập nhật 1 bản ghi
        /// </summary>
        /// <param name="id">mã id của bản ghi</param>
        /// <param name="entityUpdateParam">các field mới của bản ghi</param>
        /// CreatedBy : NVAnh - MF1618
        Task UpdateAsync(Guid id, TEntityUpdateParam entityUpdateParam);

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="id">mã id của bản ghi</param>
        /// CreatedBy : NVAnh - MF1618
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Tạo mới 1 bản ghi
        /// </summary>
        /// <param name="entityCreateParam"></param>
        /// CreatedBy : NVAnh - MF1618
        Task CreateAsync(TEntityCreate entityCreate);

    
    }
}
