using System;

namespace BootWrapper.FW.Interfaces
{
    public interface ILoginContext
    {
        Guid[] Grupos { get; }        
        Guid[] Aeroportos { get; }
    }
}