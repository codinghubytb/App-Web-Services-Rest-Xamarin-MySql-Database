using System;
using System.Collections.Generic;
using System.Text;

namespace AppApi.Models
{
    public class Student
    {
        public int StudentId { get; set; }
        public int ClassroomId { get; set; }
        public string StudentName { get; set; }
        public int StudentAge { get; set; }
        public DateTime StudentCreated { get; set; }
        public DateTime StudentModified { get; set; }
        public bool StudentChatty { get; set; }
    }
}
