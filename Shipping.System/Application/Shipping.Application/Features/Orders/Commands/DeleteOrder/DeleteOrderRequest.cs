using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.DeleteOrder;

public class DeleteOrderRequest : IRequest<Result<string>>
{
    public Guid Id { get; set; }

}