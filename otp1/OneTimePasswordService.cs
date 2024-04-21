using System.Security.Cryptography;

namespace otp1
{
    public class OneTimePasswordService : IOneTimePasswordService
    {
        private const int OTPDuration = 300;
        private static Dictionary<string, DateTime> otpStore = new Dictionary<string, DateTime>();

        public string GenerateOTP()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_-+=<>?";

            var otpLength = 8; // Lungimea OTP generat
            var otp = new char[otpLength];

            using (var rng = RandomNumberGenerator.Create())
            {
                // Generare aleatorie a caracterelor OTP din lista de caractere
                for (int i = 0; i < otpLength; i++)
                {
                    byte[] randomBytes = new byte[1];
                    rng.GetBytes(randomBytes);
                    int randomNumber = randomBytes[0];
                    otp[i] = chars[randomNumber % chars.Length];
                }
            }

            var otpString = new string(otp);
            otpStore.Add(otpString, DateTime.Now.AddSeconds(OTPDuration));
            return otpString;
        }


        public bool ValidateOTP(string otp)
        {
            if (otpStore.ContainsKey(otp) && otpStore[otp] > DateTime.Now)
            {
                otpStore.Remove(otp);
                return true;
            }
            return false;
        }
    }

}
