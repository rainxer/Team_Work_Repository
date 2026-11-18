using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WikipediaApi.Data;
using WikipediaApi.Models;

namespace WikipediaApi.Controllers;

[ApiController]
[Route("api/product-characteristics")]
public class ProductCharacteristicsController : ControllerBase
{
    private readonly WikipediaContext _db;

    public ProductCharacteristicsController(WikipediaContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _db.ProductCharacteristics.ToListAsync());
    }

    [HttpGet("{productId:int}/{characteristicId:int}")]
    public async Task<IActionResult> Get(int productId, int characteristicId)
    {
        var item = await _db.ProductCharacteristics.FindAsync(
            productId, characteristicId);

        return item == null ? NotFound() : Ok(item);
    }

    [HttpPost]
    public async Task<IActionResult> Create(ProductCharacteristic item)
    {
        _db.ProductCharacteristics.Add(item);
        await _db.SaveChangesAsync();
        return Ok(item);
    }

    [HttpPut("{productId:int}/{characteristicId:int}")]
    public async Task<IActionResult> Update(
        int productId, int characteristicId, ProductCharacteristic item)
    {
        var existing = await _db.ProductCharacteristics.FindAsync(
            productId, characteristicId);

        if (existing == null)
            return NotFound();

        existing.Value = item.Value;

        await _db.SaveChangesAsync();
        return Ok(existing);
    }

    [HttpDelete("{productId:int}/{characteristicId:int}")]
    public async Task<IActionResult> Delete(
        int productId, int characteristicId)
    {
        var item = await _db.ProductCharacteristics.FindAsync(
            productId, characteristicId);

        if (item == null)
            return NotFound();

        _db.ProductCharacteristics.Remove(item);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}