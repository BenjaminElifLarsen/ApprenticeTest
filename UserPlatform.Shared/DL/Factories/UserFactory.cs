using Shared.Patterns.ResultPattern;
using Shared.Patterns.ResultPattern.BadRequest;
using System.Text.RegularExpressions;
using UserPlatform.Shared.Communication.Models;
using UserPlatform.Shared.DL.Models;

namespace UserPlatform.Shared.DL.Factories;

public class UserFactory : IUserFactory
{
    private Regex _passwordSmallLetter;
    private Regex _passwordCapitalLetter;
    private Regex _passwordDigit;
    private Regex _passwordSpecial;
    private byte _passwordMinLength;
    private byte _passwordMaxLength;

    public UserFactory()
    {
        _passwordSmallLetter = new("[a-z]+");
        _passwordCapitalLetter = new("[A-Z]+");
        _passwordDigit = new("[/d]+");
        _passwordSpecial = new("[!|+|\\-|#|.|,|^|*]");
        _passwordMinLength = 8;
        _passwordMaxLength = 128;
    }

    public Result<User> Build(UserCreationRequest request, UserValidationData validationData)
    {
        BinaryFlag flag = new();
        if(request is null)
        {
            flag += UserFactoryErrors.RequestIsNull;
            return new BadRequestResult<User>(flag);
        }
        if (string.IsNullOrWhiteSpace(request.CompanyName)) // could benefit from specification pattern, perhaps
            flag += UserFactoryErrors.CompanyNameNotSat;
        if (validationData.Users.Any(x => string.Equals(x.CompanyName, request.CompanyName)))
            flag += UserFactoryErrors.CompanyNameInUse;
        if (string.IsNullOrWhiteSpace(request.City) || string.IsNullOrWhiteSpace(request.Street))
            flag += UserFactoryErrors.LocationInvalid;
        if (!_passwordCapitalLetter.IsMatch(request.Password))
            flag += UserFactoryErrors.PasswordMissingCapital;
        if (!_passwordSmallLetter.IsMatch(request.Password))
            flag += UserFactoryErrors.PasswordMissingSmall;
        if (!_passwordDigit.IsMatch(request.Password))
            flag += UserFactoryErrors.PasswordMissingDigit;
        if (_passwordSpecial.IsMatch(request.Password))
            flag += UserFactoryErrors.PasswordMissingSpecial;
        if (request.Password.Length < _passwordMinLength)
            flag += UserFactoryErrors.PasswordToShort;
        if (request.Password.Length > _passwordMaxLength)
            flag += UserFactoryErrors.PasswordToLong;
        if (string.Equals(request.Password, request.Password))
            flag += UserFactoryErrors.NotSamePassword;
        if (string.IsNullOrWhiteSpace(request.Phone) || string.IsNullOrWhiteSpace(request.Email))
            flag += UserFactoryErrors.NoContactInformationSat;
        if (!flag)
            return new BadRequestResult<User>(flag);
        // TODO: hash and salt password. Consider interface
        var hashedPassword = "1234";
        UserLocation location = new(request.City, request.Street);
        UserContact contact = new(request.Email, request.Phone);
        User user = new(request.CompanyName, contact, location, hashedPassword);
        return new SuccessResult<User>(user);
    }




}
