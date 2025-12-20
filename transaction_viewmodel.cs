using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using ATARAXIA;

namespace Cashier;

public class TransactionViewModel : INotifyPropertyChanged
{
    private DateTime _startDate;
    private DateTime _endDate;
    private double _totalAmount;
    private double _totalSales;
    private double _totalExpenses;
    private int _orderCount;
    private int _expenseCount;
    private List<Order> _allOrders;
    private ObservableCollection<Order> _filteredOrders;

    public TransactionViewModel()
    {
        // Load orders from HandleData
        _allOrders = HandleData.GetElement<List<Order>>.GetItem("OrderHistory") ?? new List<Order>();
        _filteredOrders = new ObservableCollection<Order>();
        
        // Set default to today
        _startDate = DateTime.Today;
        _endDate = DateTime.Today;
        
        FilterOrders();
    }

    public DateTime StartDate
    {
        get => _startDate;
        set
        {
            if (_startDate != value)
            {
                _startDate = value;
                OnPropertyChanged();
            }
        }
    }

    public DateTime EndDate
    {
        get => _endDate;
        set
        {
            if (_endDate != value)
            {
                _endDate = value;
                OnPropertyChanged();
            }
        }
    }

    public double TotalAmount
    {
        get => _totalAmount;
        set
        {
            if (_totalAmount != value)
            {
                _totalAmount = value;
                OnPropertyChanged();
            }
        }
    }

    public double TotalSales
    {
        get => _totalSales;
        set
        {
            if (_totalSales != value)
            {
                _totalSales = value;
                OnPropertyChanged();
            }
        }
    }

    public double TotalExpenses
    {
        get => _totalExpenses;
        set
        {
            if (_totalExpenses != value)
            {
                _totalExpenses = value;
                OnPropertyChanged();
            }
        }
    }

    public int OrderCount
    {
        get => _orderCount;
        set
        {
            if (_orderCount != value)
            {
                _orderCount = value;
                OnPropertyChanged();
            }
        }
    }

    public int ExpenseCount
    {
        get => _expenseCount;
        set
        {
            if (_expenseCount != value)
            {
                _expenseCount = value;
                OnPropertyChanged();
            }
        }
    }

    public ObservableCollection<Order> FilteredOrders
    {
        get => _filteredOrders;
        set
        {
            if (_filteredOrders != value)
            {
                _filteredOrders = value;
                OnPropertyChanged();
            }
        }
    }

    public void SetToday()
    {
        StartDate = DateTime.Today;
        EndDate = DateTime.Today;
        FilterOrders();
    }

    public void SetLast30Days()
    {
        EndDate = DateTime.Today;
        StartDate = DateTime.Today.AddDays(-30);
        FilterOrders();
    }

    public void SetLastMonth()
    {
        var today = DateTime.Today;
        var firstDayOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
        var firstDayOfLastMonth = firstDayOfCurrentMonth.AddMonths(-1);
        var lastDayOfLastMonth = firstDayOfCurrentMonth.AddDays(-1);

        StartDate = firstDayOfLastMonth;
        EndDate = lastDayOfLastMonth;
        FilterOrders();
    }

    public void FilterOrders()
    {
        // Reload orders in case they were updated
        _allOrders = HandleData.GetElement<List<Order>>.GetItem("OrderHistory") ?? new List<Order>();
        
        // Filter orders by date range
        var filtered = _allOrders
            .Where(o => o.OrderDate.Date >= StartDate.Date && o.OrderDate.Date <= EndDate.Date)
            .OrderByDescending(o => o.OrderDate)
            .ToList();

        // Update filtered collection
        FilteredOrders.Clear();
        foreach (var order in filtered)
        {
            FilteredOrders.Add(order);
        }

        // Calculate totals
        var sales = filtered.Where(o => !o.IsTransaction).Sum(o => o.Total);
        var expenses = Math.Abs(filtered.Where(o => o.IsTransaction && o.TransactionAmount < 0).Sum(o => o.TransactionAmount));
        
        TotalSales = sales;
        TotalExpenses = expenses;
        TotalAmount = sales - expenses; // Net amount
        
        OrderCount = filtered.Count(o => !o.IsTransaction);
        ExpenseCount = filtered.Count(o => o.IsTransaction && o.TransactionAmount < 0);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}