using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft;
using Newtonsoft.Json;

namespace FuwanViewer.Fuwanovel
{
    /// <summary>
    /// Provides static methods for interacting with Fuwanovel API.
    /// </summary>
    public static class FuwanovelAPI
    {
        public static string Username { get; set; }
        public static string Password { get; set; }

        /// <summary>
        /// Returns list with IDs of all novels.
        /// </summary>
        public static List<int> GetAllIds()
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://fuwanovel.org/api/v1/novels");
            request.Credentials = new NetworkCredential(Username, Password);
            HttpWebResponse response;
            try
            {
                using (response = request.GetResponse() as HttpWebResponse)
                {
                    StreamReader reader = new StreamReader(response.GetResponseStream());
                    List<FuwaVisualNovelShort> infoList = JsonConvert.DeserializeObject<List<FuwaVisualNovelShort>>(reader.ReadToEnd());
                    response.Close();

                    return infoList.ConvertAll(info => info.id).ToList();
                }
            }
            catch (WebException)
            {
                return null;
            }
        }
        
        /// <summary>
        /// Returns list with all Visual Novels.
        /// </summary>
        /// <returns>List with Novels or null if an error occured.</returns>
        public static List<FuwaVisualNovel> GetAllNovels()
        {
            HttpWebRequest request = WebRequest.CreateHttp(@"http://fuwanovel.org/api/v1/novels?full=1");
            request.Credentials = new NetworkCredential(Username, Password);
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    var result = new List<FuwaVisualNovel>();
                    result = JsonConvert.DeserializeObject<List<FuwaVisualNovel>>(reader.ReadToEnd());
                    return result;
                }
            }
            catch (WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// Returns a Visual Novel with specified Id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Novel with specified Id or null if an error occured</returns>
        public static FuwaVisualNovel GetNovel(int id)
        {
            HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://fuwanovel.org/api/v1/novels/{0}", id));
            request.Credentials = new NetworkCredential(Username, Password);
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    FuwaVisualNovel result = JsonConvert.DeserializeObject<FuwaVisualNovel>(reader.ReadToEnd());

                    return result;
                }
            }
            catch (WebException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets a DateTime of last modification to API's data.
        /// </summary>
        /// <returns>DateTime of last modification or default(DateTime) if an error occured.</returns>
        public static DateTime GetLastModified()
        {
            HttpWebRequest request = WebRequest.CreateHttp("http://fuwanovel.org/api/v1/novels?full=1");
            request.Method = "HEAD";
            request.Timeout = 7000;
            request.Credentials = new NetworkCredential(Username, Password);
            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return response.LastModified;
                }
            }
            catch (WebException)
            {
                return default(DateTime);
            }
        }

        /// <summary>
        /// Gets a DateTime of last modification to Novel with specified Id.
        /// </summary>
        /// <returns>DateTime of last modification or default(DateTime) if an error occured.</returns>
        public static DateTime GetLastModifiedFor(int id)
        {
            HttpWebRequest request = WebRequest.CreateHttp(String.Format("http://fuwanovel.org/api/v1/novels/{0}", id));
            request.Method = "HEAD";
            request.Timeout = 7000;
            request.Credentials = new NetworkCredential(Username, Password);

            try
            {
                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {
                    return response.LastModified;
                }
            }
            catch (WebException)
            {
                return default(DateTime);
            }
        }
    }
}
