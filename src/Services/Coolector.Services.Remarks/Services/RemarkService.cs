using System;
using System.Threading.Tasks;
using Coolector.Services.Remarks.Domain;
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

        public async Task CreateAsync(string userId, Guid categoryId, File photo, Position position,
            string description = null)
        {
            var user = await _userRepository.GetByUserIdAsync(userId);
            if (user.HasNoValue)
                throw new ArgumentException($"User with id: {userId} has not been found.");

            var category = await _categoryRepository.GetByIdAsync(categoryId);
            if (category.HasNoValue)
                throw new ArgumentException($"Category with id: {userId} has not been found.");

            var location = Location.Create(position);
            var remarkPhoto = RemarkPhoto.Empty;
            await _fileHandler.UploadAsync(photo, fileId =>
            {
                remarkPhoto = RemarkPhoto.Create(fileId);
            });
            var remark = new Remark(user.Value, category.Value, location, remarkPhoto, description);
            await _remarkRepository.AddAsync(remark);
        }
    }
}