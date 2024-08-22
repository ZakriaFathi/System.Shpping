using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Orders.Commands.ChangeOrderStateByRepresentative;

public class ChangeOrderStateByRepresentativeRequest : IRequest<Result<string>>
{
    public string OrderNo { get; set; }
}