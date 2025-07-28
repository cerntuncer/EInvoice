using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class ProductAndServiceRepository : BaseRepositoriesository<ProductAndService>, IProductAndServiceRepository
{
    public ProductAndServiceRepository(MyContext db) : base(db) { }
}
