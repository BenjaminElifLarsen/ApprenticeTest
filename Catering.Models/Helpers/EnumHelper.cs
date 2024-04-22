using Catering.Shared.DL.Models.Enums;

namespace Catering.Shared.Helpers;

public static class EnumHelper
{
    public static bool IsEnumValueValid(OrderState e)
    {
        var validValues = Enum.GetValues<OrderState>().Select(x => (int)x);
        return validValues.Any(x => x == (int)e);
    }
}
