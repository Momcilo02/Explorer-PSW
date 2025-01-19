using Explorer.BuildingBlocks.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Shopping.Core.Domain;

public enum BundleStatus
{
    Draft = 0,
    Published = 1,
    Archived = 2,
}
public class Bundle: Entity
{
    public string Name { get; init; }

    public double Price { get; init; }
    public BundleStatus Status { get; init; }

    public List<Product> Products { get; set; }
    public long CreatorId { get; init; }
    public Bundle(string name, double price, BundleStatus status, long creatorId)
    {
        Name = name;
        Price = price;
        Status = status;
        CreatorId = creatorId;
    }
}
