using FlowZone.Api.Data;
using FlowZone.Api.Data.Entities;
using FlowZone.shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace FlowZone.Api.Services
{
    public class AvatarService(DataContext context)
    {
        private readonly DataContext _context = context;
        public async Task<Guid> SaveAvatarAsync(AvatarDto avatarDto)
        {
            var imagePath = await SaveAvatarImage(avatarDto.ImageData, avatarDto.FilePath);

            var avatar = new Avatar
            {
                Name = avatarDto.Name,
                Price = avatarDto.Price,
                ImagePath = imagePath
            };

            _context.Avatars.Add(avatar);
            await _context.SaveChangesAsync();

            return avatar.AvatarId; // Return the ID of the newly created avatar
        }


        public async Task DeleteAvatarAsync(Guid avatarId)
        {
            var avatar = await _context.Avatars.FindAsync(avatarId);
            if (avatar != null)
            {
                _context.Avatars.Remove(avatar);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new ArgumentException("Avatar not found", nameof(avatarId));
            }
        }

        public async Task<string> SaveAvatarImage(byte[] imageData, string fileName)
        {
            var filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                await fileStream.WriteAsync(imageData, 0, imageData.Length);
            }

            return filePath;
        }
    }
}
