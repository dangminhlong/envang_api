using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using System.Data;

namespace WebEnvang.Models.Article
{
    public class ArticleService : BaseService
    {
        public async Task<dynamic> GetList(ArticleDTO dto)
        {
            DataSet ds = await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_article_getlist",
                new string[] { "@articletypeid", "@page", "@pageSize" },
                new object[] { dto.ArticleTypeId, dto.Page, dto.PageSize });
            return new
            {
                Data = (from r in ds.Tables[0].AsEnumerable()
                        select new
                        {
                            Id = r.Field<object>("Id"),
                            Name = r.Field<object>("Name"),
                            Description = r.Field<object>("Description"),
                            ArticleTypeId = r.Field<object>("ArticleTypeId"),
                            Content = r.Field<object>("Content"),
                            ImageUrl = r.Field<object>("ImageUrl")
                        }).ToList(),
                Tong = Convert.ToInt32(ds.Tables[1].Rows[0][0])
            };
        }
        public async Task<dynamic> GetItem(ArticleDTO dto)
        {
            DataTable dt = await MsSqlHelper.ExecuteDataTableTask(ConnectionString, "sp_article_get",
                new string[] { "@id" },
                new object[] { dto.Id });
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        Description = r.Field<object>("Description"),
                        ArticleTypeId = r.Field<object>("ArticleTypeId"),
                        Content = r.Field<object>("Content"),
                        ImageUrl = r.Field<object>("ImageUrl"),
                        CreatedOn = r.Field<object>("CreatedOn")
                    }).FirstOrDefault();
        }

        public async Task<dynamic> GetLatest()
        {
            DataTable dt = await MsSqlHelper.ExecuteDataTableTask(ConnectionString, "sp_article_getlatest");
            return (from r in dt.AsEnumerable()
                    select new
                    {
                        Id = r.Field<object>("Id"),
                        Name = r.Field<object>("Name"),
                        FriendlyName = (r.Field<string>("Name")).GenerateSlug(),
                        Description = r.Field<object>("Description"),
                        ImageUrl = r.Field<object>("ImageUrl"),
                        CreatedOn = r.Field<object>("CreatedOn")
                    }).FirstOrDefault();
        }

        public async Task<dynamic> GetListView(int page, int pageSize)
        {
            DataSet ds = await MsSqlHelper.ExecuteDataSetTask(ConnectionString, "sp_article_getlist_view",
                new string[] { "@page", "@pageSize" },
                new object[] { page, pageSize });

            return new
            {
                Data = (from r in ds.Tables[0].AsEnumerable()
                        select new
                        {
                            Id = r.Field<object>("Id"),
                            Name = r.Field<object>("Name"),
                            FriendlyName = (r.Field<string>("Name")).GenerateSlug(),
                            Description = r.Field<object>("Description"),
                            ImageUrl = r.Field<object>("ImageUrl"),
                            CreatedOn = r.Field<object>("CreatedOn")
                        }).ToList(),
                TotalItems = Convert.ToInt32(ds.Tables[1].Rows[0][0])
            };
        }

        public Task Save(ArticleDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_article_save",
                new string[] { "@id", "@name", "@description", "@content", "@imgurl", "@articletypeid", "@userid", "@ip" },
                new object[] { dto.Id, dto.Name, dto.Description, dto.Content, dto.ImageUrl, dto.ArticleTypeId, userId, IP });
        }

        public Task Delete(ArticleDTO dto, string userId, string IP)
        {
            return MsSqlHelper.ExecuteNonQueryTask(ConnectionString, "sp_article_delete",
                new string[] { "@id", "@userid", "@ip" },
                new object[] { dto.Id, userId, IP });
        }
    }
}