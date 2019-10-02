using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace bpm.Models
{
    public class Month
    {
        [NotMapped]
        public int Yr { get; set; }

        [NotMapped]
        public int Mnth { get; set; }

        [NotMapped]
        public double SysAvg { get; set; }

        [NotMapped]
        public double DiaAvg { get; set; }
    }
}
