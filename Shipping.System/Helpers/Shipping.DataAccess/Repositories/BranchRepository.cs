using FluentResults;
using Microsoft.EntityFrameworkCore;
using Shipping.Application.Abstracts;
using Shipping.Application.Features.Branchs.Commands.CreateBranch;
using Shipping.Application.Features.Branchs.Commands.DeleteBranch;
using Shipping.Application.Features.Branchs.Commands.UpdateBranch;
using Shipping.Application.Features.Branchs.Queries;
using Shipping.Application.Features.Branchs.Queries.GetBranchById;
using Shipping.Application.Features.Branchs.Queries.GetBranchs;
using Shipping.DataAccess.Persistence.DataBase;
using Shipping.Domain.Entities;

namespace Shipping.DataAccess.Repositories;

public class BranchRepository : IBranchRepository
{
    private readonly ShippingDbContext _shippingDb;

    public BranchRepository(ShippingDbContext shippingDb)
    {
        _shippingDb = shippingDb;
    }

    public async Task<Result<string>> CreateBranchAsync(CreateBranchRequest request, CancellationToken cancellationToken)
    {
        var branch = await _shippingDb.Branchs.FirstOrDefaultAsync(x => x.Name == request.Name , cancellationToken);
        if (branch != null)
            return Result.Fail("الفرع موجودة مسبقا");
        
        var newBranch = new Branch()
        {
            Name = request.Name,
            IsMajor = request.IsMajor
        };
        
        await _shippingDb.Branchs.AddAsync(newBranch, cancellationToken);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return newBranch.Id.ToString();
    }

    public async Task<Result<string>> UpdateBranchAsync(UpdateBranchRequest request, CancellationToken cancellationToken)
    {
        var branch = await _shippingDb.Branchs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (branch == null)
            return Result.Fail("الفرع غير موجودة");   
        
        branch.Name = request.Name;
        branch.IsMajor = request.IsMajor;
        
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return Result.Ok("تمت عملية التعديل بنجاح");
    }

    public async Task<Result<List<BranchsResopnse>>> GetBranchsAsync(GetBranchsRequest request, CancellationToken cancellationToken)
    {
        var branchs = await _shippingDb.Branchs
            .Select(x => new BranchsResopnse
            {
                BranchId = x.Id,
                BranchName = x.Name,
                IsMajor = x.IsMajor
            }).ToListAsync(cancellationToken);
        
        if (branchs.Count <= 0)
            return Result.Fail<List<BranchsResopnse>>( "لا يوجد فروع" );
        
        return branchs;
    }

    public async Task<Result<BranchsResopnse>> GetBranchByIdAsync(GetBranchByIdRequest request, CancellationToken cancellationToken)
    {
        var branch = await _shippingDb.Branchs
            .Where(x=>x.Id == request.BranchId)
            .Select(x => new BranchsResopnse
            {
                BranchId = x.Id,
                BranchName = x.Name,
                IsMajor = x.IsMajor
            }).FirstOrDefaultAsync(cancellationToken);
        
        if (branch == null)
            return Result.Fail<BranchsResopnse>( "الفرع غير موجود" );
        
        return branch;
    }

    public async Task<Result<string>> DeleteBranch(DeleteBranchRequest request, CancellationToken cancellationToken)
    {
        var branch = await _shippingDb.Branchs.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if(branch == null)
            return Result.Fail("الفرع غير موجود");
        
        _shippingDb.Branchs.Remove(branch);
        await _shippingDb.SaveChangesAsync(cancellationToken);
        
        return "تم الحذف";    
    }
}