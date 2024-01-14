using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherReminder.Models.UnconfirmedUsers
{
    public interface IUnconfirmedUserRepository
    {
        public void AddUser(UnconfirmedUser user);
        public void RemoveUser(UnconfirmedUser user);
        public List<UnconfirmedUser> GetUsers();
        public UnconfirmedUser GetUser(string email);
        public void UpdateUser(UnconfirmedUser user);
    }
}
