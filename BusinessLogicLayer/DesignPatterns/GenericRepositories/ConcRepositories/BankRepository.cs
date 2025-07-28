using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class BankRepository : BaseRepositoriesository<Bank>, IBankRepository
{
    public BankRepository(MyContext db) : base(db) { }
}
