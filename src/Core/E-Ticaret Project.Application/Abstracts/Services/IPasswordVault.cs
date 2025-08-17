namespace E_Ticaret_Project.Application.Abstracts.Services;

public interface IPasswordVault
{
    string Protect(string plain);
    string? Unprotect(string? cipher);
}
