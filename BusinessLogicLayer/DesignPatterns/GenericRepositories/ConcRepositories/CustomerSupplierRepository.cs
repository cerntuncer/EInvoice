using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class CustomerSupplierRepository : BaseRepositoriesository<CustomerSupplier>, ICustomerSupplierRepository
{
    public CustomerSupplierRepository(MyContext db) : base(db) { }
}
