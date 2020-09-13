using DataSynchronizer.Domain.Interfaces;
using DataSynchronizer.Infra.Mapping;
using DataSynchronizer.Infra.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Infra
{
    public class Context : DbContext
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        public Context(DbContextOptions<Context> options,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
            : base(options)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            dbContextOptionsBuilder.UseSqlServer(_configuration.GetConnectionString("Default").Decrypt());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SyncHistoricMapping());
        }
    }
}
