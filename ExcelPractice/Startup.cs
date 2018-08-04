using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ExcelPractice.Startup))]
namespace ExcelPractice
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
