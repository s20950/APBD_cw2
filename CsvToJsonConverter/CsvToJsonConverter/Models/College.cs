using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsvToJsonConverter.Models
{
    public class College
    {
       public string Name { get; set; }
       public HashSet<Student> Students { get; set; }

       public College(string name, HashSet<Student> students)
       {
           Students = students;
           Name = name;
       }
    }
}
