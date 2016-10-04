using System;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Queries;
using Coolector.Services.Remarks.Repositories;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public class RemarkService : IRemarkService
    {
        private readonly IFileHandler _fileHandler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICategoryRepository _categoryRepository;

        public RemarkService(IFileHandler fileHandler, IRemarkRepository remarkRepository, 
            IUserRepository userRepository, ICategoryRepository categoryRepository)
        {
            _fileHandler = fileHandler;
            _remarkRepository = remarkRepository;
            _userRepository = userRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id)
            => await _remarkRepository.GetByIdAsync(id);

        public async Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query)
            => await _remarkRepository.BrowseAsync(query);

        public async Task CreateAsync(Guid id, string userId, Guid categoryId, File photo, 
            Position position, string description = null)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasNoValue)
                throw new ArgumentException($"User with id: {userId} has not been found.");

            //TODO: Use this code if the different categories will be needed.
            //var category = await _categoryRepository.GetByIdAsync(categoryId);
            //if (category.HasNoValue)
            //    throw new ArgumentException($"Category with id: {userId} has not been found.");

            var category = await _categoryRepository.GetDefaultAsync();
            if (category.HasNoValue)
                throw new ArgumentException("Default category has not been found.");

            var location = Location.Create(position);
            var remarkPhoto = RemarkPhoto.Empty;
            await _fileHandler.UploadAsync(photo, fileId =>
            {
                remarkPhoto = RemarkPhoto.Create(fileId, photo.Name, photo.ContentType);
            });
            var remark = new Remark(id, user.Value, category.Value, location, remarkPhoto, description);
            await _remarkRepository.AddAsync(remark);
        }
    }
}