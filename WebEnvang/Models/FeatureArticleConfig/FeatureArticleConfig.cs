using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.FeatureArticleConfig
{
    [Table("FeatureArticleConfig")]
    public class FeatureArticleConfig
    {
        public void Copy(FeatureArticleConfig dto)
        {
            this.Name = dto.Name;
            this.Order = dto.Order;
            this.ArticleId = dto.ArticleId;
            this.GroupId = dto.GroupId;
            this.Style = dto.Style;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int ArticleId { get; set; }
        public int GroupId { get; set; }
        public string Style { get; set; }
    }
}