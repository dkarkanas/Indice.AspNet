/* Relying Party Identifier https://www.w3.org/TR/webauthn/#relying-party-identifier */

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A service that helps resolve the identifier of the Relying Party (e.x. Identity Server).
    /// </summary>
    public interface IRelyingPartyIdResolver
    {
        /// <summary>
        /// Resolves the Relying Party Identifier based on host's domain name or based on selection.
        /// </summary>
        string Resolve();
    }
}
