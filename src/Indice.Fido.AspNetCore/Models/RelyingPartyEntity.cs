/* https://www.w3.org/TR/webauthn/#dictdef-publickeycredentialrpentity */

namespace Indice.AspNetCore.Fido
{
    /// <summary>
    /// Contains data about the Relying Party responsible for the request.
    /// </summary>
    public class RelyingPartyEntity
    {
        /// <summary>
        /// Default Relying Party name.
        /// </summary>
        public const string DEFAULT_NAME = "Indice Fido2 Server";
        /// <summary>
        /// Relying Party specifies the value the credential should be scoped to.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// The name of the Relying Party.
        /// </summary>
        public string Name { get; set; } = DEFAULT_NAME;
    }
}
