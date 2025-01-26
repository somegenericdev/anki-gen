using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiGen.DTOs
{
    public class WordDto
    {
        public string Name { get; set; }
        public List<string> Definitions { get; set; }

        public WordDto(string name, List<string> definitions)
        {
            Name = name;
            Definitions = definitions;
        }
    }
}
