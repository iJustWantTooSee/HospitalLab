﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class AnalysisModel
    {
        [Required]
        [MaxLength(200)]
        public String Type { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public String Status { get; set; }
    }
}
