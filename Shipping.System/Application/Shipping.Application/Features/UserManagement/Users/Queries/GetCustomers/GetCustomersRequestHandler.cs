using FluentResults;
using MediatR;
using Shipping.Application.Abstracts;

namespace Shipping.Application.Features.UserManagement.Users.Queries.GetCustomers;

public class GetCustomersRequestHandler : IRequestHandler<GetCustomersRequest, Result<List<GetCustomersResponse>>>
{
    private readonly IUserManagmentRepository _userManagementRepository;

    public GetCustomersRequestHandler(IUserManagmentRepository userManagementRepository)
    {
        _userManagementRepository = userManagementRepository;
    }


    public async Task<Result<List<GetCustomersResponse>>> Handle(GetCustomersRequest request, CancellationToken cancellationToken)
        => await _userManagementRepository.GetCustomersAsync(request, cancellationToken);
}