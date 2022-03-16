﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VKI_Telegram_bot.DB
{
    internal class VKITGBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<PDFPEntity> Timetables { get; set; }
        public DbSet<PDFPEntity> Sgroups { get; set; }
        public DbSet<PDFPEntity> Iertifications { get; set; }
        

        public VKITGBContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=DataBase.db");
        }
    }
}