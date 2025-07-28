using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class CaseRepository : BaseRepositoriesository<Case>, ICaseRepository
{
    public CaseRepository(MyContext db) : base(db) { }
}
