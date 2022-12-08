using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesFileWork.Data
{
    public class Faculty
    {
        public string Department { get; set; }
        public string Part { get; set; }

        public Faculty() { }

        public Faculty(string department, string part)
        {
            Department = department;
            Part = part;
        }
    }
}
