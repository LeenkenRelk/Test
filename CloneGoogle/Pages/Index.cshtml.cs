using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
namespace CloneGoogle.Pages;

using CloneGoogle.Data;
using CloneGoogle.Models;
using System.Threading.Tasks;

public class IndexModel : PageModel
{
    
    private readonly ILogger<IndexModel> _logger;
    private readonly ApplicationDbContext _context;
    public IndexModel(ILogger<IndexModel> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
        Changes_Urls = new ChangesUrl();
        
    }

    public List<ChangesUrl> ChangesUrls { get; set; }
    public ChangesUrl Changes_Urls { get; set; }
    public void OnGet(int id)
    {
        ChangesUrls = _context.ChangesUrls.AsNoTracking().ToList();
    }

    public async Task<IActionResult> OnGetRedirect(int id)
    {
        var urlEntry = await _context.ChangesUrls.FindAsync(id);
        if (urlEntry == null)
            return NotFound();

        // Увеличиваем счётчик кликов
        urlEntry.ClickUrl++;
        await _context.SaveChangesAsync();

        // Редирект на внешний URL
        return Redirect(urlEntry.OriginUrl);
    }
    public IActionResult OnPost()
    {
        if (!ModelState.IsValid)
        {
            return Page();
        }

        // Генерация короткого кода
        string GenerateShortCode(int length)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var sb = new StringBuilder(length);
            for (int i = 0; i < length; i++)
                sb.Append(alphabet[random.Next(alphabet.Length)]);
            return sb.ToString();
        }


        (string OriginUrl, string ShortUrl) CreateShortLink(string originUrl)
        {
            string shortCode = GenerateShortCode(6);
            return (originUrl, shortCode);
        }

        // Получение оригинального URL
        string originUrl = Request.Form["Changes_Urls.OriginUrl"];
        if (originUrl == null)
        {
            ModelState.AddModelError("OriginUrl", "URL не может быть пустым.");
            return Page();
        }
        else
        {

            var (url, shortCode) = CreateShortLink(originUrl);


            Changes_Urls.OriginUrl = url;
            Changes_Urls.ShortUrl = shortCode;
            Changes_Urls.CreateUrlTime = DateTime.Now;
            Changes_Urls.ClickUrl = 0;


            _context.ChangesUrls.Add(Changes_Urls);
            _context.SaveChanges();

            return Page();
        }
    }

}
