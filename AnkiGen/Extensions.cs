using AnkiGen.DTOs;
using AnkiNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiGen
{
    public static class Extensions
    {
        public static string GetDescription<T>(this T value) where T:struct
        {
            DescriptionAttribute attribute = value.GetType()
                    .GetField(value.ToString())
                    ?.GetCustomAttributes(typeof(DescriptionAttribute), false)
                    .SingleOrDefault() as DescriptionAttribute;
            return attribute == null ? "" : attribute.Description;
        }

        public static T ParseEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static AnkiCollection ToAnkiCollection(this IEnumerable<CardDto> list, string deckName)
        {

            var noteType = new AnkiNoteType(
                name: "Basic",
                cardTypes: new[] {
                    new AnkiCardType(
                        Name: "Card 1",
                        Ordinal: 0,
                        QuestionFormat: "{{Front}}",
                        AnswerFormat: "{{Front}}<hr id=\"answer\">{{Back}}"

                    )
                },
                fieldNames: new[] { "Front", "Back" },
                css: ".card {font-family: arial;font-size: 20px;text-align: center;color: black;background-color: white;}"
            );


            var collection = new AnkiCollection();

            var noteTypeId = collection.CreateNoteType(noteType);
            var deckId = collection.CreateDeck(deckName);
            list.ToList().ForEach(x =>
            {
                collection.CreateNote(deckId, noteTypeId, x.Front, x.Back);
            });
            return collection;
        }

        public static string Implode(this IEnumerable<string> strings, string separator)
        {
            return string.Join(separator, strings);
        }

    }
}
