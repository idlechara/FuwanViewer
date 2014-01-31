using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Services.Messaging
{
    public struct AuthenticationResponse
    {
        public bool success;
        public AuthenticationError error;

        public AuthenticationResponse(bool success, AuthenticationError error)
        {
            this.success = success;
            this.error = error;
        }
    }
}
