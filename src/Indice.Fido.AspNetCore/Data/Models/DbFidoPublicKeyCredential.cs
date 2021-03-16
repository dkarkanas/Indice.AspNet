using Indice.AspNetCore.Fido.Models;

namespace Indice.AspNetCore.Fido.EntityFrameworkCore
{
    public class DbFidoPublicKeyCredential
    {
        public byte[] Id { get; set; }
        public string UserId { get; set; }
        public string DeviceFriendlyName { get; }
        public byte[] UserHandle { get; set; }

        public FidoPublicKeyCredential ToModel() => new() {
            Id = Id,
            UserId = UserId,
            DeviceFriendlyName = DeviceFriendlyName,
            UserHandle = UserHandle
        };
    }
}
