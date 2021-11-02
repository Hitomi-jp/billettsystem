using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WebAppExamPart2.Models
{
    [ExcludeFromCodeCoverage]
    public class Strekning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string StrekningId { get; set; }

        public string Fra {get; set; }

        public string Til { get; set; }
        
    }
}