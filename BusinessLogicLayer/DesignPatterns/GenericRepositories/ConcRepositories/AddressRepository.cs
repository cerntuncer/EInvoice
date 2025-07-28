using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class AddressRepository : BaseRepositoriesository<Address>, IAddressRepository
{
    public AddressRepository(MyContext db) : base(db) { }
}
