namespace Task.Models;

public class User
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
}