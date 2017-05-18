using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.BookApi
{
    [Table("TicketPassenger")]
    public class TicketPassenger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public int TicketInfoId { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public string FullName { get; set; }
        public decimal? Baggage { get; set; }
        public decimal? ReturnBaggage { get; set; }

        public virtual TicketInfo Ticket { get; set; }
    }
}