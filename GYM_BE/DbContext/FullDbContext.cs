﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.Dynamic;
using API;
using GYM_BE.DTO;
using GYM_BE.ENTITIES;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Options;

namespace GYM_BE.Entities
{
    public partial class FullDbContext : DbContext
    {
        public AppSettings _appSettings;

        public FullDbContext()
        {
        }

        public FullDbContext(DbContextOptions<FullDbContext> options, IOptions<AppSettings> appSettings)
            : base(options)
        {
            _appSettings = appSettings.Value;
        }
        public virtual DbSet<GOODS_EQUIPMENT> GoodsEquipments { get; set; }
        public virtual DbSet<GOODS_EQUIPMENT_FIX> GoodsEquipmentFixs { get; set; }
        public virtual DbSet<GOODS_PACKAGE> GymPackages { get; set; }
        public virtual DbSet<GOODS_SHIFT> GoodsShifts { get; set; }
        public virtual DbSet<GOODS_SHIFT> GoodsLists { get; set; }
        public virtual DbSet<GOODS_LOCKER> GoodsLockers { get; set; }
        public virtual DbSet<SYS_OTHER_LIST_TYPE> SysOtherListTypes { get; set; }
        public virtual DbSet<SYS_OTHER_LIST> SysOtherLists { get; set; }
        public virtual DbSet<SYS_MENU> SysMenus { get; set; }
        public virtual DbSet<SYS_USER> SysUsers { get; set; }
        public virtual DbSet<PER_CUSTOMER> PerCustomers { get; set; }
        public virtual DbSet<PER_EMPLOYEE> PerEmployees { get; set; }
        public virtual DbSet<PER_CUS_TRANSACTION> PerCusTransactions { get; set; }
        public virtual DbSet<PER_CUS_LIST_CARD> PerCusListCards { get; set; }
        public virtual DbSet<CARD_CHECK_IN> CardCheckIns { get; set; }
        public virtual DbSet<CARD_INFO> CardInfos { get; set; }
        public virtual DbSet<CARD_ISSUANCE> CardIssuances { get; set; }
        public virtual DbSet<CARD_HISTORY> CardHistorys { get; set; }
        protected override void ConfigureConventions(
        ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>()
                .HavePrecision(18, 9);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_appSettings.ConnectionStrings.CoreDb);
            optionsBuilder.EnableSensitiveDataLogging();
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}