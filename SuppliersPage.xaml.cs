using System.Diagnostics;
using System.Threading.Tasks;
using ATARAXIA;

namespace Cashier;

public partial class SuppliersPage : ContentPage
{
	public SuppliersPage()
	{
		InitializeComponent();
	}
    protected override void OnAppearing()
    {
	var list=	HandleData.GetElement<List<Supplier>>.GetItem("Suppliers");
		SuppliersListView.ItemsSource = list;
        base.OnAppearing();
    }
			private async void Back_Clicked(object sender, EventArgs e)
	{
		try{
await Navigation.PopAsync();
		}
		catch(Exception ex){

		}
	}

	private void SearchBar_TextChanged(System.Object sender, Microsoft.Maui.Controls.TextChangedEventArgs e)
	{
		if (string.IsNullOrEmpty(e.NewTextValue))
		{
			var list = HandleData.GetElement<List<Supplier>>.GetItem("Suppliers");
			SuppliersListView.ItemsSource = list;
		}
		else
		{
			var list = HandleData.GetElement<List<Supplier>>.GetItem("Suppliers");
			SuppliersListView.ItemsSource = list.Where(p => p.Name.Contains(e.NewTextValue));
		}
	}
	private async void ListView_ItemTapped(System.Object sender, Microsoft.Maui.Controls.ItemTappedEventArgs e)
	{
		if (e.Item is Supplier supplier)
		{
		await	Navigation.PushAsync(new SupplierPage(supplier));
		}
	 }
	private async void Add_Clicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PushAsync(new SupplierPage(null));
	 }
}