namespace Catering.Shared.DL.Factories.DishFactory;

public class DishValidationData(IEnumerable<string> namesInUse)
{
    public IEnumerable<string> NamesInUse { get; private set; } = namesInUse;
}
