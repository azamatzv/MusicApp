using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;

namespace N_Tier.Application.Services.Impl;

public class MusicService : IMusicService
{
    private readonly IMusicRepository _musicRepository;
    private readonly IConfiguration _configuration;

    public MusicService(IMusicRepository musicRepository, IConfiguration configuration)
    {
        _musicRepository = musicRepository;
        _configuration = configuration;
    }

    public async Task<Music> AddMusicAsync(IFormFile file, string name, Guid authorId, Guid genreId)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("Invalid file.");

        string rootPath = _configuration["MusicSettings:RootPath"];
        string fileName = $"{Guid.NewGuid()}_{file.FileName}";
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), rootPath, fileName);

        string directory = Path.GetDirectoryName(fullPath) ?? string.Empty;
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        string relativePath = Path.Combine(rootPath, fileName).Replace("\\", "/");

        var music = new Music
        {
            Name = name,
            AuthorId = authorId,
            GenreId = genreId,
            FilePath = relativePath,
            CreatedOn = DateTime.UtcNow
        };

        return await _musicRepository.AddAsync(music);
    }

    public async Task<byte[]> PlayMusicAsync(string fileName)
    {
        string rootPath = _configuration["MusicSettings:RootPath"];
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), rootPath, fileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Music file not found.");

        return await File.ReadAllBytesAsync(fullPath);
    }

    public async Task<byte[]> DownloadMusicAsync(string fileName)
    {
        string rootPath = _configuration["MusicSettings:RootPath"];
        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), rootPath, fileName);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Music file not found.");

        return await File.ReadAllBytesAsync(fullPath);
    }
}
