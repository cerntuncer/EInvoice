using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class InvoiceRepository : BaseRepositoriesository<Invoice>, IInvoiceRepository
{
    public InvoiceRepository(MyContext db) : base(db) { }
}
