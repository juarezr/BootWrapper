using BootWrapper.BW.Interfaces;
using BootWrapper.BW.Menu;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace BootWrapper.BW.Core
{
    public class BWConfigurationSection : ConfigurationSection
    {
        public static BWConfigurationSection GetConfig()
        {
            return (BootWrapper.BW.Core.BWConfigurationSection)System.Configuration.ConfigurationManager.GetSection("bwConfigGroup/bwConfig");
        }

        [ConfigurationProperty("debugMode", DefaultValue = "false", IsRequired = false)]
        public Boolean DebugMode
        {
            get
            {
                return (Boolean)this["debugMode"];
            }
            set
            {
                this["debugMode"] = value;
            }
        }

        [ConfigurationProperty("logProvider")]
        public LogProviderElement LogProvider
        {
            get
            {
                return (LogProviderElement)this["logProvider"];
            }
            set
            { this["logProvider"] = value; }
        }

        // Create a "color element."
        [ConfigurationProperty("roleProvider")]
        public RoleProviderElement RoleProvider
        {
            get
            {
                return (RoleProviderElement)this["roleProvider"];
            }
            set
            { this["roleProvider"] = value; }
        }
    }

    public abstract class InvokerElement : ConfigurationElement
    {
        [ConfigurationProperty("typeName", DefaultValue = "", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\GHIJKLMNOPQRSTUVWXYZ")]
        public String TypeName
        {
            get
            {
                return (String)this["typeName"];
            }
            set
            {
                this["typeName"] = value;
            }
        }

        [ConfigurationProperty("assemblyName", DefaultValue = "", IsRequired = true)]
        //[StringValidator(InvalidCharacters = "~!@#$%^&*()[]{}/;'\"|\\GHIJKLMNOPQRSTUVWXYZ")]
        public String AssemblyName
        {
            get
            {
                return (String)this["assemblyName"];
            }
            set
            {
                this["assemblyName"] = value;
            }
        }

        public virtual Object GetProxyInstance()
        {
            if (string.IsNullOrEmpty(AssemblyName) || String.IsNullOrEmpty(TypeName))
                return null;

            return Activator.CreateInstance(AssemblyName, TypeName).Unwrap();
        }
    }

    // Define the "logProvider" element 
    // with "Type" attributes.
    public class LogProviderElement : InvokerElement
    {
        public ILogProvider GetProvider()
        {
            ILogProvider logProvider = null;
            var provider = base.GetProxyInstance();
            if (provider != null)
                logProvider = provider as ILogProvider;
                        
            if (logProvider == null)
                logProvider = new DefaultLogProvider();

            return logProvider;
        }        
    }

    // Define the "logProvider" element 
    // with "Type" attributes.
    public class RoleProviderElement : InvokerElement
    {
        public IRoleProvider GetProvider()
        {
            IRoleProvider roleProvider = null;
            var provider = base.GetProxyInstance();
            if (provider != null)
                roleProvider = provider as IRoleProvider;                
            

            if (roleProvider == null)
                throw new Exception("IRoleProvider não detectado.");

            return roleProvider;
        }        
    }
}
