using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FuwanViewer.Model.Users
{
    /// <summary>
    /// Class representing a user who reads VisualNovels
    /// </summary>
    public class User
    {
        public string Username { get; set; }
        public List<int> Playing { get; set; }
        public List<int> Finished { get; set; }
        public List<int> Dropped { get; set; }

        public User(string username, List<int> playing, List<int> finished, List<int> dropped)
        {
            this.Username = username;
            this.Playing = playing ?? new List<int>();
            this.Finished = finished ?? new List<int>();
            this.Dropped = dropped ?? new List<int>();
        }
    }

}
