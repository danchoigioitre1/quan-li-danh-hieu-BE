using Aspose.Cells;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using MISA.FRESHER032023.COMMON.Helper;
using MISA.FRESHER032023.COMMON.Models.Base;
using MISA.FresherWeb032023.Controllers.BaseExportAndImport;
using MISA.FRESHERWEB032023.BL.Services.ExportAndImportService;
using MISA.FRESHERWEB032023.DL.Entity.ExportAndImport;
using System.Drawing;
using System.Reflection;
using System.Xml.Linq;

namespace MISA.FresherWeb032023.Controllers.Middlewares
{
    [Route("api/v1/[controller]s")]
    [ApiController]
    public class EmulationImportController : BaseImportController<EmulationMapping>
    {
        public readonly IEmulationImportService _emulationImportService;
        public EmulationImportController(IEmulationImportService emulationImportService) : base(emulationImportService)
        {
            _emulationImportService = emulationImportService;
        }




        [HttpPost("getDataFromExcel")]

        public async Task<List<EmulationExcel>> getDataFromExcel(IFormFile file, string subSystemCode, int headerIndex, int sheetIndex)
        {
            var mappingTable = await _emulationImportService.GetMappingTable(subSystemCode);
            var headerColIndex = await GetColIndex(file, subSystemCode, headerIndex, sheetIndex);

            var emulationList = new List<EmulationExcel>();


            // Lưu file Excel vào thư mục tạm thời
            string tempFilePath = Path.GetTempFileName();
            using (var fileStream = new FileStream(tempFilePath, FileMode.Create))
            {
                file.CopyTo(fileStream);
            }

            // Khởi tạo Workbook từ file Excel
            Workbook workbook = new Workbook(tempFilePath);
            Worksheet worksheet = workbook.Worksheets[sheetIndex];
            // Lấy danh sách các sheet trong Workbook
            Cells cells = worksheet.Cells;
            int rowCount = cells.MaxDataRow + 1;

            var ConverData = new ConverEmulationDataToEnum();
            for (int i = headerIndex + 1; i < rowCount; i++)
            {
                var emulation = new EmulationExcel();
                foreach (FieldIndex field in headerColIndex)
                {
                    var stringValue = cells[i, field.IndexColumn].StringValue;
                    switch (field.Field)
                    {
                        case "EmulationName":
                            emulation.EmulationName = stringValue;
                            break;
                        case "EmulationCode":
                            emulation.EmulationCode = stringValue;
                            break;
                        case "EmulationTarget":
                            emulation.EmulationTarget = ConverData.ConverTarget(stringValue);
                            break;
                        case "EmulationLevel":
                            emulation.EmulationLevel = ConverData.ConverLevel(stringValue);
                            break;
                        case "EmulationType":
                            emulation.EmulationType = ConverData.ConverType(stringValue);
                            break;
                        case "EmulationStatus":
                            emulation.EmulationStatus = ConverData.ConverStatus(stringValue);
                            break;
                    }

                }
                emulationList.Add(emulation);
            }

            // Xóa file tạm thời
            System.IO.File.Delete(tempFilePath);

            return emulationList;
        }



        [HttpPost("statisticalEmulation")]
        public async Task<StatisticalEmulation> statisticalEmulation(IFormFile file, string subSystemCode, int headerIndex, int sheetIndex)
        {
            var listData = await getDataFromExcel(file, subSystemCode, headerIndex, sheetIndex);
            var statisticalEmulation = new StatisticalEmulation();
            statisticalEmulation.Total = listData.Count;
            statisticalEmulation.Invalid = 0;
            statisticalEmulation.Valid = 0;

            var checkFunction = new CheckObjectEmptyFieldsOrNegativeValues();
            foreach (var item in listData)
            {

                var isInvalid = checkFunction.CheckData(item);
                if (isInvalid)
                {
                    statisticalEmulation.Invalid++;
                }
                else
                {
                    statisticalEmulation.Valid++;
                }

            }


            return statisticalEmulation;

        }



