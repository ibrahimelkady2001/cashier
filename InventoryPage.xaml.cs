using System.Threading.Tasks;
using ATARAXIA;

namespace Cashier;

public partial class InventoryPage : ContentPage
{
	public InventoryPage()
	{
		InitializeComponent();
		ProductsListView.RefreshCommand = new Command(() =>
		{
			ProductsListView.ItemsSource = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
ProductsListView.IsRefreshing = false;
		});
	}
    protected override void OnAppearing()
    {
			ProductsListView.ItemsSource = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
        base.OnAppearing();
    }

    private async void AddProduct_Clicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PushAsync(new AddProduct());
	}

	private async void ProductsListView_ItemTapped(object sender, ItemTappedEventArgs e)
	{
			await Navigation.PushAsync(new AddProduct(e.Item as Product));
    }
}