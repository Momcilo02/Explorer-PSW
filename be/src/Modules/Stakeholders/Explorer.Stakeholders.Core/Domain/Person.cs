using Explorer.BuildingBlocks.Core.Domain;
using System.Net.Mail;

namespace Explorer.Stakeholders.Core.Domain;

public enum TouristStatus
{
    GOLDEN,
    SILVER,
    BRONZE,
    BASIC
}

public enum TouristRank
{
    EXPLORER,
    SURVIVOR,
    TRAVELLER,
    CAPTAIN,
    ULTIMATE
}
public class Person : Entity
{
    public long UserId { get; init; }
    public string Name { get; init; }
    public string Surname { get; init; }
    public string ProfilePictureUrl { get; init; }
    public string Biography { get; init; }
    public string Motto { get; init; }
    public string Email { get; init; }
    public int TouristLevel { get; init; }
    public int TouristXp { get; init; }

    public DateTime LastWheelSpinTime { get; init; }
    public List<int> Followers { get; init; } = new List<int>();
    public List<int> Following { get; init; } = new List<int>();
    public List<int> ClubMember { get; init; } = new List<int>();
    public TouristStatus TouristStatus { get; init; }
    public TouristRank? TouristRank { get; set; }

    public Person(long userId, string name, string surname, string email, string profilePictureUrl, string biography, string motto,int touristLevel, int touristXp,DateTime lastWheelSpinTime, TouristStatus touristStatus)
    {
        UserId = userId;
        Name = name;
        Surname = surname;
        Email = email;
        ProfilePictureUrl = profilePictureUrl;
        Biography = biography;
        Motto = motto;
        TouristLevel = touristLevel;
        TouristXp = touristXp;
        LastWheelSpinTime = lastWheelSpinTime;
        TouristStatus = touristStatus;
        SetRank();
        Validate();
    }

    private void SetRank()
    {
        switch (TouristLevel)
        {
            case < 2:
                TouristRank = Domain.TouristRank.EXPLORER;
                break;
            case < 4:
                TouristRank = Domain.TouristRank.SURVIVOR;
                break;
            case < 6:
                TouristRank = Domain.TouristRank.TRAVELLER;
                break;
            case < 8:
                TouristRank = Domain.TouristRank.CAPTAIN;
                break;
            default:
                TouristRank = Domain.TouristRank.ULTIMATE;
                break;
        }
    }

    private void Validate()
    {
        if (UserId == 0) throw new ArgumentException("Invalid UserId");
        if (string.IsNullOrWhiteSpace(Name)) throw new ArgumentException("Invalid Name");
        if (string.IsNullOrWhiteSpace(Surname)) throw new ArgumentException("Invalid Surname");
        if (!MailAddress.TryCreate(Email, out _)) throw new ArgumentException("Invalid Email");
    }

}