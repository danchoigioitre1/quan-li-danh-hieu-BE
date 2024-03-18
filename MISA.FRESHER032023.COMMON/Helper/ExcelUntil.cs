using Aspose.Cells;
using MISA.FRESHER032023.COMMON.Enums;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Helper
{
    public class SetStyleItem
    {
        public void ExcelUntil(Cell cell, string fontType, int type, Color BgColor, bool isBold = false, bool isItalic = false, bool isUnderlined = false, int fontSize = 14)
        {
            // bold italic underline size alight fonttype border
            Style style = cell.GetStyle();
            Alignment(type, style);
            style.Font.Name = fontType; // Tên font chữ
            style.Font.IsBold = isBold; // In đậm
            style.Font.Size = fontSize;
            style.Pattern = BackgroundType.Solid;
            style.ForegroundColor = BgColor;

            style.VerticalAlignment = TextAlignmentType.Center;

            style.Borders[BorderType.LeftBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.LeftBorder].Color = Color.Gray;
            style.Borders[BorderType.TopBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.TopBorder].Color = Color.Gray;
            style.Borders[BorderType.RightBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.RightBorder].Color = Color.Gray;
            style.Borders[BorderType.BottomBorder].LineStyle = CellBorderType.Thin;
            style.Borders[BorderType.BottomBorder].Color = Color.Gray;

            cell.SetStyle(style);

        }
        private static void Alignment(int type, Style style)
        {
            switch (type)
            {
                case 1:
                    style.HorizontalAlignment = TextAlignmentType.Left;
                    break;
                case 2:
                    style.HorizontalAlignment = TextAlignmentType.Right;
                    break;
                case 3:
                    style.HorizontalAlignment = TextAlignmentType.Center;
                    break;
            }
        }
        public void SetColWidth(Cells cells, int indexCol, int colWidth)
        {
            cells.SetColumnWidth(indexCol, colWidth);
        }
        public void SetRowHeight(Cells cells, int row, double height)
        {
            cells.SetRowHeight(row, height);
        }

        public void Merge(Cells cells, int headerLength)
        {
            cells.Merge(0, 0, 1, headerLength + 1);
        }

        public void SetFontColor(Cell cell, Color color)
        {
            var style = cell.GetStyle();
            style.Font.Color = color;
            cell.SetStyle(style);
        }
        public void CommentCells(CommentCollection comments, int row, int col, string note)
        {

            int commentIndex1 = comments.Add(row, col);
            Comment comment1 = comments[commentIndex1];
            comment1.Note = note;
        }
    }
}
