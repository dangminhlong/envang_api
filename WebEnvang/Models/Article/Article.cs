using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Article
{
    [Table("Article")]
    public class Article
    {
        public void Copy(ArticleDTO dto)
        {
            this.Name = dto.Name;
            this.FriendlyName = dto.FriendlyName;
            this.Description = dto.Description;
            this.Content = dto.Content;
            this.ImageUrl = dto.ImageUrl;
            this.ArticleTypeId = dto.ArticleTypeId;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int ArticleTypeId { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}