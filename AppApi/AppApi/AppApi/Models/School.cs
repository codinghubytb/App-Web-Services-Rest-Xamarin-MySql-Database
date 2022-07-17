using System;
using System.Collections.Generic;
using System.Text;

namespace AppApi.Models
{
    public class School
    {
        public int nb_enreg { get; set; }
        public double nb_pages { get; set; }

        public List<Classroom> ListClassroom { get; set; }
    }
}
