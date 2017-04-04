using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace WebEnvang.Controllers
{
    [Authorize]
    public class FileManagerController : ApiController
    {
        [HttpGet]
        public async Task<dynamic> GetListFolders(string folder)
        {
            var server = System.Web.HttpContext.Current.Server;
            Task<List<Item>> task = Task<List<Item>>.Run(() =>
            {
                var itemList = new List<Item>();
                if (string.IsNullOrEmpty(folder))
                {
                    string rootPath = server.MapPath("~/userfiles");
                    var listFolders = Directory.GetDirectories(rootPath);
                    listFolders.ToList().ForEach(path =>
                    {
                        var item = new Item();
                        item.Type = 0;
                        var lidx = path.LastIndexOf(@"\") + 1;
                        item.Name = path.Substring(lidx);
                        item.RelativePath = path.Replace(server.MapPath("~/"), "/").Replace(@"\", "/");
                        itemList.Add(item);
                    });
                }
                else
                {
                    string rootPath = server.MapPath("~" + folder);
                    var listFolders = Directory.GetDirectories(rootPath);
                    listFolders.ToList().ForEach(path =>
                    {
                        var item = new Item();
                        item.Type = 0;
                        var lidx = path.LastIndexOf(@"\") + 1;
                        item.Name = path.Substring(lidx);
                        item.RelativePath = path.Replace(server.MapPath("~/"), "/").Replace(@"\", "/");
                        itemList.Add(item);
                    });
                }
                return itemList;
            });
            return await task;
        }
        [HttpGet]
        public async Task<dynamic> GetListFiles(string folder)
        {
            var server = System.Web.HttpContext.Current.Server;
            Task<List<Item>> task = Task<List<Item>>.Run(() =>
            {
                var itemList = new List<Item>();
                if (string.IsNullOrEmpty(folder))
                {
                    string rootPath = server.MapPath("~/userfiles");
                    var listFolders = Directory.GetFiles(rootPath);
                    listFolders.ToList().ForEach(path =>
                    {
                        var item = new Item();
                        item.Type = 1;
                        var lidx = path.LastIndexOf(@"\") + 1;
                        item.Name = path.Substring(lidx);
                        item.RelativePath = path.Replace(server.MapPath("~/"), "/").Replace(@"\", "/");
                        itemList.Add(item);
                    });
                }
                else
                {
                    string rootPath = server.MapPath("~" + folder);
                    var listFolders = Directory.GetFiles(rootPath);
                    listFolders.ToList().ForEach(path =>
                    {
                        var item = new Item();
                        item.Type = 1;
                        var lidx = path.LastIndexOf(@"\") + 1;
                        item.Name = path.Substring(lidx);
                        item.RelativePath = path.Replace(server.MapPath("~/"), "/").Replace(@"\", "/");
                        itemList.Add(item);
                    });
                }
                return itemList;
            });
            return await task;
        }
        [HttpPost]
        public dynamic UploadFile()
        {
            HttpRequestMessage request = this.Request;
            if (!request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(new HttpResponseMessage((HttpStatusCode.UnsupportedMediaType)));
            }
            var context = HttpContext.Current.Request;
            try
            {
                string path = context.Form["path"];
                string serverPath = HttpContext.Current.Server.MapPath("~" + path);
                if (context.Files.Count > 0)
                {
                    var file = context.Files[0];
                    string fullFilePath = Path.Combine(serverPath, file.FileName);
                    file.SaveAs(fullFilePath);
                    return new
                    {
                        success = true,
                        message = "Thành công"
                    };
                }
                else
                {
                    return new
                    {
                        success = false,
                        message = "Không có file"
                    };
                }
            }
            catch(Exception e)
            {
                return new
                {
                    success = false,
                    message = "Lỗi"
                };
            }
        }

        [HttpGet]
        public dynamic DeleteFile(string filepath)
        {
            try
            {
                string serverPath = HttpContext.Current.Server.MapPath("~" + filepath);
                if (File.Exists(serverPath))
                {
                    File.Delete(serverPath);
                }
                return new
                {
                    success = true,
                    message = "Thành công"
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }
        [HttpGet]
        public dynamic DeleteFolder(string folderpath)
        {
            try
            {
                string serverPath = HttpContext.Current.Server.MapPath("~" + folderpath);
                if (Directory.Exists(serverPath))
                {
                    Directory.Delete(serverPath);
                }
                return new
                {
                    success = true,
                    message = "Thành công"
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }
        [HttpGet]
        public dynamic CreateFolder(string folderpath)
        {
            try
            {
                string serverPath = HttpContext.Current.Server.MapPath("~" + folderpath);
                if (!Directory.Exists(serverPath))
                {
                    Directory.CreateDirectory(serverPath);
                }
                return new
                {
                    success = true,
                    message = "Thành công"
                };
            }
            catch (Exception e)
            {
                return new
                {
                    success = false,
                    message = e.Message
                };
            }
        }

        class Item
        {
            public int Type { get; set; }
            public string Name { get; set; }
            public string RelativePath { get; set; }
        }
        //string relativePath = absolutePath.Replace(HttpContext.Current.Server.MapPath("~/"), "~/").Replace(@"\", "/");
    }
}
