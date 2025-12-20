using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ATARAXIA;

namespace Cashier;

public partial class ExpensePage : ContentPage
{
    private ObservableCollection<Order> _recentExpenses;

    public ExpensePage()
    {
        InitializeComponent();
        LoadRecentExpenses();
    }

    private void LoadRecentExpenses()
    {
        // Load all orders
        var allOrders = HandleData.GetElement<List<Order>>.GetItem("OrderHistory") ?? new List<Order>();
        
        // Filter only transactions (expenses) and get last 10
        _recentExpenses = new ObservableCollection<Order>(
            allOrders
                .Where(o => o.IsTransaction && o.TransactionAmount < 0)
                .OrderByDescending(o => o.OrderDate)
                
                .ToList()
        );
        
        RecentExpensesCollection.ItemsSource = _recentExpenses;
    }

    private async void OnSaveExpenseClicked(object sender, EventArgs e)
    {
        // Validate inputs
        if (TransactionTypePicker.SelectedIndex == -1)
        {
            ShowError("الرجاء اختيار نوع المنصرف");
            return;
        }

        if (string.IsNullOrWhiteSpace(AmountEntry.Text))
        {
            ShowError("الرجاء إدخال المبلغ");
            return;
        }

        if (!double.TryParse(AmountEntry.Text, out double amount) || amount <= 0)
        {
            ShowError("الرجاء إدخال مبلغ صحيح");
            return;
        }

        if (string.IsNullOrWhiteSpace(EmployeeNameEntry.Text))
        {
            ShowError("الرجاء إدخال اسم الموظف");
            return;
        }

        try
        {
            // Create the expense transaction
            var expense = new Order
            {
                OrderId = $"EXP-{DateTime.Now:yyyyMMddHHmmss}",
                OrderDate = DateTime.Now,
                IsTransaction = true,
                TransactionType = TransactionTypePicker.SelectedItem.ToString(),
                TransactionAmount = -amount, // Negative for expense
                EmployeeName = EmployeeNameEntry.Text.Trim(),
                Products = new List<Product>(), // Empty for transactions
                Total = 0
            };

            // Add notes if provided
            if (!string.IsNullOrWhiteSpace(NotesEditor.Text))
            {
                // Append notes to TransactionType
                expense.TransactionType += $" - {NotesEditor.Text.Trim()}";
            }

            // Load existing orders
            var allOrders = HandleData.GetElement<List<Order>>.GetItem("OrderHistory") ?? new List<Order>();
            
            // Add new expense
            allOrders.Add(expense);

            // Save back to HandleData
            HandleData.AddItemToStorage("OrderHistory", allOrders);

            // Show success message
            await DisplayAlert("نجح", $"تم تسجيل المنصرف بمبلغ ${amount:N2}", "حسناً");

            // Clear form
            ClearForm();
            
            // Reload recent expenses
            LoadRecentExpenses();
        }
        catch (Exception ex)
        {
            await DisplayAlert("خطأ", $"حدث خطأ أثناء حفظ المنصرف: {ex.Message}", "حسناً");
        }
    }

    private void OnCancelClicked(object sender, EventArgs e)
    {
        ClearForm();
    }

    private void ClearForm()
    {
        TransactionTypePicker.SelectedIndex = -1;
        AmountEntry.Text = string.Empty;
        NotesEditor.Text = string.Empty;
        EmployeeNameEntry.Text = string.Empty;
        HideError();
    }

    private void ShowError(string message)
    {
        ErrorLabel.Text = message;
        ErrorLabel.IsVisible = true;
    }

    private void HideError()
    {
        ErrorLabel.Text = string.Empty;
        ErrorLabel.IsVisible = false;
    }
}