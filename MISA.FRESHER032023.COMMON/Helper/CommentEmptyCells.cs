using Aspose.Cells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Helper
{
    public class CommentEmptyCells
    {
        public void Comment(string value, CommentCollection comments, int row, int col, string comment)
        {

            if (value == "")
            {
                int commentIndex1 = comments.Add(row, col);
                Comment comment1 = comments[commentIndex1];
                comment1.Note = comment;
            }
        }
    }
}
