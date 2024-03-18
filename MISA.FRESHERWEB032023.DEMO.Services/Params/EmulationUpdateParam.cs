using MISA.FRESHER032023.COMMON.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHERWEB032023.BL.Params
{
    public class EmulationUpdateParam
    {
        public EmulationStatus EmulationStatus { get; set; }
        public string? EmulationName { get; set; }
        public string? EmulationCode { get; set; }
        public EmulationTarget EmulationTarget { get; set; }
        public EmulationLevel EmulationLevel { get; set; }
        public EmulationType EmulationType { get; set; }
    }
}
