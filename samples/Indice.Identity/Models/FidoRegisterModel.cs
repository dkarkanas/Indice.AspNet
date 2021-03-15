using System.ComponentModel.DataAnnotations;

namespace Indice.Identity.Models
{
    public class FidoRegisterModel
    {
        [Required]
        public string UserId { get; set; }
        public string DeviceFriendlyName { get; set; }
    }
}
