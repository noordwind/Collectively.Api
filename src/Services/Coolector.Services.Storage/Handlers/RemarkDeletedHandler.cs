using System.Threading.Tasks;
using Coolector.Common.Events;
using Coolector.Common.Events.Remarks;
using Coolector.Services.Storage.Files;
using Coolector.Services.Storage.Repositories;

namespace Coolector.Services.Storage.Handlers
{
    public class RemarkDeletedHandler : IEventHandler<RemarkDeleted>
    {
        private readonly IRemarkRepository _repository;
        private readonly IFileHandler _fileHandler;

        public RemarkDeletedHandler(IRemarkRepository repository, IFileHandler fileHandler)
        {
            _repository = repository;
            _fileHandler = fileHandler;
        }

        public async Task HandleAsync(RemarkDeleted @event)
        {
            var remark = await _repository.GetByIdAsync(@event.Id);
            if (remark.HasNoValue)
                return;

            await _fileHandler.DeleteAsync(remark.Value.Photo.FileId);
            await _repository.DeleteAsync(remark.Value);
        }
    }
}