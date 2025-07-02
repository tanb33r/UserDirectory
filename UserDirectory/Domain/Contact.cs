namespace UserDirectory.Domain;

public class Contact
{
    public int Id { get; init; }
    public int UserId { get; set; }
    public User User { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string Address { get; set; } = null!;
    public string City { get; set; } = null!;
    public string Country { get; set; } = null!;
}

