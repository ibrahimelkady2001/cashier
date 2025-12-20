using System.Diagnostics;
using System.Globalization;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using ATARAXIA;

namespace Cashier;

public partial class InventoryPage : ContentPage
{
	public static bool Refresh = true;
	List<Product> products = new List<Product>();
	public InventoryPage()
	{
		InitializeComponent();
		orderpicker.ItemsSource= new string[] { "ابجدي","المورد","الكمية","الاكثر مبيعا" ,"النوع","بدون باركود", };
	var names =	Enum.GetNames(typeof(ProductCategory));
		var list =new string[] { "الكل" }.ToList();
	var newlist =	list.Concat(names.AsEnumerable()).ToList();
		catgoryPicker.ItemsSource = newlist;
		orderpicker.SelectedIndex = 0;

		ProductsListView.RefreshCommand = new Command(() =>
		{

			if (orderpicker.SelectedIndex == 0)
			{
				SortAlpahbitic();
			}
			else if (orderpicker.SelectedIndex == 1)
			{
				SortSupplier();
			}
			else if (orderpicker.SelectedIndex == 2)
			{
				SortQuantity();
			}
			else if (orderpicker.SelectedIndex == 3)
			{
				SortMostSelling();
			}
			else if (orderpicker.SelectedIndex == 4)
			{
				SortType();
			}
			else if (orderpicker.SelectedIndex == 5)
			{
				SortemptyBarcode();
		}
			ProductsListView.IsRefreshing = false;
		});
	}
    protected override void OnAppearing()
    {
		if (Refresh)
		{




			if (orderpicker.SelectedIndex == 0)
			{
				SortAlpahbitic();
			}
			else if (orderpicker.SelectedIndex == 1)
			{
				SortSupplier();
			}
			else if (orderpicker.SelectedIndex == 2)
			{
				SortQuantity();
			}
			else if (orderpicker.SelectedIndex == 3)
			{
				SortMostSelling();
			}
			else if (orderpicker.SelectedIndex == 4)
			{
				SortType();

			}
					else if (orderpicker.SelectedIndex == 5)
			{
				SortemptyBarcode();
		}
			
		
		}
		else
		{
			Refresh = true;
		}
        base.OnAppearing();
    }
	public void SortAlpahbitic()
	{
		var culture = new CultureInfo("ar");


		products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
		products = products
.OrderBy(p => p.ProudctName, StringComparer.Create(culture, ignoreCase: false))
.ToList();
		MainThread.BeginInvokeOnMainThread(() =>
				{
					ProductsListView.ItemsSource = products;
				});
				Search(searchbar.Text);
	}
	public void SortSupplier()
	{


		// Load suppliers and products
		var suppliers = HandleData.GetElement<List<Supplier>>.GetItem("Suppliers") ?? new List<Supplier>();
		products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();

		// Join products with suppliers and sort by supplier name
		products = products
			.OrderBy(p =>
			{
				var supplier = suppliers.FirstOrDefault(s => s.Name == p.SupplierName);
				return supplier?.Name ?? string.Empty;
			})
			.ToList();
		MainThread.BeginInvokeOnMainThread(() =>
			{

				ProductsListView.ItemsSource = products;
			});
			Search(searchbar.Text);
	}
	public void SortType()
	{
		// Load suppliers and products

		products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();

		// Sort by category (enum)
		products = products
			.OrderBy(p => p.ProudctCategory) // Enum sort is by underlying int value unless custom comparer used
			.ToList();

		MainThread.BeginInvokeOnMainThread(() =>
		{


			ProductsListView.ItemsSource = products;
		});
		Search(searchbar.Text);
}

	public void SortQuantity()
	{



		products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
		products = products
			   .OrderBy(p => p.Quantity)
			   .ToList();
		ProductsListView.ItemsSource = products;
		Search(searchbar.Text);
	}
		public void SortMostSelling()
	{
			


				products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
 products = products
        .OrderByDescending(p => p.SellCount)
        .ToList();
		ProductsListView.ItemsSource = products;
	}
			public void SortemptyBarcode()
	{
			


				products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
 products = products
        .Where(p=>string.IsNullOrEmpty(p.ProudctId))
        .ToList();
		ProductsListView.ItemsSource = products;
	}

    private async void AddProduct_Clicked(System.Object sender, System.EventArgs e)
	{
		await Navigation.PushAsync(new AddProduct());
	}

	private async void ProductsListView_ItemTapped(object sender, ItemTappedEventArgs e)
	{
			await Navigation.PushAsync(new AddProduct(e.Item as Product));
    }
		private async void Back_Clicked(object sender, EventArgs e)
	{
		try{
await Navigation.PopAsync();
		}
		catch(Exception ex){

		}
	}

	private void searchbar_TextChanged(object sender, TextChangedEventArgs e)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{


			try
			{
				if (string.IsNullOrEmpty(e.NewTextValue))
				{
	
					ProductsListView.ItemsSource = products;
				}
				else
				{
					Search(e.NewTextValue);
				}
			}
			catch (System.Exception ex)
			{

				Debug.WriteLine(ex);
			}
			});
	}
	void Search(string text)
	{
		MainThread.BeginInvokeOnMainThread(() =>
		{

if (string.IsNullOrEmpty(text))
            {
				ProductsListView.ItemsSource = products;
				return;
            }

			try
			{
				List<Product> searchproducts = new List<Product>();
				foreach (var p in products)
				{
					if (p.ProudctName.ToLower().Contains(text.ToLower()) || (p.ProudctId ?? string.Empty).ToLower().Contains(text.ToLower()))
					{
						searchproducts.Add(p);
					}
					else if (p.SubProduct != null)
					{



						if (p.SubProduct.ProudctName.ToLower().Contains(text.ToLower()))
						{
							searchproducts.Add(p);

						}
					}
				}

				ProductsListView.ItemsSource = searchproducts;
			}
			catch (System.Exception ex)
			{

				Debug.WriteLine(ex);

			}
					});
	}

	private void orderpicker_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (orderpicker.SelectedIndex == 0)
		{
			SortAlpahbitic();
		}
		else if (orderpicker.SelectedIndex == 1)
		{
			SortSupplier();
		}
		else if (orderpicker.SelectedIndex == 2)
		{
			SortQuantity();
		}
		else if (orderpicker.SelectedIndex == 3)
		{
			SortMostSelling();
		}
		else if (orderpicker.SelectedIndex == 4)
		{
			SortType();
		}
			else if (orderpicker.SelectedIndex == 5)
			{
				SortemptyBarcode();
		}
	}

	private void catgoryPicker_SelectedIndexChanged(object sender, EventArgs e)
	{
		if (catgoryPicker.SelectedItem is string str)
		{


			if (str == "الكل")
			{
				ProductsListView.ItemsSource = products;
			}
			else
			{

				ProductsListView.ItemsSource = products.Where(p => p.ProudctCategory.ToString() == str).ToList();
			}
			}
    }
    }
