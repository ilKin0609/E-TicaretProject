namespace E_Ticaret_Project.Domain.Entities;

public class KeywordSearchStat:BaseEntity
{
    public string Keyword { get; set; }
    public string Slug { get; set; }
    public long Count { get; set; } = 0;
    public DateTime LastSearchedAt { get; set; } = DateTime.UtcNow;
}
