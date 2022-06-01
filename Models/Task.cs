namespace Task.Models;

public class Task
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public User User { get; set; }
    public DateTime Day { get; set; }
}