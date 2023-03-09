using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AtmAspRazorApp.Pages
{
    public class IndexModel : PageModel
    {
        public string ? day;
        public void OnGet()
        {
            ViewData["Title"] = "Login";
            day = DateTime.Now.ToString("D");
        }
    }
}
