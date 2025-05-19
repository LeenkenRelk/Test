using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
namespace CloneGoogle.Pages;

using CloneGoogle.Data;
using CloneGoogle.Models;


public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;
    public IndexModel(ILogger<IndexModel> logger , ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        Changes_Urls = new ChangesUrl();
    }




    public void OnGet()
    {

    }

    public ChangesUrl Changes_Urls { get; set; }
    
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        Changes_Urls.OriginUrl = Request.Form["Changes_Urls.OriginUrl"];

        Changes_Urls.CreateUrlTime = DateTime.Now;

        Changes_Urls.ClickUrl = 0;
        
        _context.ChangesUrls.Add(Changes_Urls);
        _context.SaveChanges();

        return Page();
    }
}