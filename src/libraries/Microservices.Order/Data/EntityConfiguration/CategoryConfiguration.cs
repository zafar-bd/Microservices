﻿using Microservices.Order.Data.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Microservices.Order.Data.EntityConfiguration
{
    public class CategoryConfiguration : IEntityTypeConfiguration<ProductCategory>
    {
        public void Configure(EntityTypeBuilder<ProductCategory> builder)
        {
            builder.Property(p => p.Name).HasMaxLength(250);
            builder
                .HasIndex(l => new
                {
                    l.Name,
                })
                .IsUnique();

        }
    }
}
