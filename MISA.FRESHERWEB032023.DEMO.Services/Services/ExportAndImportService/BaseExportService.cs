using Aspose.Cells;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MISA.FRESHERWEB032023.DL;
using MISA.FRESHERWEB032023.DL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MISA.FRESHER032023.COMMON.Models.Base;
using System.Text.Json;
using MISA.FRESHER032023.COMMON.Enums;
using System.Reflection;
using MISA.FRESHERWEB032023.DL.Entity.Emualtion;
using MISA.FRESHER032023.COMMON.Helper;
using System.Drawing;

namespace MISA.FRESHERWEB032023.BL.Services.ExportAndImportService
{
    public class BaseExportService<TEntity, TEntityExcelDto> : IBaseExportService<TEntity, TEntityExcelDto>
    {
        protected readonly IBaseExportRepository<TEntity> _baseExportRepository;
        protected readonly IMapper _mapper;
        public BaseExportService(IBaseExportRepository<TEntity> baseExportRepository, IMapper mapper)
        {
            _baseExportRepository = baseExportRepository;
            _mapper = mapper;
        }


        public async void GetColValue(Worksheet worksheet, List<TEntityExcelDto> data , string subSystemCode)
        {
            int startIndexRow = 4;
            var mappingTable = await _baseExportRepository.GetMappingTable(subSystemCode);
            var headerList = new List<MappingTableBase>();
            for (var i = 0; i < mappingTable.Count; i++)
            {
                var header = _mapper.Map<MappingTableBase>(mappingTable[i]);
                headerList.Add(header);

            }
            // Header
            var style = new SetStyleItem();

            worksheet.Cells[startIndexRow, 0].PutValue("STT");
            style.ExcelUntil(worksheet.Cells[startIndexRow, 0], "Arial", 3, Color.LightGray, true, false, false, 10);
            style.SetRowHeight(worksheet.Cells, startIndexRow, 20);
            for (int i = 0; i < mappingTable.Count; i++)
            {
                worksheet.Cells[startIndexRow, headerList[i].IndexInTemplate].PutValue(headerList[i].Mapping.Split(';')[0]);


                style.ExcelUntil(worksheet.Cells[startIndexRow, headerList[i].IndexInTemplate], "Arial", 3, Color.LightGray, true, false, false, 10);
                style.SetColWidth(worksheet.Cells, headerList[i].IndexInTemplate, 20);

            }



            //FillDataToExcelColumn(worksheet, data, "EmulationName");1
            var test = ColumnDictionary(data);
            MapExport(worksheet, test, mappingTable);


        }
       
        public async Task<List<TEntity>> GetMappingTable(string subSystemCode)
        {
            var entity = await _baseExportRepository.GetMappingTable(subSystemCode);
            return entity;
        }

      


        protected void MapExport(Worksheet worksheet, Dictionary<string, List<object>> propertyDictionary, List<TEntity> mappingTable)
        {
            var mappingTableBase = new List<MappingTableBase>();

            foreach (var entity in mappingTable)
            {
                var item = _mapper.Map<MappingTableBase>(entity);
                mappingTableBase.Add(item);
            }

            // Mapping Data
            var headerRow = worksheet.Cells.Rows[4];

            // Get Header Names of excel file
            List<string> headerNames = new List<string>();

            foreach (Cell cell in headerRow)
            {
                headerNames.Add(cell.Value.ToString());
            }

            int columnIndex = 0;
            foreach (MappingTableBase item in mappingTableBase)
            {
                // Get header name of mapping table in db
                string mappingValue = item.Mapping;
                var header = headerNames.FindIndex(header => mappingValue.Contains(header));
                if (header >= 0)
                {
                    string fieldName = item.Field;
                    string enumName = item.EnumName;
                    int? alignmentType = item.Type;
                    int index = item.IndexInTemplate;
                    columnIndex += 1;
                    // Populate the column with the field data
                    if (propertyDictionary.TryGetValue(fieldName, out List<object> columnData))
                    {
                        for (int rowIndex = 5; rowIndex <= columnData.Count + 4; rowIndex++)
                        {
                            if (index == 1)
                            {
                                Cell indexCell = worksheet.Cells[rowIndex, 0];

                                indexCell.PutValue(rowIndex - 4);
                                var style = new SetStyleItem();

                                style.ExcelUntil(indexCell, "Arial", 3, Color.White, false, false, false, 10);

                            }
                            object data = (rowIndex - 5 < columnData.Count) ? columnData[rowIndex - 5] : null;
                            Cell cell = worksheet.Cells[rowIndex, index];
                            var styleCell = new SetStyleItem();

                            styleCell.ExcelUntil(cell, "Arial", (int)alignmentType, Color.White, false, false, false, 10);
                            var convert = new ConvertEnumToData();
                            switch (enumName)
                            {
                                case "EmulationTarget":
                                    data = convert.ConvertTarget((int)data);
                                    break;

                                case "EmulationLevel":
                                    data = convert.ConvertLevel((int)data);
                                    break;

                                case "EmulationType":
                                    data = convert.ConvertType((int)data);
                                    break;
                                case "EmulationStatus":
                                    data = convert.ConvertStatus((int)data);
                                    break;
                            }

                            cell.PutValue(data);
                        }
                    }
                }
            }
        }



        protected virtual Dictionary<string, List<object>> ColumnDictionary(List<TEntityExcelDto> data)
        {
            Dictionary<string, List<object>> propertyDictionary = new Dictionary<string, List<object>>();

            Type entityType = typeof(TEntityExcelDto);

            PropertyInfo[] properties = entityType.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                string propertyName = property.Name;
                List<object> propertyValues = new List<object>();

                foreach (TEntityExcelDto dto in data)
                {
                    object propertyValue = property.GetValue(dto);
                    propertyValues.Add(propertyValue);
                }

                propertyDictionary.Add(propertyName, propertyValues);
            }
            return propertyDictionary;
        }




    }
}
