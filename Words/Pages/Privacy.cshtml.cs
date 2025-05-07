using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Words.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;
        private readonly IConfiguration _configuration;
        public string? ContactEmail { get; set; }

        public PrivacyModel(ILogger<PrivacyModel> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public void OnGet()
        {
            ContactEmail = _configuration["ContactEmail"];
        }
    }

}
