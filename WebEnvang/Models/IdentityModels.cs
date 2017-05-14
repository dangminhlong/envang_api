using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using WebEnvang.Models.General;
using System.Data.Entity;


namespace WebEnvang.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string Address { get; set; }
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager, string authenticationType)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, authenticationType);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        
        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<GeneralInformation> GeneralInformations { get; set; }
        public DbSet<Page> Pages { get; set; }
        public DbSet<PageRole> PageRoles { get; set; }
        public DbSet<WebConfig.WebConfig> WebConfigs { get; set; }
        public DbSet<FeatureArticleConfigGroup.FeatureArticleConfigGroup> FeatureArticleConfigGroups { get; set; }
        public DbSet<Region.Region> Regions { get; set; }
        public DbSet<Location.Location> Locations { get; set; }
        public DbSet<FlightRoute.FlightRoute> FlightRoutes { get; set; }
        public DbSet<ArticleTypes.ArticleType> ArticleTypes { get; set; }
        public DbSet<Article.Article> Articles { get; set; }
        public DbSet<FeatureArticleConfig.FeatureArticleConfig> FeatureArticleConfigs { get; set; }
        public DbSet<Airline.Airline> Airlines { get; set; }
        public DbSet<LuggagePrice.LuggagePrice> LuggagePrices { get; set; }
        public DbSet<Province.Province> Provinces { get; set; }
    }
}