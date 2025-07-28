using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

namespace BusinessLogicLayer.DesignPatterns.GenericRepositories.ConcRepositories
{
    public class UserRepository : BaseRepositoriesository<User>, IUserRepository
    {
        public UserRepository(MyContext db) : base(db) { }
    }
}
