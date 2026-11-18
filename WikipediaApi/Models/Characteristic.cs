using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WikipediaApi.Models;

public partial class Characteristic
{
    public int CharacteristicId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<ProductCharacteristic> ProductCharacteristics { get; set; } = new List<ProductCharacteristic>();
}