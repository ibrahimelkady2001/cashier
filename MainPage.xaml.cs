using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using ATARAXIA;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Cashier;

public partial class MainPage : ContentPage
{


    public static Label OrdersCountlab = null;
    
  public MainPage()
    {
        InitializeComponent();
        OrdersCountlab = OrdersCount;
        OrdersCount.Text = "عدد الطلبات: " + App.CurrentOrders.Count() ;
    }
    protected override void OnAppearing()
    {
                OrdersCount.Text = "عدد الطلبات: " + App.CurrentOrders.Count() ;
        base.OnAppearing();
    }

   private async void Employees_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        await Navigation.PushAsync(new EmployeesListPage());
    }
   

     private async void Expanses_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        await Navigation.PushAsync(new  ExpensePage());
    }
       private async void Transactions_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        await Navigation.PushAsync(new TransactionSummaryPage());
    }
    private async void Inventory_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
        await Navigation.PushAsync(new InventoryPage());
    }

    private async void Orders_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrdersPage());
    }
 private async void OrdersHistory_Tapped(object sender, TappedEventArgs e)
    {
        await Navigation.PushAsync(new OrdersPage(true));
    }

    private async void Suppliers_TouchGestureCompleted(object sender, CommunityToolkit.Maui.Core.TouchGestureCompletedEventArgs e)
    {
      await  Navigation.PushAsync(new SuppliersPage());
    }
    
}