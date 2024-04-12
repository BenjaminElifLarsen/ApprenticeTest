using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.Helpers;

public interface IPasswordHasher
{
    public string Hash(User user, string password);
    public bool VerifyPassword(User user, string providedPassword);
}
