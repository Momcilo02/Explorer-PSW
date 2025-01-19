using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.API.Dtos;

public enum ItemType
{
    Tour = 0,
    Bundle = 1
}
public class ItemDto
{
    public long SellerId { get; set; }
    public long ItemId { get; set; }
    public string Name { get; set; } = string.Empty;
    public double Price { get; set; }
    public ItemType Type { get; set; }
}
