using Microsoft.AspNetCore.DataProtection;

namespace BookingApp.Business.DataProtection
{
    public class DataProtection : IDataProtection
    {
        private readonly IDataProtector _protector;

        // Constructor to initialize the data protector with a specific purpose string
        public DataProtection(IDataProtectionProvider provider)
        {
            _protector = provider.CreateProtector("BookingAppSecurity");
        }
        
        // Method to protect (encrypt) a string
        public string Protect(string str)
        {
            return _protector.Protect(str);
        }

        // Method to unprotect (decrypt) a string
        public string Unprotect(string protectedStr)
        {
            return _protector.Unprotect(protectedStr);
        }
    }
}