using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebApp.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }
        public string StudentCreated { get; set; }
        public string StudentModified { get; set; }
        public bool StudentChatty { get; set; }
    }
}
