using BusinessLogicLayer.Handler.CustomerSupplierHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateCustomerSupplierHandleRequest : IRequest<CreateCustomerSupplierHandleResponse>
{
    public CustomerOrSupplierType Type { get; set; }

    // Person bilgileri
    public string Name { get; set; }
    public long IdentityNumber { get; set; }
    public string TaxOffice { get; set; }
  
}
