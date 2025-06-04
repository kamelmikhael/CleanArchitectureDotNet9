namespace SharedKernal.Guards;

public static class Check
{
    public static Func<string, bool> ValidLength(int min, int max)
    {
        return value => value.Length <= max && value.Length >= min;
    }

    public static Func<string, bool> NotEmpty()
    {
        return n => !string.IsNullOrWhiteSpace(n);
    }

    public static Func<string, bool> ValidEmailFormat()
    {
        return n => n.Split('@').Length == 2;
    }
}
