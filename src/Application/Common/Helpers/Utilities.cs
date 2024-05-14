namespace CleanArchitecture.Application.Common.Helpers;

public static class Utilities
{
    public static string GenerateRandomString(int length)
    {
        if (length <= 0)
            throw new ArgumentException("can't generate random string less than 1 length.");
        var chars = "0123456789";
        var stringChars = new char[length];
        var random = new Random();

        for (var i = 0; i < stringChars.Length; i++) stringChars[i] = chars[random.Next(chars.Length)];

        var finalString = new string(stringChars);
        return finalString;
    }

    public static int GenerateRandomNumbers()
    {
        var generator = new Random();
        var code = generator.Next(10000, 99999);
        return code; 
    }
}