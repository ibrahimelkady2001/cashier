using System;

namespace Cashier;

public class Product
{
        public string? OrderUnique{ get; set; }
    public string? ProudctId { get; set; }
    public string _ProductName = string.Empty;
    public string ProudctName
    {
        get
        {
            return _ProductName;
        }
        set
        {
            _ProductName = value;
        }
    }
    public double SellCount { get; set; }
public ProductCategory ProudctCategory { get; set; }
    public double ProudctPrice { get; set; }
    public double RealPrice { get; set; }

    public double GomlaPrice { get; set; }
    public string? SupplierName { get; set; }
    public double Quantity { get; set; }

    public int SubProductCount { get; set; }
    public string? SubProducttype { get; set; }
    public Product? SubProduct { get; set; }
// used for Order Model this quantity for product in order
    public string _quant = "1";
    public string quant
    {
        get
        {
            return _quant;
        }
        set
        {
            _quant = value;
        }
    }
}

public enum ProductCategory

{
    غير_محدد

   , رجالي,
    حريمي,
    مكتبة,
    اقمشة,
    شنط


}
