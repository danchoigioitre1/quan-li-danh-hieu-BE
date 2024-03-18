using MISA.FRESHER032023.COMMON.Constant;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHER032023.COMMON;
using MISA.FRESHERWEB032023.DL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MISA.FRESHER032023.COMMON.Models;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;

namespace MISA.FRESHERWEB032023.BL.Services
{
    public abstract class BaseService<TEntity, TEntityCreate, TEntityDto, TEntityUpdateParam> : IBaseService<TEntity, TEntityCreate, TEntityDto, TEntityUpdateParam>
    {

        protected readonly IBaseRepository<TEntity, TEntityCreate> _baseRepository;
        protected readonly IMapper _mapper;
        public BaseService(IBaseRepository<TEntity, TEntityCreate> baseRepository, IMapper mapper)
        {
            _baseRepository = baseRepository;
            _mapper = mapper;
        }


        public virtual async Task CreateAsync(TEntityCreate entityCreate)
        {
            await _baseRepository.CreateAsync(entityCreate);
        }

        public virtual async Task DeleteAsync(Guid id)
        {
            // Kiểm tra dữ liệu từ DB trước khi xóa
            var emulation = await _baseRepository.GetAsync(id);
            // Nếu null thì throw 1 exception
            if (emulation == null) throw new NotFoundException(Resource.ResourceManager.GetString("errorNotFound") ?? "", errorCode: ErrorCodeConst.BusinessNotFound);
            else
            {
                // Gọi đến Repo xóa bản ghi trong DB
                await _baseRepository.DeleteAsync(id);
            }
        }

        public virtual async Task<TEntityDto?> GetAsync(Guid id)
        {
            // Kiểm tra dữ liệu từ DB trước khi get
            var entity = await _baseRepository.GetAsync(id);
            if (entity == null)
            {
                // Nếu null thì throw 1 exception
                throw new NotFoundException(Resource.ResourceManager.GetString("errorNotFound") ?? "", errorCode: ErrorCodeConst.BusinessNotFound);
            }
            // Chuyển đổi kiểu cho phù hợp với kết quả trả về
            var entityDto = _mapper.Map<TEntityDto>(entity);
            return entityDto;
        }

        public virtual Task UpdateAsync(Guid id, TEntityUpdateParam entityUpdateParam)
        {
            throw new NotImplementedException();
        }
    }
}
