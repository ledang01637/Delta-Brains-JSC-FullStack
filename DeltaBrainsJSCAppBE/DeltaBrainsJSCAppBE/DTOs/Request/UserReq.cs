using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.DTOs.Request
{
    public class UserReq
    {
        [Required]
        public string? Name { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }

        [Required]
        public int RoleId {  get; set; }

        public string? DeviceToken { get; set; }
    }
}
