namespace IlVecchioForno.API.Attributes;

[AttributeUsage(AttributeTargets.Method)]
public sealed class CreatedAtActionAttribute : Attribute
{
    public CreatedAtActionAttribute(string actionName)
    {
        this.ActionName = actionName.EndsWith("Async")
            ? actionName[..^5]
            : actionName;
    }

    public string ActionName { get; }
}