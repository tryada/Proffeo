namespace Proffeo.Models.Users;

public class User
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Email { get; private set; }

    private User(Guid id, string name, string email)
    {
        Id = id;
        Name = name;
        Email = email;
    }

    public void UpdateName(string name) => Name = name;
    public void UpdateEmail(string email) => Email = email;
    
#pragma warning disable CS8618
    private User() { }
#pragma warning restore CS8618

    public static User Create(string name, string email) =>
        new(Guid.NewGuid(), name, email);
}