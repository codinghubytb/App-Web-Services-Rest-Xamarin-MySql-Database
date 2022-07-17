using System;
using System.Collections.Generic;
using System.Text;

namespace AppApi.Models
{
    public class Classroom
    {
        public int ClassroomId { get; set; }
        public string ClassroomName { get; set; }
        public DateTime ClassroomCreated { get; set; }
        public DateTime ClassroomModified { get; set; }
        public int ClassroomNbPerson { get; set; }
        public int nb_enreg { get; set; }
        public double nb_pages { get; set; }
    }
}
