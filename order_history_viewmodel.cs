using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using ATARAXIA;

namespace Cashier;

public class OrderHistoryViewModel : INotifyPropertyChanged
{
    private string _searchText;
    private List<Order> _allOrders;
    private ObservableCollection<Order> _filteredOrders;
    private string _resultsText;

    public OrderHistoryViewModel()
    {
        _searchText = string.Empty;
        _filteredOrders = new ObservableCollection<Order>();
        LoadOrders();
    }

    public string SearchText
    {
        get => _searchText;
        set
        {
            if (_searchText != value)
            {
                _searchText = value;
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

    public string ResultsText
    {
        get => _resultsText;
        set
        {
            if (_resultsText != value)
            {
                _resultsText = value;
                OnPropertyChanged();
            }
        }
    }

    public void LoadOrders()
    {
        // Load orders from HandleData
        _allOrders = HandleData.GetElement<List<Order>>.GetItem("OrderHistory") ?? new List<Order>();
        
        // Sort by date descending (newest first)
        _allOrders = _allOrders.OrderByDescending(o => o.OrderDate).ToList();
        
        FilterOrders();
    }

    public void FilterOrders()
    {
        IEnumerable<Order> filtered = _allOrders;

        // Apply search filter if search text is not empty
        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLower().Trim();
            
            filtered = _allOrders.Where(o =>
                // Search by Order ID
                o.OrderId?.ToLower().Contains(searchLower) == true ||
                
                // Search by Employee Name
                o.EmployeeName?.ToLower().Contains(searchLower) == true ||
                
                // Search by Total Amount (convert to string)
                o.Total.ToString("N2").Contains(searchLower) ||
                o.Total.ToString().Contains(searchLower) ||
                
                // Search by Date
                o.OrderDate.ToString("MMM dd, yyyy").ToLower().Contains(searchLower) ||
                o.OrderDate.ToString("MM/dd/yyyy").Contains(searchLower) ||
                
                // Search by Product Names
                (o.Products?.Any(p => p.ProudctName?.ToLower().Contains(searchLower) == true) == true)
            );
        }

        var filteredList = filtered.ToList();

        // Update filtered collection
        FilteredOrders.Clear();
        foreach (var order in filteredList)
        {
            FilteredOrders.Add(order);
        }

        // Update results text
        if (string.IsNullOrWhiteSpace(SearchText))
        {
            ResultsText = $"Showing all {filteredList.Count} order(s)";
        }
        else
        {
            ResultsText = $"Found {filteredList.Count} order(s) matching '{SearchText}'";
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
