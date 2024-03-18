using AutoMapper;
using Dapper;
using Microsoft.VisualBasic;
using MISA.FRESHER032023.COMMON;
using MISA.FRESHER032023.COMMON.Constant;
using MISA.FRESHER032023.COMMON.Enums;
using MISA.FRESHER032023.COMMON.Exceptions;
using MISA.FRESHER032023.COMMON.Models;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Params;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHERWEB032023.DL.Repository.Emulation;
using MISA.FRESHERWEB032023.DL.Repository.EmulationRepo;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Services.EmulationService
{

    public class EmulationService : BaseService<EmulationBase, EmulationCreate, EmulationDto, EmulationUpdateParam>, IEmulationService
    {
        private readonly IEmulationRepository _emulationRepository;

        public EmulationService(IEmulationRepository emulationRepository, IMapper mapper) : base(emulationRepository, mapper)
        {
            // Lấy repository từ DL
            _emulationRepository = emulationRepository;
        }


        public override async Task CreateAsync(EmulationCreate emulationCreate)
        {
            // Kiểm tra độ dài tối đa của tham số, nếu vượt quá thì cắt bớt
            var maxLengthCode = (int)ValidateMaxLength.MaxLengthCode;
            var maxLengthName = (int)ValidateMaxLength.MaxLengthName;
            if (emulationCreate.EmulationCode != null && emulationCreate.EmulationCode.Length > maxLengthCode)
            {
                emulationCreate.EmulationCode = emulationCreate.EmulationCode.Substring(0, maxLengthCode);

            }
            if (emulationCreate.EmulationName != null && emulationCreate.EmulationName.Length > maxLengthName)
            {
                emulationCreate.EmulationName = emulationCreate.EmulationName.Substring(0, maxLengthName);
            }
            // Gửi dữ liệu tới Repo
            await _baseRepository.CreateAsync(emulationCreate);
        }


        public override async Task UpdateAsync(Guid emulationId, EmulationUpdateParam emulationUpdateParam)
        {
            // Kiểm tra dữ liệu từ DB trước khi xóa
            var emulation = await _baseRepository.GetAsync(emulationId);
            // Nếu null thì throw 1 exception
            if (emulation == null) { throw new NotFoundException(Resource.ResourceManager.GetString("errorNotFound") ?? "", errorCode: ErrorCodeConst.BusinessNotFound); };

            // Kiểm tra độ dài tối đa của tham số, nếu vượt quá thì cắt bớt
            var maxLengthCode = (int)ValidateMaxLength.MaxLengthCode;
            var maxLengthName = (int)ValidateMaxLength.MaxLengthName;
            if (emulationUpdateParam.EmulationCode != null && emulationUpdateParam.EmulationCode.Length > maxLengthCode)
            {
                emulationUpdateParam.EmulationCode = emulationUpdateParam.EmulationCode.Substring(0, maxLengthCode);

            }
            if (emulationUpdateParam.EmulationName != null && emulationUpdateParam.EmulationName.Length > maxLengthName)
            {
                emulationUpdateParam.EmulationName = emulationUpdateParam.EmulationName.Substring(0, maxLengthName);
            }

            // Chuyển đổi dữ liệu cho phù hợp vưới đầu vào của repo
            var updateEmulation = new EmulationBase
            {
                EmulationId = emulationId,
                EmulationCode = emulationUpdateParam.EmulationCode,
                EmulationLevel = emulationUpdateParam.EmulationLevel,
                EmulationName = emulationUpdateParam.EmulationName,
                EmulationStatus = emulationUpdateParam.EmulationStatus,
                EmulationTarget = emulationUpdateParam.EmulationTarget,
                EmulationType = emulationUpdateParam.EmulationType
            };

            // Gửi dữ liệu đến Repo để cập nhật
            await _baseRepository.UpdateAsync(emulationId, updateEmulation);
        }


        public async Task<int> CheckCodeAsync(string emulationCode, string id)
        {
            // Kiểm tra sự tồn tại của mã code trong DB
            var maxLengthCode = (int)ValidateMaxLength.MaxLengthCode;
            if (emulationCode.Length > maxLengthCode)
            {
                emulationCode = emulationCode.Substring(0, maxLengthCode);
            }
            // Gọi tới repo và nhận về kết quả
            var isValidCode = await _emulationRepository.CheckCode(emulationCode, id);
            // Trả về kết quả
            return isValidCode;
        }

        public async Task MultipleDeleteAsync(List<Guid> listId)
        {
            // Gọi đến Repo xóa nhiều bản ghi
            await _emulationRepository.MultipleDelete(listId);
        }

        public async Task<List<EmulationBase>?> GetPageAsync(int pageSize, int pageNumber, string? searchName, string? searchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus)
        {
            // Kiểm tra dữ liệu truyền vào 
            searchName ??= "";
            searchCode ??= "";
            EmulationLevel ??= (int?)FRESHER032023.COMMON.Enums.EmulationLevel.All;
            EmulationType ??= (int?)FRESHER032023.COMMON.Enums.EmulationType.All;
            EmulationTarget ??= (int?)FRESHER032023.COMMON.Enums.EmulationTarget.All;
            EmulationStatus ??= (int?)FRESHER032023.COMMON.Enums.EmulationStatus.All;

            // Kiểm tra độ dài tối đa của tham số, nếu vượt quá thì cắt bớt
            var maxLengthCode = (int)ValidateMaxLength.MaxLengthCode;
            var maxLengthName = (int)ValidateMaxLength.MaxLengthName;
            if (searchCode != null && searchCode.Length > maxLengthCode)
            {
                searchCode = searchCode.Substring(0, maxLengthCode);

            }
            if (searchCode != null && searchCode.Length > maxLengthName)
            {
                searchCode = searchCode.Substring(0, maxLengthName);
            }

            // Gọi đến Repo để lấy ra các bản ghi đã filter và phân trang
            var list = await _emulationRepository.GetPageAsync(pageSize, pageNumber, searchName, searchCode, EmulationLevel, EmulationType, EmulationTarget, EmulationStatus);
            // Trả về kết quả là danh sách đã lấy
            return list;
        }

        public async Task MultipleChangeStatusAsync(Guid[] listId, int status)
        {
            // Gọi đến Repo để thay đổi trạng thái của nhiều bản ghi
            await _emulationRepository.MultipleChangeStatusAsync(listId, status);
        }

        public async Task<int> GetTotalAsync(string? SearchName, string? SearchCode, int? EmulationLevel, int? EmulationType, int? EmulationTarget, int? EmulationStatus)
        {
            // Kiểm tra dữ liệu truyền vào 
            SearchName ??= "";
            SearchCode ??= "";
            EmulationLevel ??= (int?)FRESHER032023.COMMON.Enums.EmulationLevel.All;
            EmulationType ??= (int?)FRESHER032023.COMMON.Enums.EmulationType.All;
            EmulationTarget ??= (int?)FRESHER032023.COMMON.Enums.EmulationTarget.All;
            EmulationStatus ??= (int?)FRESHER032023.COMMON.Enums.EmulationStatus.All;

            // Kiểm tra độ dài tối đa của tham số, nếu vượt quá thì cắt bớt
            var maxLengthCode = (int)ValidateMaxLength.MaxLengthCode;
            var maxLengthName = (int)ValidateMaxLength.MaxLengthName;
            if (SearchCode != null && SearchCode.Length > maxLengthCode)
            {
                SearchCode = SearchCode.Substring(0, maxLengthCode);

            }
            if (SearchCode != null && SearchCode.Length > maxLengthName)
            {
                SearchCode = SearchCode.Substring(0, maxLengthName);
            }


            // Gọi đến Repo để lấy ra các bản ghi đã filter và phân trang
            var total = await _emulationRepository.GetTotalAsync(SearchName, SearchCode, EmulationLevel, EmulationType, EmulationTarget, EmulationStatus);
            // Trả về kết quả là danh sách đã lấy
            return total;
        }

        public async Task<List<dynamic>> GetImportMapping()
        {
            var data = await _emulationRepository.GetImportMapping();
            return data;
        }
    }
}
