using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebEnvang.Models.Article
{
    public class ArticleDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImageUrl { get; set; }
        public int ArticleTypeId { get; set; }

        public int Page { get; set; }
        public int PageSize { get; set; }
    }
}