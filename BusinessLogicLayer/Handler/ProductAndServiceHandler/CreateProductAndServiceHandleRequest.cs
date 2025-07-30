using BusinessLogicLayer.Handler.ProductAndServiceHandler;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class CreateProductAndServiceHandleRequest : IRequest<CreateProductAndServiceHandleResponse>
{
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public UnitType UnitType { get; set; }  // ❗ eklendi
}
