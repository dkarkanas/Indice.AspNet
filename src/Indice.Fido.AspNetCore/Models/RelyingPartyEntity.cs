/* https://www.w3.org/TR/webauthn/#dictdef-publickeycredentialrpentity */

namespace Indice.AspNetCore.Fido
{
    public class RelyingPartyEntity
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
