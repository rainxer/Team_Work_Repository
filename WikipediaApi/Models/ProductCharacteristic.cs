using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WikipediaApi.Models;

public partial class ProductCharacteristic
{
    public int ProductId { get; set; }

    public int CharacteristicId { get; set; }

    public string Value { get; set; } = null!;

    [JsonIgnore]
    public virtual Characteristic Characteristic { get; set; } = null!;

    [JsonIgnore]
    public virtual Product Product { get; set; } = null!;
}