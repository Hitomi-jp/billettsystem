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
    public class Rute
    {
        // [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.None)]

        // [RegularExpression(@"[A-Z]{3}[\-][A-Z]{3}")]
        public int Id { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public string BoatNavn { get; set; }

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public String RuteFra { get; set; }   

        [RegularExpression(@"[a-zA-ZæøåÆØÅ. \-]{2,30}")]
        public String RuteTil { get; set; }

        [RegularExpression(@"[0-9]+")]
        public int PrisEnvei { get; set; }

        [RegularExpression(@"[0-9]+")]
        public int PrisToVei { get; set; }

        [RegularExpression(@"[0-9]{1,2}")]
        public string PrisRabattBarn { get; set; }

        [RegularExpression(@"[0-9]+")]
        public int PrisStandardLugar { get; set; }

        [RegularExpression(@"[0-9]+")]
        public int PrisPremiumLugar { get; set; }

        [RegularExpression(@"[012][0-9][:][0-5][0-9]")]
        public String Avgang { get; set; }

        [RegularExpression(@"[0-9]+")]
        public int AntallDagerEnVei { get; set; }

        [RegularExpression(@"[0-9]+")]
        public int AntallDagerToVei { get; set; }

    }
}