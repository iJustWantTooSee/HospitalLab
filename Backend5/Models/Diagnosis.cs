﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Backend5.Models
{
    public class Diagnosis
    {
        public Int32 DiagnosisId { get; set; }

        [Required]
        [MaxLength(200)]
        public String Type { get; set; }

        [Required]
        [MaxLength(200)]
        public String Complications { get; set; }

        public String Details { get; set; }

        public Int32 PatientId { get; set; }

        public Patient Patient { get; set; }

    }
}
