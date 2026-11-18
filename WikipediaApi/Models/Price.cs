using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WikipediaApi.Models;

public partial class Price
{
    public int PriceId { get; set; }

    public int ProductId { get; set; }

    public int StoreId { get; set; }

    public decimal Price1 { get; set; }

    public string? ProductUrl { get; set; }

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;

    [JsonIgnore]
    public virtual Store Store { get; set; } = null!;
}