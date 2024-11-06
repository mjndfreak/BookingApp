using Microsoft.AspNetCore.DataProtection;

namespace BookingApp.Business.DataProtection;

public interface IDataProtection
{
    string Protect(string str);
    string Unprotect(string protectedStr);
}