using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootWrapper.BW.Interfaces
{
    public interface ILogProvider
    {
        void Log(string msg);

        void Log(Exception ex);

        void Log(string msg, Exception ex);
    }

    /// <summary>
    /// Default Log provider when it is not declared.
    /// </summary>
    public class DefaultLogProvider : ILogProvider
    {
        public void Log(string msg)
        { }

        public void Log(Exception ex)
        { }

        public void Log(string msg, Exception ex)
        { }
    }
}
