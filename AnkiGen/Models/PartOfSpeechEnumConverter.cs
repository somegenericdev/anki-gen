using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace AnkiGen.Models;

public class PartOfSpeechEnumConverter : ValueConverter<PartOfSpeechEnum, string>
{
    public PartOfSpeechEnumConverter() 
        : base(
            v => v.ToString(),
            v => v.Replace("-", "_").ParseEnum<PartOfSpeechEnum>()
            )

    {
    }
}
