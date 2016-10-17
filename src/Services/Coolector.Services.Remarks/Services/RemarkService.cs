using System;
using System.Linq;
using System.Threading.Tasks;
using Coolector.Common.Types;
using Coolector.Services.Domain;
using Coolector.Services.Remarks.Domain;
using Coolector.Services.Remarks.Extensions;
using Coolector.Services.Remarks.Queries;
using Coolector.Services.Remarks.Repositories;
using File = Coolector.Services.Remarks.Domain.File;

namespace Coolector.Services.Remarks.Services
{
    public class RemarkService : IRemarkService
    {
        private const double AllowedDistance = 15.0;

        private readonly IFileHandler _fileHandler;
        private readonly IRemarkRepository _remarkRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserRepository _userRepository;

        public RemarkService(IFileHandler fileHandler, 
            IRemarkRepository remarkRepository, 
            IUserRepository userRepository,
            ICategoryRepository categoryRepository)
        {
            _fileHandler = fileHandler;
            _remarkRepository = remarkRepository;
            _categoryRepository = categoryRepository;
            _userRepository = userRepository;
        }

        public async Task<Maybe<Remark>> GetAsync(Guid id)
            => await _remarkRepository.GetByIdAsync(id);

        public async Task<Maybe<PagedResult<Remark>>> BrowseAsync(BrowseRemarks query)
            => await _remarkRepository.BrowseAsync(query);

        public async Task<Maybe<PagedResult<Category>>> BrowseCategoriesAsync(BrowseCategories query)
            => await _categoryRepository.BrowseAsync(query);

        public async Task<Maybe<FileStreamInfo>> GetPhotoAsync(Guid id)
            => await _fileHandler.GetFileStreamInfoAsync(id);

        public async Task CreateAsync(Guid id, string userId, Guid categoryId, File photo, 
            Location location, string description = null)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasNoValue)
                throw new ArgumentException($"User with id: {userId} has not been found.");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category.HasNoValue)
                throw new ArgumentException($"Category with id: {userId} has not been found.");

            var remarkPhoto = RemarkPhoto.Empty;
            var extension = photo.Name.Split('.').Last();
            var fileName = $"remark-{id:N}.{extension}";
            await _fileHandler.UploadAsync(photo, fileId =>
            {
                remarkPhoto = RemarkPhoto.Create(fileId, fileName, photo.Name, photo.ContentType);
            });
            var remark = new Remark(id, user.Value, category.Value, location, remarkPhoto, description);
            await _remarkRepository.AddAsync(remark);
        }

        public async Task ResolveAsync(Guid id, string userId, File photo, Location location)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasNoValue)
                throw new ArgumentException($"User with id: {userId} has not been found.");

            var remark = await _remarkRepository.GetByIdAsync(id);
            if (remark.HasNoValue)
                throw new ServiceException($"Remark with id: {id} does not exist!");

            if (remark.Value.Location.IsInRange(location, AllowedDistance) == false)
                throw new ServiceException($"The distance between user and remark: {id} is to high!");

            var remarkPhoto = RemarkPhoto.Empty;
            var extension = photo.Name.Split('.').Last();
            var fileName = $"remark-{id:N}-resolved.{extension}";
            await _fileHandler.UploadAsync(photo, fileId =>
            {
                remarkPhoto = RemarkPhoto.Create(fileId, fileName, photo.Name, photo.ContentType);
            });

            remark.Value.Resolve(user.Value, remarkPhoto);
            await _remarkRepository.UpdateAsync(remark.Value);
        }

        public async Task DeleteAsync(Guid id, string userId)
        {
            var remark = await _remarkRepository.GetByIdAsync(id);
            if (remark.HasNoValue)
                throw new ServiceException($"Remark with id: {id} does not exist!");

            if (remark.Value.Author.UserId != userId)
                throw new ServiceException($"User: {userId} is not allowed to delete remark: {id}");

            await _fileHandler.DeleteAsync(remark.Value.Photo.FileId);
            await _remarkRepository.DeleteAsync(remark.Value);
        }
    }
}