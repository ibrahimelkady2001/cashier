namespace Cashier;

public partial class OrderHistoryPage : ContentPage
{
    private readonly OrderHistoryViewModel _viewModel;

    public OrderHistoryPage()
    {
        InitializeComponent();
        _viewModel = new OrderHistoryViewModel();
        BindingContext = _viewModel;
    }

    private void OnSearchTextChanged(object sender, TextChangedEventArgs e)
    {
        _viewModel.SearchText = e.NewTextValue;
        _viewModel.FilterOrders();
    }

    private void OnSearchButtonPressed(object sender, EventArgs e)
    {
        _viewModel.FilterOrders();
    }

    private async void OnOrderTapped(object sender, TappedEventArgs e)
    {
        if (e.Parameter is Order order)
        {
            // Show order details
            await ShowOrderDetails(order);
        }
    }

    private async Task ShowOrderDetails(Order order)
    {
        var products = string.Join("\n", order.Products.Select(p => $"• {p.ProudctName} x{p.Quantity} - ${p.ProudctPrice:N2}"));
        
        var message = $"Order ID: {order.OrderId}\n" +
                     $"Date: {order.OrderDate:MMM dd, yyyy hh:mm tt}\n" +
                     $"Employee: {order.EmployeeName}\n" +
                     $"\nProducts:\n{products}\n" +
                     $"\nTotal: ${order.Total:N2}";

        await DisplayAlert("Order Details", message, "OK");
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // Refresh data when page appears
        _viewModel.LoadOrders();
    }
}
