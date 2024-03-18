using Aspose.Cells;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MISA.FRESHER032023.COMMON.Enums;
using MISA.FRESHER032023.COMMON.Helper;
using MISA.FRESHER032023.COMMON.Models;
using MISA.FresherWeb032023.Controllers;
using MISA.FRESHERWEB032023.BL.Dto;
using MISA.FRESHERWEB032023.BL.Params;
using MISA.FRESHERWEB032023.BL.Services.EmulationService;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MySqlConnector;
using System.Data;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication1.Controllers
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class EmulationController : BaseController<EmulationBase, EmulationCreate, EmulationDto, EmulationUpdateParam>
    {
        //public readonly string _connetionString;
        public readonly IEmulationService _emulationService;

        public EmulationController(IEmulationService emulationService) : base(emulationService)
        {
            // Khởi tại service từ IEmulationService
            _emulationService = emulationService;
        }
        // GET: api/<EmolyeeController>
        /// <summary>
        ///  Api lấy danh sách bản ghi 
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageNumber"></param>
        /// <param name="searchName"></param>
        /// <param name="searchCode"></param>
        /// <param name="EmulationLevel"></param>
        /// <param name="EmulationType"></param>
        /// <param name="EmulationTarget"></param>
        /// <param name="EmulationStatus"></param>
        /// <returns>Danh sách bản ghi đã được lọc</returns>
        /// CreatedBy: NVAanh - MF1618
        [HttpGet]
        public async Task<List<EmulationBase>?> GetList(int pageSize, int pageNumber, string? searchName, string? searchCode, int EmulationLevel, int EmulationType, int EmulationTarget, int EmulationStatus)
        {
            // Gọi đến service để lấy danh sách bản ghi
            var list = await _emulationService.GetPageAsync(pageSize, pageNumber, searchName, searchCode, EmulationLevel, EmulationType, EmulationTarget, EmulationStatus);
            // Trả về danh sách bản ghi
            return list;
        }

        // CheckCode api/<EmolyeeController>/5

        /// <summary>
        /// Api kiểm tra sự tồn tại của mã code trong DB
        /// </summary>
        /// <param name="code">mã code cần check</param>
        /// <param name="id">id của bản ghi(dùng cho Th sửa)</param>
        /// <returns>sự tồn tại của má code trong DB</returns>
        /// CreatedBy: NVAanh - MF1618 
        [HttpGet("check/{code}")]
        public async Task<int> CheckCode(string code, string? id)
        {
            // Gọi đến Service để kiểm tra sự tồn tại cảu mã code
            var isValidCode = await _emulationService.CheckCodeAsync(code, id);
            // Trả về kết quả
            return isValidCode;
        }

        // DELETE api/MultipleDelete<EmolyeeController>/5
        /// <summary>
        /// Api Xóa nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="list">Danh sách mã id</param>
        /// CreatedBy: NVAanh - MF1618 
        [HttpPost("MultipleDelete")]
        public async Task MultipleDelete([FromBody] List<Guid> list)
        {
            // Gọi đến service để thực hiện xóa nhiều bản ghi
            await _emulationService.MultipleDeleteAsync(list);

        }

        // PUT api/<EmolyeeController>/MultipleChangeStatus
        /// <summary>
        /// Api thay đổi trạng thái của nhiều bản ghi cùng lúc
        /// </summary>
        /// <param name="listId">Danh sách mã id</param>
        /// <param name="status">TRạng thái muốn thay đổi</param>
        /// CreatedBy: NVAanh - MF1618
        [HttpPost("MultipleChangeStatus")]
        public async Task MultiplePut([FromBody] Guid[] listId, int status)
        {
            //  Gọi đến service để thực hiện thay đổi trạng thái nhiều bản ghi
            await _emulationService.MultipleChangeStatusAsync(listId, status);
        }

        /// <summary>
        /// Api lấy tổng số bản ghi được tìm thấy
        /// </summary>
        /// <param name="searchName"></param>
        /// <param name="searchCode"></param>
        /// <param name="EmulationLevel"></param>
        /// <param name="EmulationType"></param>
        /// <param name="EmulationTarget"></param>
        /// <param name="EmulationStatus"></param>
        /// <returns>tổng số bản ghi tương ứng</returns>
        [HttpGet("Total")]
        public async Task<int> GetTotalData(string? searchName, string? searchCode, int EmulationLevel, int EmulationType, int EmulationTarget, int EmulationStatus)
        {
            // Gọi đến service để lấy tổng số bản ghi
            var total = await _emulationService.GetTotalAsync(searchName, searchCode, EmulationLevel, EmulationType, EmulationTarget, EmulationStatus);
            // Trả về danh sách bản ghi
            return total;
        }




        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel()
        {
            try
            {
                var list = await _emulationService.GetPageAsync(5, 1, "", "", 0, 0, 0, 0);
                // Tạo một đối tượng Workbook
                Workbook workbook = new Workbook();

                // Lấy trang tính đầu tiên trong Workbook
                Worksheet worksheet = workbook.Worksheets[0];

                // Ghi dữ liệu vào các ô
                //worksheet.Cells["A1"].PutValue("Hello");
                //worksheet.Cells["B1"].PutValue("World!");
                worksheet.Cells[0, 0].PutValue("Bảng danh hiệu khen thưởng");
                worksheet.Cells.Merge(0, 0, 1, 7);
                for (int i = 0; i < list.Count; i++)
                {
                    worksheet.Cells[i + 3, 0].PutValue(list[i].EmulationName);
                    worksheet.Cells[i + 3, 1].PutValue(list[i].EmulationCode);
                }


                Style style = worksheet.Cells[0, 0].GetStyle();
                style.HorizontalAlignment = TextAlignmentType.Center; // Căn giữa theo chiều ngang
                style.Font.Name = "Arial"; // Tên font chữ
                style.Font.IsBold = true; // In đậm
                style.Font.Size = 15;
                worksheet.Cells[0, 0].SetStyle(style);

                // Lưu Workbook thành tệp Excel
                var stream = new System.IO.MemoryStream();
                workbook.Save(stream, SaveFormat.Xlsx);

                // Trả về tệp Excel như là một phản hồi
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "output.xlsx");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


        

        [HttpGet("getImportMapping")]
        public async Task<List<dynamic>> GetImportMapping()
        {
            var data = await _emulationService.GetImportMapping();
            return data;
        }

        [HttpGet("getExcelTemplate")]

        public async Task<IActionResult> ExportExcelTemplate()
        {
            try
            {
                //var list = await _emulationService.GetPageAsync(5, 1, "", "", 0, 0, 0, 0);

                // Tạo một đối tượng Workbook
                Workbook workbook = new Workbook();

                // Lấy trang tính đầu tiên trong Workbook
                Worksheet worksheet = workbook.Worksheets[0];
                var startRowIndex = 4;
                // Ghi dữ liệu vào các ô

                worksheet.Cells[0, 0].PutValue("Bảng danh hiệu khen thưởng");
                worksheet.Cells.Merge(0, 0, 1, 7);
                //var setTitleStyle = new ExcelUntil();
                //setTitleStyle.ExcelUntil(worksheet.Cells[0, 0]);
                // Chỉnh style co title
                //worksheet.Cells.StandardWidth = 16;
                //Style style = worksheet.Cells[0, 0].GetStyle();
                //style.HorizontalAlignment = TextAlignmentType.Center; // Căn giữa theo chiều ngang
                //style.Font.Name = "Arial"; // Tên font chữ
                //style.Font.IsBold = true; // In đậm
                //style.Font.Size = 15;
                //worksheet.Cells[0, 0].SetStyle(style);

                // Header
                var importMappingData = await _emulationService.GetImportMapping();
                worksheet.Cells[startRowIndex, 0].PutValue("STT");
                for (int i = 0; i < importMappingData.Count; i++)
                {
                    string[] header = importMappingData[i].Mapping.Split(';');
                    worksheet.Cells[startRowIndex, importMappingData[i].IndexInTemplate].PutValue(header[0]);

                }
                // Style cho header
                for (int i = 0; i <= importMappingData.Count; i++)
                {
                    Style headerStyle = worksheet.Cells[startRowIndex, i].GetStyle();
                    headerStyle.HorizontalAlignment = TextAlignmentType.Left;
                    headerStyle.Font.Name = "Arial"; // Tên font chữ
                    headerStyle.Font.Size = 10;
                    headerStyle.Font.IsBold = true; // In đậm
                    worksheet.Cells.SetColumnWidth(i, 20);
                    worksheet.Cells[startRowIndex, i].SetStyle(headerStyle);
                }

                // Lưu Workbook thành tệp Excel
                var stream = new System.IO.MemoryStream();
                workbook.Save(stream, SaveFormat.Xlsx);

                // Trả về tệp Excel như là một phản hồi
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "output.xlsx");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu có
                return StatusCode(500, $"Lỗi: {ex.Message}");
            }
        }


    }
}
