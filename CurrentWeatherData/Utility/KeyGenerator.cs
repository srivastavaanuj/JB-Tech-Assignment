using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CurrentWeatherData.Utility 
{
    public class KeyGenerator
    {
        public static string  Generate()
        {
            var key = new byte[32];
            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);
            return Convert.ToBase64String(key);
        }
    }
}
