using System.Threading.Tasks;
using ATARAXIA;

namespace Cashier;

public partial class SupplierPage : ContentPage
{
	Supplier CurrentSupplier = null;
	public SupplierPage(Supplier supplier)
	{
		CurrentSupplier = supplier;
		InitializeComponent();
		if (supplier != null)
		{

			addproductbut.Text = "تعديل المورد";
			delproductbor.IsVisible = true;
			var products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
			ProductsListView.ItemsSource = products.Where(p => p.SupplierName == supplier.Name).ToList() ?? new List<Product>(); ;
			SupplierNameEntry.Text = supplier.Name ?? string.Empty;
			AddressEntry.Text = supplier.Address ?? string.Empty;
			PhoneEntry.Text = supplier.Phone ?? string.Empty;
			Moneydate.Date = supplier.MoneyDate;
			MoneyEntry.Text = supplier.Money ?? string.Empty;
		}
	}
		private async void ProductsListView_ItemTapped(object sender, ItemTappedEventArgs e)
	{
			await Navigation.PushAsync(new AddProduct(e.Item as Product));
    }
	private async void Del_Clicked(System.Object sender, System.EventArgs e)
	{
		var suppliers = HandleData.GetElement<List<Supplier>>.GetItem("Suppliers") ?? new List<Supplier>();
		var newlist = suppliers.Where(p => p.Name != CurrentSupplier.Name).ToList();
		HandleData.AddItemToStorage("Suppliers", newlist);
		await Navigation.PopAsync();
	}
	private async void Add_Or_Modify_Clicked(System.Object sender, System.EventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(async() =>
		{


			try
			{
				if (CurrentSupplier == null)
				{
					var suppliers = HandleData.GetElement<List<Supplier>>.GetItem("Suppliers") ?? new List<Supplier>();
					suppliers.Add(new Supplier()
					{
						Name = SupplierNameEntry.Text,
						Address = AddressEntry.Text,
						Phone = PhoneEntry.Text,
						MoneyDate = Moneydate.Date,
						Money = MoneyEntry.Text
					});
					HandleData.AddItemToStorage("Suppliers", suppliers);
				}
				else
				{
					var suppliers = HandleData.GetElement<List<Supplier>>.GetItem("Suppliers") ?? new List<Supplier>();
					foreach (var item in suppliers)
					{
						if (item.Name == CurrentSupplier.Name)
						{
							// item.Name = SupplierNameEntry.Text;
							item.Address = AddressEntry.Text;
							item.Phone = PhoneEntry.Text;
							item.MoneyDate = Moneydate.Date;
							item.Money = MoneyEntry.Text;
						}
					}
					HandleData.AddItemToStorage("Suppliers", suppliers);
				}
				await Navigation.PopAsync();
			}
			catch (System.Exception)
			{
				
			
			}
		});
	}
	 		private async void Back_Clicked(object sender, EventArgs e)
	{
		try{
await Navigation.PopAsync();
		}
		catch(Exception ex){

		}
	}

}