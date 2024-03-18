using MISA.FRESHER032023.COMMON.Enums;
using MISA.FRESHER032023.COMMON.Models;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Params;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.EmulationService
{
    public interface IEmulationService : IBaseService<EmulationBase, EmulationCreate, EmulationDto, EmulationUpdateParam>
    {
        /// <summary>
        /// Hàm lấy 1 bản ghi theo id
        /// </summary>
        /// <param name="emulationId">mã id của danh hiệu</param>
        /// <returns>EmulationDto</returns>
        /// CreatedBy: NVAnh - MF1618 
        //Task<EmulationDto?> GetAsync(Guid emulationId);

        /// <summary>
        /// Service cập nhật 1 bản ghi
        /// </summary>
        /// <param name="emulationId">mã id của bản ghi</param>
        /// <param name="emulationUpdateParam">các field mới của bản ghi</param>
        /// CreatedBy : NVAnh - MF1618
        //Task UpdateAsync(Guid emulationId, EmulationUpdateParam emulationUpdateParam);

        /// <summary>
        /// Xóa 1 bản ghi
        /// </summary>
        /// <param name="emulationId">mã id của bản ghi</param>
        /// CreatedBy : NVAnh - MF1618
        //Task DeleteAsync(Guid emulationId);

        /// <summary>
        /// Tạo mới 1 bản ghi
        /// </summary>
        /// <param name="emlationCreate"></param>
        /// CreatedBy : NVAnh - MF1618
        //Task CreateAsync(EmulationCreate emlationCreate);

        /// <summary>
        /// Lấy danh sách bản ghi theo filter vả phân trang
        /// </summary>
        /// <param name="pageSize"> Số bản ghi trên trang </param>
        /// <param name="pageNumber"> Số thứ tự của trang </param>
        /// <param name="searchName"> chuỗi trong tên </param>
        /// <param name="searchCode"> chuỗi trong code </param>
        /// <param name="EmulationLevel"> cấp bậc </param>
        /// <param name="EmulationType"> thể loại </param>
        /// <param name="EmulationTarget"> đối tượng </param>
        /// <param name="EmulationStatus"> trạng thaí </param>
        /// <returns>danh sách bản ghi đã lọc </returns>
        /// CreatedBy : NVAnh - MF1618
        Task<List<EmulationBase>?> GetPageAsync(int pageSize, int pageNumber, string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus);

        /// <summary>
        /// Hàm kiểm tra sự tồn tại của mã code trong DB
        /// </summary>
        /// <param name="emulationCode">mã code của bản ghi</param>
        /// <returns>sự tồn tại của mã code trong DB</returns>
        /// CreatedBy : NVAnh - MF1618
        Task<int> CheckCodeAsync(string emulationCode, string id);

        /// <summary>
        /// Hàm xóa nhiều bản ghi cung lúc
        /// </summary>
        /// <param name="listId">danh sách id của các bản ghi muốn xóa</param>
        /// CreatedBy : NVAnh - MF1618
        Task MultipleDeleteAsync(List<Guid> listId);

        /// <summary>
        /// Hàm thay đổi trạng thái nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="listId">danh sách id của các bản ghi muốn sửa</param>
        /// <param name="status">trạng thái muốn sửa</param>
        /// CreatedBy : NVAnh - MF1618
        Task MultipleChangeStatusAsync(Guid[] listId, int status);

        /// <summary>
        /// Lấy tổng số bản ghi được tìm thấy
        /// </summary>
        /// <param name="searchName"> chuỗi trong tên </param>
        /// <param name="searchCode"> chuỗi trong code </param>
        /// <param name="EmulationLevel"> cấp bậc </param>
        /// <param name="EmulationType"> thể loại </param>
        /// <param name="EmulationTarget"> đối tượng </param>
        /// <param name="EmulationStatus"> trạng thaí </param>
        /// <returns>Tổng số bản ghi</returns>
        /// CreatedBy: NVAnh - MF1618
        Task<int> GetTotalAsync(string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus);
        
        /// <summary>
        /// Hàm lấy dữ liệu từ bảng importMapping 
        /// </summary>
        /// <returns></returns>
        Task<List<dynamic>> GetImportMapping();    

    }
}
