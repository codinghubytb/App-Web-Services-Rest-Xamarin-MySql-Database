using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APIWebApp.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string ClassroomName { get; set; }
        public int ClassroomNbPerson { get; set; }
        public string ClassroomCreated { get; set; }
        public string ClassroomModified { get; set; }
    }
}
