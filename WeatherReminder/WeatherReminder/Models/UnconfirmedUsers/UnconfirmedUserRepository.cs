using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
using System.Text;
using WeatherReminder.Models.DataStorageModel;


namespace WeatherReminder.Models.UnconfirmedUsers
{
    public class UnconfirmedUserRepository : IUnconfirmedUserRepository
    {
        private readonly UnconfirmedUserDbContext _unconfirmedUserDbContext;

        public UnconfirmedUserRepository(UnconfirmedUserDbContext unconfirmedUserDbContext)
        {
            _unconfirmedUserDbContext = unconfirmedUserDbContext;
        }

        public List<UnconfirmedUser> GetUsers()
        {
            if (_unconfirmedUserDbContext.UnconfirmedUsers.Local.Count > 0)
            {

                return _unconfirmedUserDbContext.UnconfirmedUsers.ToList();

            }
            return new List<UnconfirmedUser>();
        }

        public UnconfirmedUser GetUser(string email)
        {
            return _unconfirmedUserDbContext.UnconfirmedUsers.Where(u => u.Email == email).ToList()[0];
        }

        public void AddUser(UnconfirmedUser user)
        {
            _unconfirmedUserDbContext.UnconfirmedUsers.Add(user);
            _unconfirmedUserDbContext.SaveChanges();
        }

        public void RemoveUser(UnconfirmedUser user)
        {
            _unconfirmedUserDbContext.UnconfirmedUsers.Remove(user);
            _unconfirmedUserDbContext.SaveChanges();
        }

        public void UpdateUser(UnconfirmedUser user)
        {
            _unconfirmedUserDbContext.UnconfirmedUsers.Update(user);
            _unconfirmedUserDbContext.SaveChanges();
        }

    }
}
