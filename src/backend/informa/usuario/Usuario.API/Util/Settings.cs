using System;

namespace Usuario.API.Util
{
    public static class Settings
    {
        public static string Secret = GerarSenhaAleatoria();
        private static string GerarSenhaAleatoria()
        {
            string chars = "abcdefghjkmnpqrstuvwxyz023456789";
            string pass = "";
            Random random = new Random();
            for (int f = 0; f < 40; f++)
            {
                pass = pass + chars.Substring(random.Next(0, chars.Length - 1), 1);
            }
            return pass;
        }
    }
}
