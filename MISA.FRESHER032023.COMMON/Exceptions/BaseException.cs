using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Exceptions
{
    public class BaseException
    {
        #region Properties
        public int ErrorCode { get; set; }
        public string? UserMessage { get; set; }
        public string? DevMsg { get; set; }
        public string? TraceId { get; set; }
        public string? MoreInfo { get; set; }
        #endregion

        #region Methods
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
        #endregion
    }
}
