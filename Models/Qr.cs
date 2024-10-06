using System.ComponentModel.DataAnnotations;

namespace QrCodeGenerator.Models;

public class Qr
{
    public int Id { get; set; }
    
    [Url]
    public string? Url { get; set; }
    
    [EmailAddress]
    public string? Email { get; set; }
    
    [Phone]
    public string? PhoneNumber { get; set; }
    
    [Required]
    public string DataType { get; set; }
    
    public string? ImagePath { get; set; }
}