        [HttpPost("exportErrorExcel")]
        public async Task<IActionResult> exportErrorExcel(IFormFile file, string subSystemCode, int headerIndex, int sheetIndex)
        {
            var mappingTable = await _emulationImportService.GetMappingTable(subSystemCode);
            var colIndex = await GetColIndex(file, subSystemCode, headerIndex, sheetIndex);
            var listData = await getDataFromExcel(file, subSystemCode, headerIndex, sheetIndex);
            var headerList = await GetHeaderList(file, headerIndex, sheetIndex);
            var styleCell = new SetStyleItem();
            var commentEmptyCell = new CommentEmptyCells();
            using (MemoryStream fileStream = new MemoryStream())
            {
                file.CopyTo(fileStream);

                Workbook workbook = new Workbook(fileStream);

                Worksheet worksheet = workbook.Worksheets[0];
                Cells cells = worksheet.Cells;
                Cell cell = worksheet.Cells[headerIndex, headerList.Count];
                cell.PutValue("Thông báo lỗi");

                CommentCollection comments = workbook.Worksheets[sheetIndex].Comments;

                for (int i = 0; i < listData.Count; i++)
                {
                    var errorMsg = "";
                    if (listData[i].EmulationName == "")
                    {
                        var col = colIndex[colIndex.FindIndex(item => item.Field == "EmulationName")].IndexColumn;
                        errorMsg += "Tên danh hiệu không được để trống";
                        styleCell.SetFontColor(cells[headerIndex + i + 1, col], Color.Red);
                        styleCell.CommentCells(comments, headerIndex + i + 1, col, "Tên danh hiệu không được để trống");
                      
                    }
                    if (listData[i].EmulationCode == "")
                    {
                        var col = colIndex[colIndex.FindIndex(item => item.Field == "EmulationCode")].IndexColumn;
                        errorMsg += "\n" + "Mã danh hiệu không được để trống";
                   
                        styleCell.SetFontColor(cells[headerIndex + i + 1, col], Color.Red);
                        styleCell.CommentCells(comments, headerIndex + i + 1, col, "Mã danh hiệu không được để trống");
                    }
                    if (listData[i].EmulationLevel == -1)
                    {
                        var col = colIndex[colIndex.FindIndex(item => item.Field == "EmulationLevel")].IndexColumn;
                        errorMsg += "\n" + "Cấp Khen thưởng không hợp lệ";
                      
                        styleCell.SetFontColor(cells[headerIndex + i + 1, col], Color.Red);
                        commentEmptyCell.Comment(cells[headerIndex + i + 1, col].StringValue,comments ,headerIndex + i + 1, col, "Cấp khen thưởng không được để trống");
                    }
                    if (listData[i].EmulationType == -1)
                    {
                        var col = colIndex[colIndex.FindIndex(item => item.Field == "EmulationType")].IndexColumn;
                        errorMsg += "\n" + "Loại Khen thưởng không hợp lệ";
                        
                        styleCell.SetFontColor(cells[headerIndex + i + 1, col], Color.Red);
                        commentEmptyCell.Comment(cells[headerIndex + i + 1, col].StringValue, comments, headerIndex + i + 1, col, "Loại khen thưởng không được để trống");
                    }
                    if (listData[i].EmulationTarget == -1)
                    {
                        var col = colIndex[colIndex.FindIndex(item => item.Field == "EmulationTarget")].IndexColumn;
                        errorMsg += "\n" + "Đối tượng Khen thưởng không hợp lệ";
                       
                        styleCell.SetFontColor(cells[headerIndex + i + 1, col], Color.Red);
                        commentEmptyCell.Comment(cells[headerIndex + i + 1, col].StringValue, comments, headerIndex + i + 1, col, "Đối tượng khen thưởng không được để trống");
                    }
                    if (listData[i].EmulationStatus == -1)
                    {
                        var col = colIndex[colIndex.FindIndex(item => item.Field == "EmulationStatus")].IndexColumn;
                        errorMsg += "\n" + "Trạng thái không hợp lệ ";
                       
                        styleCell.SetFontColor(cells[headerIndex + i + 1, col], Color.Red);
                        commentEmptyCell.Comment(cells[headerIndex + i + 1, col].StringValue, comments, headerIndex + i + 1, col, "Trạng thái");
                    }
                    worksheet.Cells[headerIndex + i + 1, headerList.Count].PutValue(errorMsg);
                    styleCell.SetFontColor(cells[headerIndex + i + 1, headerList.Count], Color.Red);
                    Style style = worksheet.Cells[headerIndex + i + 1, headerList.Count].GetStyle();
                    style.IsTextWrapped = true;
                    style.Font.IsBold = true;
                    worksheet.Cells[headerIndex + i + 1, headerList.Count].SetStyle(style);
                    styleCell.SetColWidth(cells, headerList.Count, 40);
                }


                // Lưu Workbook thành tệp Excel
                var stream = new System.IO.MemoryStream();
                workbook.Save(stream, SaveFormat.Xlsx);

                // Trả về tệp Excel như là một phản hồi
                stream.Position = 0;
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "ErrorExcel");
            }
        }

