using System.ComponentModel.DataAnnotations;

namespace DeltaBrainsJSCAppBE.DTOs.Response
{
    public class UserRes
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? RoleName {  get; set; }
        public string? DeviceToken { get; set; }
    }
}
