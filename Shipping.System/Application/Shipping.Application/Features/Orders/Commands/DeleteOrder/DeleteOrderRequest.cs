using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public Guid Id { get; set; }

}