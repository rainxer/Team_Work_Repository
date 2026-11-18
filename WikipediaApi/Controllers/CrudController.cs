using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikipediaApi.Data;

namespace WikipediaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class CrudController<TEntity> : ControllerBase
    where TEntity : class
{
    protected readonly WikipediaContext Db;
    protected readonly DbSet<TEntity> Table;

    protected CrudController(WikipediaContext db, DbSet<TEntity> table)
    {
        Db = db;
        Table = table;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await Table.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var item = await Table.FindAsync(id);

        if (item == null)
            return NotFound();
        return Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(TEntity item)
    {
        Table.Add(item);
        await Db.SaveChangesAsync();

        return Ok(item);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, TEntity item)
    {
        var existing = await Table.FindAsync(id);

        if (existing == null)
            return NotFound();

        var entry = Db.Entry(existing);

        foreach (var property in entry.Properties)
        {
            if (property.Metadata.IsPrimaryKey())
                continue;

            var incoming = Db.Entry(item)
                .Property(property.Metadata.Name).CurrentValue;

            property.CurrentValue = incoming;
        }

        await Db.SaveChangesAsync();

        return Ok(existing);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var item = await Table.FindAsync(id);

        if (item == null)
            return NotFound();

        Table.Remove(item);
        await Db.SaveChangesAsync();

        return NoContent();
    }
}