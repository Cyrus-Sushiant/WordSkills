using WordSkills.Shared.Dtos.Glossary;

namespace WordSkills.Client.Core.Controllers.Glossary;

[Route("api/[controller]/[action]/")]
public interface IWordController : IAppController
{
    [HttpGet("{id}")]
    Task<WordDto> Get(long id, CancellationToken cancellationToken = default);

    [HttpPost]
    Task<WordDto> Create(WordDto dto, CancellationToken cancellationToken = default);

    [HttpPut]
    Task<WordDto> Update(WordDto dto, CancellationToken cancellationToken = default);

    [HttpDelete("{id}")]
    Task Delete(long id, CancellationToken cancellationToken = default);

    [HttpGet]
    Task<List<WordDto>> Get(CancellationToken cancellationToken = default) => default!;
}
