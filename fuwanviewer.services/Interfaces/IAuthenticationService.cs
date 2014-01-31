using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Services.Messaging;

namespace FuwanViewer.Services.Interfaces
{
    public interface IAuthenticationService
    {
        AuthenticationResponse Authenticate(AuthenticationRequest request);
    }
}
