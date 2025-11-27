using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDistributedSemaphore
{
    public interface IDistributedSemaphore
    {
        Task<bool> TryAcquireAsync();
        Task AcquireAsync(TimeSpan? timeout = null);
        Task ReleaseAsync();
        int CurrentCount { get; }
        string Name { get; }
    }
}
