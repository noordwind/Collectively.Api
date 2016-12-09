using System;
using System.Threading.Tasks;
using Coolector.Api.Queries;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Shared.Dto;

namespace Coolector.Api.Storages
{
    public class RemarkStorage : IRemarkStorage
    {
        private readonly IStorageClient _storageClient;

        public RemarkStorage(IStorageClient storageClient)
        {
            _storageClient = storageClient;
        }

        public async Task<Maybe<RemarkDto>> GetAsync(Guid id)
            => await _storageClient.GetAsync<RemarkDto>($"remarks/{id}");

        public async Task<Maybe<PagedResult<RemarkDto>>> BrowseAsync(BrowseRemarks query)
            => await _storageClient.GetFilteredCollectionAsync<RemarkDto, BrowseRemarks>(query, "remarks");

        public async Task<Maybe<PagedResult<RemarkCategoryDto>>> BrowseCategoriesAsync(BrowseRemarkCategories query)
            => await _storageClient.GetFilteredCollectionAsync<RemarkCategoryDto, BrowseRemarkCategories>
                (query, "remarks/categories");
    }
}