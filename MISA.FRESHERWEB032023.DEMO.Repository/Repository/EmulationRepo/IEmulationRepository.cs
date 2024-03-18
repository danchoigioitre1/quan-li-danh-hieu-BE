using MISA.FRESHER032023.COMMON.Models;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL.Repository.EmulationRepo
{
    public interface IEmulationRepository: IBaseRepository<EmulationBase, EmulationCreate>
    {
        // Hàm check trong DB mã Code đã tồn tại chưa
        Task<int> CheckCode(string emulationCode, string id);

        // Hàm lấy bản ghi theo filter và phân trang
        Task<List<EmulationBase>?> GetPageAsync(int pageSize, int pageNumber, string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus);
        
        // Hàm xóa nhiều bản ghi
        Task MultipleDelete(List<Guid> listId);

        // Hàm thay đổi status của nhiều bản ghi cùng lúc
        Task MultipleChangeStatusAsync(Guid[] listId, int status);

        // Hàm lấy tổng số bản ghi được tìm thấy
        Task<int> GetTotalAsync(string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus);

        // Hàm lấy dữ liệu từ importMapping
        Task<List<dynamic>> GetImportMapping();
    }
}
