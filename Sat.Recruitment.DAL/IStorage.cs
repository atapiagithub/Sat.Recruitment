using Sat.Recruitment.BE;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Sat.Recruitment.DAL
{
    public interface IStorage
    {
        Task SaveUser(User user);
        Task<User> GetUserByEmail(User user);
    }
}
