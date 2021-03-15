namespace Indice.AspNetCore.Fido.Models
{
    /// <summary>
    /// Provides information about a public key/private key pair, which is a credential for registering/logging in to a service using an asymmetric key pair.
    /// </summary>
    public class FidoPublicKeyCredential
    {
        /// <summary>
        /// A globally unique identifier for this <see cref="FidoPublicKeyCredential"/>.
        /// </summary>
        public byte[] Id { get; }
        /// <summary>
        /// A unique identifier for the user.
        /// </summary>
        public string UserId { get; }
        /// <summary>
        /// A friendly name for the authenticator.
        /// </summary>
        public string DeviceFriendlyName { get; }
        /// <summary>
        /// Used to map a specific public key credential to a specific user account with the Relying Party.
        /// </summary>
        public byte[] UserHandle { get; }
    }
}
