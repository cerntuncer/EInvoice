using BusinessLogicLayer.Handler.ProductAndServiceHandler.DTOs;
using DatabaseAccessLayer.Enumerations;
using MediatR;

public class UpdateProductAndServiceHandleRequest : IRequest<UpdateProductAndServiceHandleResponse>
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal UnitPrice { get; set; }
    public UnitType UnitType { get; set; }
}
