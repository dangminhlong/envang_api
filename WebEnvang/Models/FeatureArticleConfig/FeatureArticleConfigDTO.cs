using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.FeatureArticleConfig
{
    public class FeatureArticleConfigDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int ArticleId { get; set; }
        public int GroupId { get; set; }
        public string Style { get; set; }
    }
}