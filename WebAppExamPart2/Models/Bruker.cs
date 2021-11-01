using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppExamPart2.Models
{
    [ExcludeFromCodeCoverage]
    public class Bruker
    { 
        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,20}")]
        public String Brukernavn { get; set; }
        [RegularExpression(@"[a-zA-Z0-9]{6,}")]
        public String Passord { get; set; }
    }
}
