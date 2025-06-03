using Domain.Roles;
using SharedKernal.Abstraction;
using SharedKernal.Primitives;

namespace Domain.Users;

public sealed class User : Entity, IAuditableEntity
{
    public UserName Username { get; private set; }

    public Email Email { get; private set; }

    public string Password { get; private set; }

    public DateTime CreatedOnUtc { get; set; }

    public DateTime? UpdatedOnUtc { get; set; }

    public ICollection<Role> Roles { get; set; }

    private User() : base()
    { }

    private User(
        Guid id,
        UserName username, 
        Email email, 
        string password) : base(id)
    {
        Username = username;
        Email = email;
        Password = password;
        CreatedOnUtc = DateTime.UtcNow;
    }

    public static Result<User> Create(Guid id,
        UserName username,
        Email email,
        string password,
        bool isEmailExists)
    {
        if(isEmailExists)
        {
            return Result.Failure<User>(UserErrors.EmailNotUnique);
        }

        return new User(id, username, email, password);
    }
}
