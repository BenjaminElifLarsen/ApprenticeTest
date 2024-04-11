using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.IPL.Repository.Interfaces;

public interface IUserRepository
{
    public void Create(User user);
    public void Delete(User user);
    public Task<User> GetSingleAsync(string companyName, string hashedPassword);
}
