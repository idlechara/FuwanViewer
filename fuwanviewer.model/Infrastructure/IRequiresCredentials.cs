using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FuwanViewer.Model.Infrastructure
{
    /// <summary>
    /// Exposes Username and Password properties.
    /// </summary>
    public interface IRequiresCredentials
    {
        string Username { get; set; }
        string Password { get; set; }

        bool Authenticate();
    }
}
