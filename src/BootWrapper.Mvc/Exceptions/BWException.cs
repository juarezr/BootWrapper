using BootWrapper.Mvc.Core;
using BootWrapper.Mvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootWrapper.Mvc.Exceptions
{
    public class BWException
    {
        private ILogProvider _provider;

        public BWException()
        {
            //create provider from web project
            InitProvider();
        }

        private void InitProvider()
        {
            _provider = BWConfigurationSection.GetConfig().LogProvider.GetProvider();
        }

        public void Log(string msg)
        {
            _provider.Log(msg);
        }

        public void Log(Exception ex)
        {
            _provider.Log(ex);
        }

        public void Log(string msg, Exception ex)
        {
            _provider.Log(msg, ex);
        }
    }
}
