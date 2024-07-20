using WordSkills.Client.Core.Controllers.Glossary;
using WordSkills.Shared.Dtos.Glossary;

namespace WordSkills.Server.Controllers.Glossary;

[ApiController, Route("api/[controller]/[action]")]
public partial class WordController : AppControllerBase, IWordController
{
    [HttpGet, EnableQuery]
    public IQueryable<WordDto> Get()
    {
        var userId = User.GetUserId();

        return DbContext.Words
            .Where(t => t.UserId == userId)
            .Project();
    }

    [HttpGet]
    public async Task<PagedResult<WordDto>> GetWords(ODataQueryOptions<WordDto> odataQuery, CancellationToken cancellationToken)
    {
        var query = (IQueryable<WordDto>)odataQuery.ApplyTo(Get(), ignoreQueryOptions: AllowedQueryOptions.Top | AllowedQueryOptions.Skip);

        var totalCount = await query.LongCountAsync(cancellationToken);

        if (odataQuery.Skip is not null)
            query = query.Skip(odataQuery.Skip.Value);

        if (odataQuery.Top is not null)
            query = query.Take(odataQuery.Top.Value);

        return new PagedResult<WordDto>(await query.ToArrayAsync(cancellationToken), totalCount);
    }

    [HttpGet("{id}")]
    public async Task<WordDto> Get(long id, CancellationToken cancellationToken)
    {
        var dto = await Get().FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        if (dto is null)
            throw new ResourceNotFoundException(Localizer[nameof(AppStrings.WordCouldNotBeFound)]);

        return dto;
    }

    [HttpPost]
    public async Task<WordDto> Create(WordDto dto, CancellationToken cancellationToken)
    {
        var entityToAdd = dto.Map();

        entityToAdd.UserId = User.GetUserId();

        entityToAdd.CreatedOn = entityToAdd.ModifiedOn = DateTimeOffset.UtcNow;

        await DbContext.Words.AddAsync(entityToAdd, cancellationToken);

        await DbContext.SaveChangesAsync(cancellationToken);

        return entityToAdd.Map();
    }

    [HttpPut]
    public async Task<WordDto> Update(WordDto dto, CancellationToken cancellationToken)
    {
        var entityToUpdate = await DbContext.Words.FirstOrDefaultAsync(t => t.Id == dto.Id, cancellationToken);

        if (entityToUpdate is null)
            throw new ResourceNotFoundException(Localizer[nameof(AppStrings.WordCouldNotBeFound)]);

        dto.Patch(entityToUpdate);

        entityToUpdate.ModifiedOn = DateTimeOffset.UtcNow;

        await DbContext.SaveChangesAsync(cancellationToken);

        return entityToUpdate.Map();
    }

    [HttpDelete("{id}")]
    public async Task Delete(long id, CancellationToken cancellationToken)
    {
        DbContext.Words.Remove(new() { Id = id });

        var affectedRows = await DbContext.SaveChangesAsync(cancellationToken);

        if (affectedRows < 1)
            throw new ResourceNotFoundException(Localizer[nameof(AppStrings.WordCouldNotBeFound)]);
    }
}

