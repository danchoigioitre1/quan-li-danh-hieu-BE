using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using MISA.FRESHER032023.COMMON.Helper;
using MISA.FRESHERWEB032023.BL.Services;
using MISA.FRESHERWEB032023.BL.Services.ExportAndImportService;
using System.Drawing;
using static Dapper.SqlMapper;

namespace MISA.FresherWeb032023.Controllers.BaseExportAndImport
{
    [ApiController]
    public class BaseExportController<TEntity, TEntityExcelDto> : ControllerBase
    {
        protected readonly IBaseExportService<TEntity, TEntityExcelDto> _baseExportService;
        public BaseExportController(IBaseExportService<TEntity, TEntityExcelDto> baseExportService)
        {
            _baseExportService = baseExportService;
        }

        [HttpGet("mappingTable")]
        public async Task<List<TEntity>> GetMappingTable(string subSystemCode)
        {
            var entity = await _baseExportService.GetMappingTable(subSystemCode);
            return entity;
        }

        [HttpPost("exportExcel")]
        public async Task<IActionResult> ExportExcel(string title, string workbookName,string subSystemCode, [FromBody] List<TEntityExcelDto> data)
        {
            // Tạo một đối tượng Workbook
            Workbook workbook = new Workbook();

            // Lấy trang tính đầu tiên trong Workbook
            Worksheet worksheet = workbook.Worksheets[0];

            // Ghi dữ liệu vào các ô
            worksheet.Cells[0, 0].PutValue(title);
            var style = new SetStyleItem();

            style.ExcelUntil(worksheet.Cells[0, 0], "Arial", 3, Color.White, true, false, false, 20);

            var mappingTable = await _baseExportService.GetMappingTable(subSystemCode);
            style.Merge(worksheet.Cells, mappingTable.Count);


            _baseExportService.GetColValue(worksheet, data, subSystemCode);

            // Lưu Workbook thành tệp Excel
            var stream = new System.IO.MemoryStream();
            workbook.Save(stream, SaveFormat.Xlsx);

            // Trả về tệp Excel như là một phản hồi
            stream.Position = 0;
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", workbookName);
        }


    }
}
