using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnkiGen.Models;

public class Definition
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Word")]
    public int WordId { get; set; }
    public Word Word { get; set; }
    public string Value { get; set; }
}