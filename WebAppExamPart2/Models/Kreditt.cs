using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppExamPart2.Models
{
    public class Kreditt
    {
        public int Id { get; set; }
        public int KundeId { get; set; }

        //[RegularExpression(@"[0-9]{16}")]
        public string Kortnummer { get; set; }
        //[RegularExpression(@"[a-zA-ZÊ¯Â∆ÿ≈\.\ \-]{2,40}")]
        public string KortHolderNavn { get; set; }
        //[RegularExpression(@"[0-9]{2}[/][0-9]{2}")]
        public string KortUtlopsdato { get; set; }
        //[RegularExpression(@"[0-9]{3}")]
        public string Cvc { get; set; }
    }
}