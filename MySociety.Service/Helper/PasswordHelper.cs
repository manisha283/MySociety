namespace MySociety.Service.Helper;

public static class PasswordHelper
{
    public static string HashPassword(string password)
    {
        // return password;
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // if (password == hashedPassword)
        // {
        //     return true;
        // }
        // else
        // {
        //     return false;
        // }
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}
