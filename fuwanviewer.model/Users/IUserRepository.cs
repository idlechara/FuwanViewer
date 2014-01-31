using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FuwanViewer.Model.Infrastructure;

namespace FuwanViewer.Model.Users
{
    public interface IUserRepository : IRepository<string, User>
    {
    }
}
