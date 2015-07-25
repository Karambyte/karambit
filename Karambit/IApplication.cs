using Karambit.Logging;
using Karambit.Net;
using System;

namespace Karambit
{
    public interface IApplication
    {
        Deployment Deployment { get; set; }
        string Name { get; set; }
        bool Running { get; }

        void Attach(IServer server);
        void Detach(IServer server);

        void Start();
        void Stop();
    }
}
