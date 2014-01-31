using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Services.Messaging
{
    public struct AuthenticationRequest
    {
        public string username;
        public string password;

        public AuthenticationRequest(string username, string password)
        {
            this.username = username;
            this.password = password;
        }
    }
}
