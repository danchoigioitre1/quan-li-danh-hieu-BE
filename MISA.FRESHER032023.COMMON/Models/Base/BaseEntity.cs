using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Models.Base
{
    public class BaseEntity
    {
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// người tạo
        /// </summary>
        public string? CreatedBy { get; set; }

        /// <summary>
        /// ngày sửa gần nhất
        /// </summary>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// người sửa gần nhất
        /// </summary>
        public string? ModifiedBy { get; set; }
    }
}
