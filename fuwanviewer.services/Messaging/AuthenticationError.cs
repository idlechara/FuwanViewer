using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Services.Messaging
{
    public enum AuthenticationError
    {
        None,
        UnableToConnect,
        WrongCredentials,
        UnknownError,
    }
}
