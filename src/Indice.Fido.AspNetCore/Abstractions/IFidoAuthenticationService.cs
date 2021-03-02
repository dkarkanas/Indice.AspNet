// WebAuthn Relying Party Operations: https://www.w3.org/TR/webauthn/#sctn-rp-operations

using System.Threading.Tasks;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A service that contains WebAuthn relying party operations, which are registration or authentication ceremony.
    /// </summary>
    public interface IFidoAuthenticationService
    {
        Task InitiateRegistration();
    }
}
