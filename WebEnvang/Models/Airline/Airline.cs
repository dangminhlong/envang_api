using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Airline
{
    [Table("Airline")]
    public class Airline
    {
        public void Copy(Airline dto)
        {
            this.Name = dto.Name;
            this.Code = dto.Code;
            this.DieuKienHanhLy = dto.DieuKienHanhLy;
            this.DieuKienVe = dto.DieuKienVe;
            this.PhiHang = dto.PhiHang;
            this.PhiHangTreEm = dto.PhiHangTreEm;
            this.PhiHangEmBe = dto.PhiHangEmBe;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string DieuKienHanhLy { get; set; }
        public string DieuKienVe { get; set; }
        public decimal PhiHang { get; set; }
        public decimal PhiHangTreEm { get; set; }
        public decimal PhiHangEmBe { get; set; }
        public bool IsDeleted { get; set; }
        public string IP { get; set; }
        public string UserId { get; set; }
    }
}