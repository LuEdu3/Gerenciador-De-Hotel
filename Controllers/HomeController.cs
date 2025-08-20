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

    [AllowAnonymous]
    public IActionResult Acomodacao(string? TipoAcomodacao, string? FaixaPreco, string? Capacidade, int page = 1)
    {
        var query = _context.Acomodacoes
            .Where(a => a.Ativa && a.Status == StatusAcomodacao.Disponivel)
            .Include(a => a.Imagens)
            .AsQueryable();

        // Aplica filtros
        if (!string.IsNullOrEmpty(TipoAcomodacao))
        {
            query = query.Where(a => a.Nome == TipoAcomodacao);
            ViewBag.FiltroTipo = TipoAcomodacao;
        }

        if (!string.IsNullOrEmpty(FaixaPreco))
        {
            var precos = FaixaPreco.Split('-').Select(decimal.Parse).ToList();
            query = query.Where(a => a.Preco >= precos[0] && a.Preco <= precos[1]);
            ViewBag.FiltroPreco = FaixaPreco;
        }

        if (!string.IsNullOrEmpty(Capacidade))
        {
            if (Capacidade.Contains("-"))
            {
                var capacidades = Capacidade.Split('-').Select(int.Parse).ToList();
                query = query.Where(a => a.QuantidadeCamas >= capacidades[0] && a.QuantidadeCamas <= capacidades[1]);
            }
            else
            {
                var capacidade = int.Parse(Capacidade);
                query = query.Where(a => a.QuantidadeCamas >= capacidade);
            }
            ViewBag.FiltroCapacidade = Capacidade;
        }

        // Paginação
        const int pageSize = 12;
        int totalItems = query.Count();
        int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var acomodacoes = query
            .OrderBy(a => a.Preco)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToList();

        ViewBag.CurrentPage = page;
        ViewBag.TotalPages = totalPages;

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
