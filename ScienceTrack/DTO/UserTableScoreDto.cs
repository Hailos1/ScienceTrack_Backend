namespace ScienceTrack.DTO;

public class UserTableScoreDto
{
    public int roundNumber { get; set; }
    public int stage { get; set; }
    public string username { get; set; }
    public string officialName { get; set; }
    public int administrativeUserChoose { get; set; }
    public int socialUserChoose { get; set; }
    public int financeUserChoose { get; set; }
    public int adminisrativeMax { get; set; }
    public int socialMax { get; set; }
    public int financeMax { get; set; }
}