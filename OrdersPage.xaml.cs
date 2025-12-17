using System.Threading.Tasks;
using ATARAXIA;

namespace Cashier;

public partial class OrdersPage : ContentPage
{
	bool IsHistory = false;
	public OrdersPage(bool IsHistoryPage = false)
	{
		InitializeComponent();
		IsHistory = IsHistoryPage;
		if (IsHistoryPage)
		{
			var src = (HandleData.GetElement<List<Order>>.GetItem("OrderHistory") ?? new List<Order>());
			src.Reverse();
			OrderListView.ItemsSource = src;
		}
		else
		{
			OrderListView.ItemsSource = App.CurrentOrders;
		}
	}
	private async void ListView_ItemSelected(System.Object sender, Microsoft.Maui.Controls.SelectedItemChangedEventArgs e)
	{
		var order = e.SelectedItem as Order;
	await Navigation.PushAsync(new OrderDetailPage(order, IsHistory));
	 }
}