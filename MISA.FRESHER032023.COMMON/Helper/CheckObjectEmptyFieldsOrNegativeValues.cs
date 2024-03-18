using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MISA.FRESHER032023.COMMON.Helper
{
    public class CheckObjectEmptyFieldsOrNegativeValues
    {
        public bool CheckData(object obj)
        {
            Type type = obj.GetType();
            PropertyInfo[] properties = type.GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object value = property.GetValue(obj);

                if (value == null || string.IsNullOrEmpty(value.ToString()) || value.Equals(-1))
                {
                    return true; // Có trường rỗng hoặc có giá trị -1
                }
            }

            return false; // Không có trường rỗng hoặc giá trị -1
        }
    }
}
