namespace ASM1.Service.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? Phone { get; set; }
        public string Password { get; set; } = null!;
        public string Role { get; set; } = null!;
        public int? DealerId { get; set; }
        public int? ManufacturerId { get; set; }
    }
}