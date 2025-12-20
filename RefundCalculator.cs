using System.Collections.Generic;
using System.Linq;
using Cashier;

using System.Collections.Generic;
using System.Linq;

public static class RefundCalculator
{
    // GetProductKey and AggregateProducts methods remain exactly the same as before.
    // They are essential for correct identification and aggregation.
    private static string GetProductKey(Product product)
    {
        if (product == null) return null;
        if (!string.IsNullOrEmpty(product.ProudctId))
        {
            return $"ID:{product.ProudctId}|NAME:{product.ProudctName}";
        }
        if (!string.IsNullOrEmpty(product.ProudctName))
        {
            return $"NAME:{product.ProudctName}";
        }
        return null;
    }

    private static IEnumerable<Product> AggregateProducts(IEnumerable<Product> products)
    {
        return products
            .GroupBy(GetProductKey)
            .Where(g => g.Key != null)
            .Select(g =>
            {
                var representativeProduct = g.First();
                double totalQuantity = g.Sum(p => double.TryParse(p.quant, out double q) ? q : 0);
                return new Product
                {
                    ProudctId = representativeProduct.ProudctId,
                    ProudctName = representativeProduct.ProudctName,
                    ProudctPrice = representativeProduct.ProudctPrice,
                    // Copy other necessary properties...
                    quant = totalQuantity.ToString()
                };
            });
    }

    /// <summary>
    /// Compares an old and current list of products to find all changes in quantity.
    /// - Returns positive quantity for reductions/removals (refunds).
    /// - Returns negative quantity for increases/additions (up-sells).
    /// - Items with no change are excluded from the list.
    /// </summary>
    public static List<Product> GetOrderChanges(List<Product> oldProducts, List<Product> currentProducts)
    {
        var changes = new List<Product>();

        var currentProductsDict = AggregateProducts(currentProducts)
                                      .ToDictionary(p => GetProductKey(p), p => p);
        
        var oldProductsDict = AggregateProducts(oldProducts)
                                  .ToDictionary(p => GetProductKey(p), p => p);

        // Step 1: Process items that were in the old order to find reductions, increases, and removals.
        foreach (var oldEntry in oldProductsDict)
        {
            var productKey = oldEntry.Key;
            var oldProduct = oldEntry.Value;

            if (currentProductsDict.TryGetValue(productKey, out var currentProduct))
            {
                // CASE 1: The product exists in both lists. Calculate the difference.
                double.TryParse(oldProduct.quant, out double oldQuantity);
                double.TryParse(currentProduct.quant, out double currentQuantity);

                double quantityDifference = oldQuantity - currentQuantity;

                // Only add an entry if there was an actual change.
                if (quantityDifference != 0)
                {
                    var changeProduct = new Product
                    {
                        ProudctId = oldProduct.ProudctId,
                        ProudctName = oldProduct.ProudctName,
                        ProudctPrice = oldProduct.ProudctPrice,
                        // ... copy other properties ...
                        quant = quantityDifference.ToString() // Will be positive for reduction, negative for increase.
                    };
                    changes.Add(changeProduct);
                }
            }
            else
            {
                // CASE 2: The product was in the old order but not the new one. It was fully removed.
                // The change is its full original quantity (positive).
                changes.Add(oldProduct);
            }
        }

        // Step 2: Process items in the new order to find brand new additions.
        foreach (var currentEntry in currentProductsDict)
        {
            var productKey = currentEntry.Key;
            var currentProduct = currentEntry.Value;

            // If an item from the new list does NOT exist in the old list, it's a new addition.
            if (!oldProductsDict.ContainsKey(productKey))
            {
                double.TryParse(currentProduct.quant, out double currentQuantity);

                var newProductChange = new Product
                {
                    ProudctId = currentProduct.ProudctId,
                    ProudctName = currentProduct.ProudctName,
                    ProudctPrice = currentProduct.ProudctPrice,
                    // ... copy other properties ...
                    quant = (-currentQuantity).ToString() // The change is negative for a new addition.
                };
                changes.Add(newProductChange);
            }
        }

        return changes;
    }
}