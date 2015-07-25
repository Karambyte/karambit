using Karambit.Logging;
using System;

namespace Karambit
{
    public interface IApplication
    {
        Deployment Deployment { get; set; }
        string Name { get; set; }
        bool Running { get; }

        void Start();
        void Stop();
    }
}
