using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BootWrapper.BW.Interfaces
{
    public interface ILoginUser
    {
        string GetLogin();

        string GetName();

        bool IsAuthenticated();
    }

    /// <summary>
    /// Default Log provider when it is not created by Web project.
    /// </summary>
    public class DefaultLoginUser : ILoginUser
    {
        public bool IsAuthenticated()
        {
            return true;
        }

        public string GetLogin()
        {
            return "DefaultLoginUser";
        }

        public string GetName()
        {
            return "DefaultLoginUser";
        }       
    }
}
