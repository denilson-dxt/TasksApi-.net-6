namespace Task.Dtos;

public class UserDto
{
    public int Id { get; set; }
    public string Firstname { get; set; } = string.Empty;
    public string Lastname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public string Password{ get; set; } = String.Empty;
    
}