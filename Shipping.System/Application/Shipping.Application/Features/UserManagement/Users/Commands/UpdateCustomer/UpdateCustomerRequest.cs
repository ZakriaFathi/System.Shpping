using FluentResults;
using MediatR;

namespace Shipping.Application.Features.Auth.Commands.UpdateCustomer;

public class UpdateCustomerRequest : IRequest<Result<string>>
{
    public string UserId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
}