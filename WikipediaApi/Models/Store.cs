using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WikipediaApi.Models;

public partial class Store
{
    public int StoreId { get; set; }

    public string Name { get; set; } = null!;

    public string? Website { get; set; }

    [JsonIgnore]
    public virtual ICollection<Price> Prices { get; set; } = new List<Price>();
}