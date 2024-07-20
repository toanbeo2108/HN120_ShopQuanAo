using System.Text;

namespace HN120_ShopQuanAo.API.EmailConfig.ViewModel
{
    public static class PasswordGenerator
    {
        private static readonly Random Random = new Random();

        public static string GeneratePassword(int length = 8)
        {
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_+[]{}|;:,.<>?";

            var allChars = upperCase + lowerCase + digits + specialChars;
            var password = new StringBuilder();

            // Ensure the password has at least one upper case letter, one lower case letter, one digit, and one special character
            password.Append(upperCase[Random.Next(upperCase.Length)]);
            password.Append(lowerCase[Random.Next(lowerCase.Length)]);
            password.Append(digits[Random.Next(digits.Length)]);
            password.Append(specialChars[Random.Next(specialChars.Length)]);

            // Fill the rest of the password length with random characters
            for (int i = 4; i < length; i++)
            {
                password.Append(allChars[Random.Next(allChars.Length)]);
            }

            // Shuffle the characters in the password
            return new string(password.ToString().OrderBy(c => Random.Next()).ToArray());
        }
    }
}
