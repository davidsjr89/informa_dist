using System;
using System.Text;

namespace Usuario.API.Util
{
    public static class Settings
    {
        public readonly static string Secret = GerarSenhaAleatoria();
        public static string Criptografia(string texto)
        {
            return Convert.ToBase64String(Encoding.ASCII.GetBytes(texto));
        }
        private static string GerarSenhaAleatoria()
        {
            string chars = "abcdefghjkmnpqrstuvwxyz023456789";
            string pass = "";
            Random random = new();
            for (int f = 0; f < 40; f++)
            {
                pass += chars.Substring(random.Next(0, chars.Length - 1), 1);
            }
            return pass;
        }
    }
}
