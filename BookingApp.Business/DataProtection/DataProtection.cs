using Microsoft.AspNetCore.DataProtection;

namespace BookingApp.Business.DataProtection;

public class DataProtection : IDataProtection
{
    private readonly IDataProtector _protector;

    public DataProtection(IDataProtectionProvider provider)
    {
        _protector = provider.CreateProtector("BookingAppSecurity");
    }
    
    public string Protect(string str)
    {
        return _protector.Protect(str);
    }

    public string Unprotect(string protectedStr)
    {
        return _protector.Unprotect(protectedStr);
    }
}