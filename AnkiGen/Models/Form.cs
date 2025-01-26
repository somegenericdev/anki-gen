using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnkiGen.Models;

public class Form
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Word")]
    public int WordId { get; set; }
    public Word Word { get; set; }
    public string Value { get; set; }
    public string Description { get; set; }
}