        [HttpPost("checkHeaderList")]

        public async Task<bool> CheckHeaderList(IFormFile file, string subSystemCode, int headerIndex, int sheetIndex)
        {

            var mappingTable = await _emulationImportService.GetMappingTable(subSystemCode);

            var headerListFromExcel = await GetHeaderList(file, headerIndex, sheetIndex);

            var headerListFromMT = new List<string>();
            foreach (var item in mappingTable)
            {
                headerListFromMT.Add(item.Mapping);
            }

            var listCheck = new List<bool>();

            foreach (var mt in headerListFromMT)
            {
                for (int i = 0; i < headerListFromExcel.Count; i++)
                {
                    if (headerListFromExcel[i] == "STT")
                    {
                        continue;
                    }
                    if (mt.Contains(headerListFromExcel[i]))
                    {
                        listCheck.Add(true);
                        break;
                    }
                    if (i == headerListFromExcel.Count - 1)
                    {
                        listCheck.Add(false);
                    }
                }
            }
            var isValidHeader = true;
            foreach (var item in listCheck)
            {
                isValidHeader = isValidHeader && item;
            }
            return isValidHeader;

        }


        [HttpPost("getColIndex")]

        public async Task<List<FieldIndex>> GetColIndex(IFormFile file, string subSystemCode, int headerIndex, int sheetIndex)
        {

            var mappingTable = await _emulationImportService.GetMappingTable(subSystemCode);

            var headerListFromExcel = await GetHeaderList(file, headerIndex, sheetIndex);

            var headerListFromMT = new List<string>();
            foreach (var item in mappingTable)
            {
                headerListFromMT.Add(item.Mapping);
            }

            var listFieldHeaderIndex = new List<FieldIndex>();

            foreach (var mt in mappingTable)
            {
                for (int i = 0; i < headerListFromExcel.Count; i++)
                {

                    if (mt.Mapping.Contains(headerListFromExcel[i]))
                    {
                        var item = new FieldIndex();
                        item.Field = mt.Field;
                        item.IndexColumn = i;
                        listFieldHeaderIndex.Add(item);
                        break;
                    }
                }
            }

            return listFieldHeaderIndex;

        }



        [HttpGet("test")]
        public async Task<List<string>> getTest()
        {
            var list = await _emulationImportService.GetListEmulationCode();
            return list;
        }

        [HttpPost("multipleAddEmulation")]
        public async Task MultipleAddEmulation(List<EmulationExcel> list)
        {
            await _emulationImportService.MultipleAddEmlation(list);
        }


        [HttpPost("getValidData")]
        public async Task<List<EmulationExcel>> GetValidData(IFormFile file, string subSystemCode, int headerIndex, int sheetIndex)
        {
            var listData = await getDataFromExcel(file, subSystemCode, headerIndex, sheetIndex);
            var listValidData = new List<EmulationExcel>();

            var checkFunction = new CheckObjectEmptyFieldsOrNegativeValues();
            foreach (var item in listData)
            {

                var isInvalid = checkFunction.CheckData(item);
                if (!isInvalid)
                {
                    listValidData.Add(item);
                }


            }
            return listValidData;

        }

    }
}
