using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Services.Interfaces;
using FuwanViewer.Services.Messaging;

namespace FuwanViewer.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        public AuthenticationResponse Authenticate(AuthenticationRequest request)
        {
            HttpWebRequest httpRequest = WebRequest.CreateHttp("http://fuwanovel.org/api/v1");
            httpRequest.Credentials = new NetworkCredential(request.username, request.password);
            httpRequest.Timeout = 10000;

            HttpWebResponse response;
            try
            {
                using (response = httpRequest.GetResponse() as HttpWebResponse)
                {
                    return new AuthenticationResponse(true, AuthenticationError.None);
                }
            }
            catch (WebException e)
            {
                if (e.Response == null)
                    return new AuthenticationResponse(false, AuthenticationError.UnableToConnect);

                response = e.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.Unauthorized)
                    return new AuthenticationResponse(false, AuthenticationError.WrongCredentials);
                else
                    return new AuthenticationResponse(false, AuthenticationError.UnknownError);
            }
        }
    }
}
