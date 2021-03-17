﻿using Core.Entities.Concrete;
using System.Collections.Generic;

namespace Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        TAccessToken CreateToken<TAccessToken>(User user, List<OperationClaim> operationClaims) where TAccessToken : IAccessToken, new();
    }
}
