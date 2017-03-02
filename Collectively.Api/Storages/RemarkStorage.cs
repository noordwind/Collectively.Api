using System;
using System.Threading.Tasks;
using Collectively.Api.Queries;
using Collectively.Common.Types;
using Collectively.Services.Storage.Models.Remarks;


namespace Collectively.Api.Storages
{
    public class RemarkStorage : IRemarkStorage
    {
        private readonly IStorageClient _storageClient;

        public RemarkStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id)
            => await _storageClient.GetAsync<Remark>($"remarks/{id}");

        public async Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query)
            => await _storageClient.GetFilteredCollectionAsync<Remark, BrowseRemarks>(query, "remarks");

        public async Task<Maybe<PagedResult<RemarkCategory>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _storageClient.GetFilteredCollectionAsync<RemarkCategory, BrowseRemarkCategories>
                (query, "remarks/categories");

        public async Task<Maybe<PagedResult<Tag>>> BrowseTagsAsync(BrowseRemarkTags query)
            => await _storageClient.GetFilteredCollectionAsync<Tag, BrowseRemarkTags>
                (query, "remarks/tags");
    }
}