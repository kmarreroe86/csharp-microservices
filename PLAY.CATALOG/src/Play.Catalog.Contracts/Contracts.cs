using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Play.Catalog.Contracts
{
    public record CatalogItemCreated(Guid itemId, string name, string description);
    
    public record CatalogItemUpdated(Guid itemId, string name, string description);

    public record CatalogItemDeleted(Guid itemId);
    
}
