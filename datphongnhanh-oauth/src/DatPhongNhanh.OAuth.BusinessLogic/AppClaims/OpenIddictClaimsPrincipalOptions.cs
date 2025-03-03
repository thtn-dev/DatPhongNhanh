using DatPhongNhanh.OAuth.SharedKernel.Collections;

namespace DatPhongNhanh.OAuth.Business.AppClaims;

public class OpenIddictClaimsPrincipalOptions
{
    public ITypeList<IOpenIddictClaimsPrincipalHandler> ClaimsPrincipalHandlers { get; } =
        new TypeList<IOpenIddictClaimsPrincipalHandler>();
}