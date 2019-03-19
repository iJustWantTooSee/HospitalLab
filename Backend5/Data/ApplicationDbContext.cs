﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend5.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend5.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<Hospital> Hospitals { get; set; }

        public DbSet<HospitalPhone> HospitalPhones { get; set; }

        public DbSet<Ward> Wards { get; set; }

        public DbSet<Lab> Labs { get; set; }

        public DbSet<LabPhone> LabPhones { get; set; }

        public DbSet<HospitalLab> HospitalLabs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<HospitalPhone>()
                .HasKey(x => new { x.HospitalId, x.PhoneId });

            modelBuilder.Entity<LabPhone>()
                .HasKey(x => new { x.LabId, x.PhoneId });

            modelBuilder.Entity<HospitalLab>()
                .HasKey(x => new { x.HospitalId, x.LabId });
        }
    }
}
