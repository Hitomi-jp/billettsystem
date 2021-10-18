using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebAppExamPart2.Models
{
    public class Billett
    {
        [Key]
        public int Id { get; set; }
        public int KundeId { get; set; }
        public string DestinationFrom { get; set; }

        public string DestinationTo { get; set; }

        public string TicketType { get; set; } 

        public string TicketClass { get; set; } 

        public string DepartureDato { get; set; }

        public string ReturnDato { get; set; }


        public int AntallAdult { get; set; }

        public int AntallChild { get; set; }
    }
}
