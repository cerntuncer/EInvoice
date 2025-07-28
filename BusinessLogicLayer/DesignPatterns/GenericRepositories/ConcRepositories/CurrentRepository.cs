using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class CurrentRepository : BaseRepositoriesository<Current>, ICurrentRepository
{
    public CurrentRepository(MyContext db) : base(db) { }
}
