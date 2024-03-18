using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Models.Base
{
    public class EmulationExcel
    {
        public int EmulationStatus { get; set; }
        public string? EmulationName { get; set; }
        public string? EmulationCode { get; set; }
        public int EmulationTarget { get; set; }
        public int EmulationLevel { get; set; }
        public int EmulationType { get; set; }
    }
}
