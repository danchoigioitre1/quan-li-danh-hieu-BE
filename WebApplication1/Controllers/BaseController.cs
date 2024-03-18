using Microsoft.AspNetCore.Mvc;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Params;
using MISA.FRESHERWEB032023.BL.Services;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;

namespace MISA.FresherWeb032023.Controllers
{
    [ApiController]
    public abstract class BaseController<TEntity, TEntityCreate, TEntityDto, TEntityUpdateParam> : ControllerBase
    {
        /// <summary>
        /// Base service 
        /// </summary>
        protected readonly IBaseService<TEntity, TEntityCreate, TEntityDto, TEntityUpdateParam> _baseService;
        public BaseController(IBaseService<TEntity, TEntityCreate, TEntityDto, TEntityUpdateParam> baseService)
        {
            _baseService = baseService;
        }
        /// <summary>
        /// Api lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="id">mã id</param>
        /// <returns>1 Bản ghi</returns>
        /// CreatedBy: NVAanh - MF1618
        [HttpGet("{id}")]
        public async Task<TEntityDto?> GetById(Guid id)
        {
            // Gọi đến base service và lấy dữ liệu trả về
            var entityDto = await _baseService.GetAsync(id);
            return entityDto;
        }

        /// <summary>
        /// Api xóa 1 bản ghi theo id
        /// </summary>
        /// <param name="id">mã id</param>
        /// <returns></returns>
        /// CreatedBy: NVAanh - MF1618
        [HttpDelete("{id}")]
        public async Task DeleteAsync(Guid id)
        {
            // gọi đến base service và xóa bản ghi
            await _baseService.DeleteAsync(id);
        }

        /// <summary>
        /// Api thêm mới 1 bản ghi theo id
        /// </summary>
        /// <param name="entityCreate">bản ghi mới</param>
        /// <returns></returns>
        /// CreatedBy: NVAanh - MF1618
        [HttpPost]
        public async Task Post([FromBody] TEntityCreate entityCreate)
        {
            // Gọi đến service để thêm mới 1 bản ghi
            await _baseService.CreateAsync(entityCreate);
        }

        /// <summary>
        /// Api cập nhật 1 bản ghi theo id
        /// </summary>
        /// <param name="id">mã id</param>
        /// <returns></returns>
        /// CreatedBy: NVAanh - MF1618
        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody] TEntityUpdateParam entityUpdateParam)
        {
            // Gọi đến service để cập nhật 1 bản ghi 
            await _baseService.UpdateAsync(id, entityUpdateParam);
        }

    }
}
