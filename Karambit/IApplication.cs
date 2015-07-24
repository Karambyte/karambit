using Karambit.Logging;
using System;

namespace Karambit
{
    public interface IApplication
    {
        Deployment Deployment { get; set; }
        Logger Logger { get; }
        string Name { get; set; }

        void Start();
        void Stop();
    }
}
