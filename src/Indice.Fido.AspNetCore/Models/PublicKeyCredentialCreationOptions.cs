/* https://www.w3.org/TR/webauthn/#dictdef-publickeycredentialcreationoptions */

namespace Indice.AspNetCore.Fido
{
    public class PublicKeyCredentialCreationOptions
    {
        public RelyingPartyEntity RelyingParty { get; set; }
    }
}
