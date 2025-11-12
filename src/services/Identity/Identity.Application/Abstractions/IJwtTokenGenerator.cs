using Identity.Domain;
using System.Collections.Generic;

namespace Identity.Application.Abstractions;

public interface IJwtTokenGenerator
{
    string CreateToken(AppUser user, IEnumerable<string> roles);
}