using System;

namespace Coolector.Services.Domain
{
    public interface IIdentifiable
    {
        Guid Id { get; }
    }
}