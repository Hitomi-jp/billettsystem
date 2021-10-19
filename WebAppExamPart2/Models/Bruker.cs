using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppExamPart2.Models
{
    public class Bruker
    {
        [RegularExpression(@"^[a-zA-ZøæåØÆÅ\\-. ]{2,30}$")]
        public String Brukernavn { get; set; }
        [RegularExpression(@"^('(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&].{8,}')$")]
        public String Passord { get; set; }
    }
}
