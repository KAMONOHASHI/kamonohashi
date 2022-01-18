using System;
using System.Net;

namespace Nssol.Platypus.Services
{
    public class ServiceHttpProxy : IWebProxy
    {
        private Uri proxy;
        public ServiceHttpProxy(string proxy)
        {
            this.proxy = new Uri(proxy);
        }

        public ICredentials Credentials { get; set; }

        public Uri GetProxy(Uri destination)
        {
            return proxy;
        }

        public bool IsBypassed(Uri host)
        {
            return false;
        }
    }
}
