﻿using Indice.Hosting.Tasks.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Indice.Hosting.Tasks.Data.Configuration
{
    /// <summary>
    /// EF Core confirugation for <see cref="DbScheduledTask"/> entity.
    /// </summary>
    public sealed class DbScheduledTaskMap : IEntityTypeConfiguration<DbScheduledTask>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<DbScheduledTask> builder) {
            // Configure table name.
            builder.ToTable("ScheduledTask", "work");
            // Configure primary key.
            builder.HasKey(x => x.Id);
            // Configure indexes.
            builder.HasIndex(x => x.WorkerId);
            builder.HasIndex(x => x.Type);
        }
    }
}
