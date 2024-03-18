using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.DL.Entity
{
    /// <summary>
    /// Lớp cơ sở
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string? CreatedBy { get; set; }
        /// <summary>
        /// Ngày sửa gần nhất
        /// </summary>
        public DateTime? ModifyDate { get; set; }
        /// <summary>
        /// Người sửa gần nhất
        /// </summary>
        public string? ModifyBy { get; set; }
    }
}
