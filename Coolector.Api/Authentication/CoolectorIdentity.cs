using System.Security.Claims;
using System.Security.Principal;

namespace Coolector.Api.Authentication
{
    public class CoolectorIdentity : ClaimsPrincipal
    {
        public CoolectorIdentity(string name) : base(new GenericIdentity(name))
        {
        }
    }
}