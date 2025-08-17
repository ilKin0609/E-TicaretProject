using E_Ticaret_Project.Application.Abstracts.Services;
using Microsoft.AspNetCore.DataProtection;

namespace E_Ticaret_Project.Persistence.Services;

public class PasswordVault:IPasswordVault
{
    private readonly IDataProtector _protector;
    public PasswordVault(IDataProtectionProvider provider)
        => _protector = provider.CreateProtector("AdminViewPasswords.v1");

    public string Protect(string plain) => _protector.Protect(plain);

    public string? Unprotect(string? cipher)
    {
        if (string.IsNullOrWhiteSpace(cipher)) return null;
        try { return _protector.Unprotect(cipher); }
        catch { return null; } // korlanıbsa boş qaytar
    }
}
