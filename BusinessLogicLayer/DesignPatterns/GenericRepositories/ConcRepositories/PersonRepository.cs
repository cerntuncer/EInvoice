using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class PersonRepository : BaseRepositoriesository<Person>, IPersonRepository
{
    public PersonRepository(MyContext db) : base(db) { }
}
