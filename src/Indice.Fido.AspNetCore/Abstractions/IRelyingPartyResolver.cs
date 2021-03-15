/* Relying Party Identifier https://www.w3.org/TR/webauthn/#relying-party-identifier */

using Indice.AspNetCore.Fido.Models;

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// A service that helps discover information about the Relying Party.
    /// </summary>
    public interface IRelyingPartyResolver
    {
        /// <summary>
        /// Resolves the Relying Party information based on host's domain name or based on selection.
        /// </summary>
        RelyingPartyEntity Resolve();
    }
}
