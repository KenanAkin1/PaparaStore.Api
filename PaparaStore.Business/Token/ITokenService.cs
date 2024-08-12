using PaparaStore.Data.Domain;

namespace PaparaStore.Business.Token;
public interface ITokenService
{
    Task<string> GetToken(User user);
}