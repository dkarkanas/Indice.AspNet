﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Indice.Types;

namespace Indice.Services
{
    /// <summary>
    /// A lockmanager that does nothing. Nada. Nil
    /// </summary>
    public class NoOpLockManager : ILockManager
    {
        /// <inheritdoc/>
        public Task<ILockLease> AcquireLock(string name, TimeSpan? duration = null) => 
            Task.FromResult((ILockLease)new LockLease(new Base64Id(Guid.NewGuid()).ToString(), name, this));

        /// <inheritdoc/>
        public Task Cleanup() => Task.CompletedTask;

        /// <inheritdoc/>
        public Task ReleaseLock(ILockLease @lock) => Task.CompletedTask;

        /// <inheritdoc/>
        public Task<ILockLease> Renew(string name, string leaseId) =>
            Task.FromResult((ILockLease)new LockLease(leaseId, name, this));
    }
}
