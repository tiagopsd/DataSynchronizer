using DataSynchronizer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataSynchronizer.Infra.Mapping
{
    public class SyncHistoricMapping : IEntityTypeConfiguration<SyncHistoric>
    {
        public void Configure(EntityTypeBuilder<SyncHistoric> builder)
        {
            builder.ToTable("SyncHistoric", "dbo");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.DateTimeSync)
                .HasColumnName("DateTimeSync")
                .HasColumnType("DateTime")
                .IsRequired();

            builder.Property(d => d.SyncGuid)
                .HasColumnName("SyncGuid")
                .HasColumnType("Uniqueidentifier")
                .IsRequired();

            builder.Property(d => d.ObjectGuid)
                .HasColumnName("ObjectGuid")
                .HasColumnType("Uniqueidentifier")
                .IsRequired();

            builder.Property(d => d.TypeSync)
                .HasColumnName("TypeSync")
                .HasColumnType("smallint")
                .IsRequired();
        }
    }
}
