﻿using Indice.Hosting.Tasks.Implementations;
using Microsoft.EntityFrameworkCore;

namespace Indice.Hosting.Tasks.Data
{
    /// <summary>
    /// Contains the required tables to implement a locking mechanism using a relational database.
    /// </summary>
    /// <remarks>Only use with caution in <see cref="RelationalLockManager"/>.</remarks>
    public class LockDbContext : TaskDbContext
    {
        /// <summary>
        /// Constructs a new <see cref="LockDbContext"/>.
        /// </summary>
        /// <param name="options"></param>
        public LockDbContext(DbContextOptions<LockDbContext> options) : base(options) { }
    }
}
