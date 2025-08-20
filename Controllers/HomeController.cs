using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GerenciadorHotel.Models;
using GerenciadorHotel.Data;

namespace GerenciadorHotel.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        var acomodacoes = _context.Acomodacoes
            .Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel)
            .Include(a => a.Imagens)
            .OrderBy(a => a.Preco)
            .Take(3)
            .ToList();

        ViewBag.TiposAcomodacao = _context.Acomodacoes
            .Select(a => a.Nome)
            .Distinct()
            .ToList();

        return View(acomodacoes);
    }

    [Authorize]
    public IActionResult Dashboard()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
