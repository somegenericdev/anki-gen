using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnkiGen.DTOs
{
    public class CardDto
    {
        public string Front { get; set; }
        public string Back { get; set; }

        public CardDto(string front, string back)
        {
            Front = front;
            Back = back;
        }
    }
}
