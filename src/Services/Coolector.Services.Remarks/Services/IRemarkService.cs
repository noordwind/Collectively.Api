using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Services.Remarks.Domain;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public interface IRemarkService
    {
        Task CreateAsync(string userId, Guid categoryId, File photo, Position position, string description = null);
    }
}