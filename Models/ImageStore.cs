using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVCAuth1.Models;

public class ImageStore
{
    [Key]
    [Required]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id {get; set;}
    [Required]
    [DataType("image")]
    public required byte[] ImageData {get; set;}

    [NotMapped]
    [Required]
    public IFormFile? ImageFile {set; get;}
    [NotMapped]
    public string? ImageDataAsBase64 {set; get;}
}