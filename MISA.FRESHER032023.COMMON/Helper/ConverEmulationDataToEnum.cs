using MISA.FRESHER032023.COMMON.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Helper
{
    public class ConverEmulationDataToEnum
    {
        public int ConverTarget(string target)
        {
            switch (target.ToLower())
            {
                case "cá nhân":
                    return (int)EmulationTarget.Individual;
                case "tập thể":
                    return (int)EmulationTarget.Collective;
                case "cá nhân và tập thể":
                    return (int)EmulationTarget.CollectiveAndIndividual;
            }
            return -1;
        }


        public int ConverLevel(string level)
        {
            switch (level.ToLower())
            {
                case "cấp nhà nước":
                    return (int)EmulationLevel.Government;
                case "cấp tỉnh":
                    return (int)EmulationLevel.Conscious;
                case "cấp huyện":
                    return (int)EmulationLevel.District;
                case "cấp xã":
                    return (int)EmulationLevel.Town;
            }
            return -1;
        }


        public int ConverType(string type)
        {
            switch (type.ToLower())
            {
                case "thường xuyên":
                    return (int)EmulationType.Frequent;
                case "theo đợt":
                    return (int)EmulationType.Batched;
               
            }
            return -1;
        }

        public int ConverStatus(string status)
        {
            switch (status.ToLower())
            {
                case "sử dụng":
                    return (int)EmulationStatus.Use;
                case "ngừng sử dụng":
                    return (int)EmulationStatus.NotUse;

            }
            return -1;
        }
    }
}
