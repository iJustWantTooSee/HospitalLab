﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models.ViewModels
{
    public class DiagnosisModel
    {
        [Required]
        [MaxLength(200)]
        public String Type { get; set; }
        
        [Required]
        [MaxLength(200)]
        public String Complications { get; set; }

        public String Details { get; set; }
    }
}
