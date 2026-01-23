namespace IlVecchioForno.API.Utilities;

public static class ActionUtility
{
    public static string ActionName(string name)
    {
        return name.EndsWith("Async")
            ? name[..^5]
            : name;
    }
}