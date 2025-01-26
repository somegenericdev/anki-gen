using System.ComponentModel.DataAnnotations;

namespace AnkiGen.Models;

public class Word
{
    [Key]
    public int Id { get; set; }
    public string Value { get; set; }
    public PartOfSpeechEnum Pos { get; set; }
    public ICollection<Definition> Definitions { get; set; }
    public ICollection<Form> Forms { get; set; }
}