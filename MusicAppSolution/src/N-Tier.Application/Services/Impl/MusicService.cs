using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using N_Tier.Core.Entities;
using N_Tier.DataAccess.Repositories;
using N_Tier.DataAccess.Repositories.Impl;

namespace N_Tier.Application.Services.Impl;

public class MusicService : IMusicService
{
    private readonly IMusicRepository _musicRepository;
    private readonly IDownloadsRepository _downloadsRepository;
    private readonly IConfiguration _configuration;

    public MusicService(IMusicRepository musicRepository, IConfiguration configuration, IDownloadsRepository downloadsRepository)
    {
        _musicRepository = musicRepository;
        _configuration = configuration;
        _downloadsRepository = downloadsRepository;
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

    public async Task<byte[]> PlayMusicAsync(Guid musicId)
    {
        try
        {
            var music = await _musicRepository.GetFirstAsync(m => m.Id == musicId);
            if (music == null)
                throw new FileNotFoundException("Music record not found in the database.");

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), music.FilePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException("Music file not found.");


            return await File.ReadAllBytesAsync(fullPath);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PlayMusicAsync: {ex.Message}");
            throw;
        }
    }



    public async Task<byte[]> DownloadMusicAsync(Guid musicId, Guid accountId, string createdBy)
    {
        var music = await _musicRepository.GetFirstAsync(m => m.Id == musicId);
        if (music == null)
            throw new FileNotFoundException("Music record not found in the database.");

        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), music.FilePath);

        Console.WriteLine(fullPath);

        if (!File.Exists(fullPath))
            throw new FileNotFoundException("Music file not found.");

        var download = new Downloads
        {
            MusicId = music.Id,
            AccountsId = accountId,
            CreatedBy = createdBy,
            CreatedOn = DateTime.UtcNow
        };

        await _downloadsRepository.AddAsync(download);


        return await File.ReadAllBytesAsync(fullPath);
    }

}
