using BusinessLogicLayer.DesignPatterns.GenericRepositories.BaseRepositories;
using BusinessLogicLayer.DesignPatterns.GenericRepositories.InterfaceRepositories;
using DatabaseAccessLayer.Contexts;
using DatabaseAccessLayer.Entities;

public class LineOfInvoiceRepository : BaseRepositoriesository<LineOfInvoice>, ILineOfInvoiceRepository
{
    public LineOfInvoiceRepository(MyContext db) : base(db) { }
}
