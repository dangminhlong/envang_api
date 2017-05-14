using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.FeatureArticleConfigGroup
{
    [Table("FeatureArticleConfigGroup")]
    public class FeatureArticleConfigGroup
    {
        public void Copy(FeatureArticleConfigGroup data)
        {
            this.Name = data.Name;
            this.FriendlyName = data.FriendlyName;
            this.Order = data.Order;
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public int Order { get; set; }
    }
}