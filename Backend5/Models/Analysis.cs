using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Analysis
    {
        public Int32 AnalysisId { get; set; }

        public Int32 LabId { get; set; }

        public Lab Lab { get; set; }

        [Required]
        [MaxLength(200)]
        public String Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public String Status { get; set; }


    }
}
