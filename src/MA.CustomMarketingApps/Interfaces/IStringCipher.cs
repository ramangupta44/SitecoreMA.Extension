using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MA.CustomMarketingApps.Interfaces
{
    public interface IStringCipher
    {
        string Encrypt(string message);
        string TryDecrypt(string encryptedMessage);
    }
}
