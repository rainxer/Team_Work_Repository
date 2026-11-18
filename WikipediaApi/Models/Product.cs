using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WikipediaApi.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    public int? ManufacturerId { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    [JsonIgnore]
    public virtual Category? Category { get; set; }

    [JsonIgnore]
    public virtual ICollection<Favorite> Favorites { get; set; } = new List<Favorite>();

    [JsonIgnore]
    public virtual Manufacturer? Manufacturer { get; set; }

    [JsonIgnore]
    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();

    [JsonIgnore]
    public virtual ICollection<ProductCharacteristic> ProductCharacteristics { get; set; } = new List<ProductCharacteristic>();

    [JsonIgnore]
    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();
}