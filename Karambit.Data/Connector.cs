using System;

namespace Karambit.Data
{
    public abstract class Connector
    {
        #region Fields

        #endregion

        #region Methods
        public abstract void Connect(string host, string username, string password);

        public abstract void Connect(string host, string username, string password, string db);

        public abstract void Connect(string host, string username, string password, uint port);

        public abstract void Connect(string host, string username, string password, string db, uint port);

        public abstract void Disconnect();
        #endregion
    }
}
