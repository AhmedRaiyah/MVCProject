﻿using GymManagementDAL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.Contexts
{
    public class GymDBContext : DbContext
    {
        public GymDBContext(DbContextOptions<GymDBContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        #region DBSets
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Member> Members { get; set; }   
        public DbSet<Session> Sessions{ get; set; }   
        public DbSet<Category> Categories { get; set; }   
        public DbSet<Booking> Bookings{ get; set; }   
        public DbSet<Membership> Memberships { get; set; }   
        public DbSet<HealthRecord> HealthRecords { get; set; }   
        public DbSet<Plan> Plans { get; set; }   
        #endregion
    }
}
