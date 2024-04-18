using Shared.Helpers.Models;
using UserPlatform.Models.User.Requests;

namespace UserPlatform.Models.Internal;

public sealed class UserUpdateDTO
{
    public Guid Id { get; private set; }
    public ChangeCarrier<string>? Street { get; private set; }
    public ChangeCarrier<string?> City { get; private set; }

    private UserUpdateDTO(Guid userId, string? street, string? city)
    {
        Street = null!;
        City = null!;
        Id = userId;
        if (street is not null)
            Street = new(street);
        if (city is not null)
            City = new(city);
    }

    public UserUpdateDTO(Guid userId, UserUpdateRequest request) : this(userId, request.Street, request.City)
    {
        
    }
}
