using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomDistributedSemaphore
{
    public interface IDistributedSemaphoreFactory
    {
        IDistributedSemaphore CreateSemaphore(string name, int maxCount);
    }
}
