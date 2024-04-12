using Microsoft.AspNetCore.Identity;
using UserPlatform.Shared.DL.Models;
using UserPlatform.Shared.Helpers;

namespace UserPlatform.Helpers;
/// <summary>
/// Kind of acts as a wrapper around Microsofts own PasswordHasher
/// </summary>
public sealed class PasswordHasher : IPasswordHasher
{
    public string Hash(User user, string password)
    {
        return new Hasher().HashPassword(user, password);
    }

    public bool VerifyPassword(User user, string providedPassword)
    {
        var result = new Hasher().VerifyHashedPassword(user, user.Password, providedPassword);
        return result is not PasswordVerificationResult.Failed;
    }

    private class Hasher : PasswordHasher<User>
    {
    }
}
