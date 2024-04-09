using AutoMapper;
using FilmesApi.Data;
using FilmesApi.Data.Dtos;
using FilmesApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SessaoController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public SessaoController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult AdicionarSessao([FromBody] CreateSessaoDto sessaoDto)
    {
        Sessao sessao = _mapper.Map<Sessao>(sessaoDto);
        _context.Sessoes.Add(sessao);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperaSessaoPorId), new 
            { filmeId = sessao.FilmeId, cinemaId = sessao.CinemaId }, sessao);
    }

    [HttpGet]
    public IEnumerable<ReadSessaoDto> RecuperaSessao()
    {
        var listaDeSessao = _mapper.Map<List<ReadSessaoDto>>(_context.Sessoes.ToList());
        return listaDeSessao;
    }

    [HttpGet("{filmeId}/{cinemaId}")]
    public IActionResult RecuperaSessaoPorId(int filmeId, int cinemaId)
    {
        Sessao sessao = _context.Sessoes.FirstOrDefault(sessao => 
            sessao.FilmeId == filmeId && sessao.CinemaId == cinemaId);

        if (sessao != null)
        {
            ReadSessaoDto sessaoDto = _mapper.Map<ReadSessaoDto>(sessao);
            return Ok(sessaoDto);
        }
        return NotFound();
    }

    [HttpPut("{id}")]
    public IActionResult AtualizaSessao(int filmeId, [FromBody] UpdateSessaoDto sessaoDto)
    {
        Sessao sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId);
        if (sessaoDto == null)
        {
            return NotFound();
        }
        _mapper.Map(sessaoDto, sessao);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeletaSessao(int filmeId)
    {
        Sessao sessao = _context.Sessoes.FirstOrDefault(sessao => sessao.FilmeId == filmeId);
        if (sessao == null)
        {
            return NotFound();
        }
        _context.Remove(sessao);
        _context.SaveChanges();
        return NoContent();
    }
}
