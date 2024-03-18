using MISA.FRESHER032023.COMMON.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Helper
{
    public class ConvertEnumToData
    {
        public string ConvertTarget(int targetEnum)
        {
            switch (targetEnum)
            {
                case (int)EmulationTarget.Family:
                    return "Gia đình";
                case (int)EmulationTarget.Collective:
                    return "Tập thể";
                case (int)EmulationTarget.Individual:
                    return "Cá nhân";
                case (int)EmulationTarget.CollectiveAndIndividual:
                    return "Cá nhân và tập thể";
            }
            return "";
        }
        public string ConvertLevel(int enumLevel) {
            switch (enumLevel)
            {
                case (int)EmulationLevel.Government:
                    return "Cấp nhà nước";
                case (int)EmulationLevel.Town:
                    return "Cấp xã";
                case (int)EmulationLevel.District:
                    return "Cấp huyện";
                case (int)EmulationLevel.Conscious:
                    return "Cấp tỉnh";
            }
            return "";
        }

        public string ConvertType(int enumType)
        {
            switch (enumType)
            {
                case (int)EmulationType.Frequent:
                    return "Thường xuyên";
                case (int)EmulationType.Batched:
                    return "Theo đợt";
            }
            return "";
        }

        public string ConvertStatus(int eumnStatus)
        {
            switch (eumnStatus)
            {
                case (int)EmulationStatus.Use:
                    return "Sử dụng";
                case (int)EmulationStatus.NotUse:
                    return "Ngừng sử dụng";
            }
            return "";
        }

    }
}
