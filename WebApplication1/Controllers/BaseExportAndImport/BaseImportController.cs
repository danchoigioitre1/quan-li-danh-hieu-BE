using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using MISA.FRESHER032023.COMMON.Helper;
using MISA.FRESHERWEB032023.BL.Services;
using MISA.FRESHERWEB032023.BL.Services.ExportAndImportService;
using System.Drawing;
using static Dapper.SqlMapper;
using static MISA.FresherWeb032023.Controllers.Middlewares.EmulationImportController;

namespace MISA.FresherWeb032023.Controllers.BaseExportAndImport
{
    [ApiController]
    public class BaseImportController<TEntity> : ControllerBase
    {

        protected readonly IBaseImportService<TEntity> _baseImportService;
        public BaseImportController(IBaseImportService<TEntity> baseImportService)
        {
            _baseImportService = baseImportService;
        }

        [HttpGet("mappingTable")]
        public async Task<List<TEntity>> GetMappingTable(string subSystemCode)
        {
            var entity = await _baseImportService.GetMappingTable(subSystemCode);
            return entity;

        }


        [HttpPost("checkFileSize")]
        public bool CheckFileSize(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Không tìm thấy file");
            }

            long fileSizeInBytes = file.Length;
            const long maxSizeInBytes = 5 * 1024 * 1024; // Kích thước tối đa là 5MB

            if (fileSizeInBytes > maxSizeInBytes)
            {
                return false;
            }

            // Tiếp tục xử lý file nếu kích thước hợp lệ

            return true;
        }

        [HttpPost("checkFileType")]
        public bool CheckFileType(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new Exception("Không tìm thấy file");
            }

            string[] allowedExtensions = { ".xls", ".xlsx" }; // Các định dạng file được phép

            string fileExtension = Path.GetExtension(file.FileName);

            if (!allowedExtensions.Contains(fileExtension))
            {
                return false;
            }

            // Tiếp tục xử lý file nếu định dạng hợp lệ

            return true;
        }


        [HttpPost("getSheetList")]
        public string[] getSheetList(IFormFile file)
        {
            // Lưu file Excel vào thư mục tạm thời
            string tempFilePath = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            // Khởi tạo Workbook từ file Excel
            Workbook workbook = new Workbook(tempFilePath);

            // Lấy danh sách các sheet trong Workbook
            int sheetCount = workbook.Worksheets.Count;
            string[] sheetNames = new string[sheetCount];

            for (int i = 0; i < sheetCount; i++)
            {
                sheetNames[i] = workbook.Worksheets[i].Name;
            }

            // Xóa file tạm thời
            System.IO.File.Delete(tempFilePath);

            return sheetNames;
        }


        [HttpPost("getHeaderList")]

        public async Task<List<string>> GetHeaderList(IFormFile file, int headerIndex, int sheetIndex)
        {


            using (MemoryStream fileStream = new MemoryStream())
            {
                file.CopyTo(fileStream);

                Workbook workbook = new Workbook(fileStream);

                Worksheet worksheet = workbook.Worksheets[sheetIndex];

                // Lấy dữ liệu từ các ô trong sheet
                Cells cells = worksheet.Cells;

                int columnCount = cells.MaxDataColumn + 1;

                int row = headerIndex;
                var headerList = new List<string>();
                for (int col = 0; col < columnCount; col++)
                {
                    // Đọc giá trị từ ô trong sheet
                    string cellValue = cells[row, col].StringValue;

                    // Xử lý giá trị đọc được từ ô
                    headerList.Add(cellValue);
                }
                return headerList;


            }


        }


       
    }
}
