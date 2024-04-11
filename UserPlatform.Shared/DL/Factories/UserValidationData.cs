using Shared.Patterns.CQRS.Queries;
using System.Linq.Expressions;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.DL.Factories;

public class UserValidationData(IEnumerable<UserData> users)
{
    public IEnumerable<UserData> Users { get; set; } = users;
}

public sealed class UserData(string companyName) : BaseReadModel
{
    public string CompanyName { get; private set; } = companyName;
}

public sealed class UserDataQuery : BaseQuery<User, UserData>
{
    public override Expression<Func<User, UserData>> Map()
    {
        return e => new(e.CompanyName);
    }
}
