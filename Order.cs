using System;
using System.Collections.Generic;

namespace Cashier;

public class Order
{
    public string OrderId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<Product> Products { get; set; }
    public double Total { get; set; }
    public string EmployeeName { get; set; }
    
    // Transaction properties for expenses/cash-out
    public bool IsTransaction { get; set; }
    public string TransactionType { get; set; } // e.g., "Fees", "Donation", "Utilities", "Salary"
    public double TransactionAmount { get; set; } // Negative for expenses
    
    // Helper property to get the display amount
    public double DisplayAmount => IsTransaction ? TransactionAmount : Total;
    
    // Helper property to check if it's an expense
    public bool IsExpense => IsTransaction && TransactionAmount < 0;
}
