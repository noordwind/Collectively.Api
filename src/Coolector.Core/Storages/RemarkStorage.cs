using System;
using System.IO;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Core.Filters;
using Coolector.Dto.Remarks;

namespace Coolector.Core.Storages
{
    public class RemarkStorage : IRemarkStorage
    {
        private readonly IStorageClient _storageClient;

        public RemarkStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<RemarkDto>> GetAsync(Guid id)
            => await _storageClient.GetUsingCacheAsync<RemarkDto>($"remarks/{id}");

        public async Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query)
            => await _storageClient.GetFilteredCollectionc<RemarkDto, BrowseRemarks>(query, "remarks");

        public async Task<Maybe<Stream>> GetPhotoStreamAsync(Guid id)
            => await _storageClient.GetStreamAsync($"remarks/{id}/photo");
    }
}