using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Dto.Users;

namespace Coolector.Api.Authentication
{
    public interface IUserSessionProvider
    {
        Task<Maybe<UserSessionDto>> GetAsync(Guid id);
    }
}