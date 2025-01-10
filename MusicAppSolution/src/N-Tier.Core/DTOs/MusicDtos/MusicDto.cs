namespace N_Tier.Core.DTOs.MusicDtos;

public class MusicDto
{
    public string Name { get; set; }
    public Guid AuthorId { get; set; }
    public Guid GenreId { get; set; }
    public string FilePath { get; set; }
}
