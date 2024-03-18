using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Models.Base
{
    public class MappingTableBase
    {
        public Guid Id { get; set; }
        public string Field { get; set; }
        public string Mapping { get; set; }
        public int Type { get; set; }
        public int IsRequired { get; set; }
        public int IndexInTemplate { get; set; }
        public string EnumName { get; set; }
    }
}
