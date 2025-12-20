using System.Threading.Tasks;
using ATARAXIA;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using ESCPOS_NET.Utilities;


namespace Cashier;

public partial class AddProduct : ContentPage
{
	public  Product productt = new Product();
	bool IsProductDetail = false;
	public AddProduct(Product product = null)
	{
		InitializeComponent();
		categoryPicker.ItemsSource =Enum.GetNames(typeof(ProductCategory));
		SupplierPicker.ItemsSource=   (HandleData.GetElement<List<Supplier>>.GetItem("Suppliers") ?? new List<Supplier>()).Select(p=>p.Name).ToList();
	
		if (product != null)
		{
			productt = product;
			delproductbor.IsVisible = true;
			addproductbut.Text = "تعديل المنتج";
			IsProductDetail = true;
			ProductNameEntry.Text = product.ProudctName ?? string.Empty;
			RealPriceEntry.Text = product.RealPrice.ToString() ?? string.Empty;
			ProductQuantaty.Text = product.Quantity.ToString() ?? string.Empty;

			categoryPicker.SelectedIndex = (int)product.ProudctCategory;
			SupplierPicker.SelectedItem = product.SupplierName ?? string.Empty;
			if (product.SubProduct != null)
			{
				subcheckbox.IsChecked = true;
				subname.Text = product.SubProduct.ProudctName;
				SubProductbarcode.Text = product.SubProduct.ProudctId ?? string.Empty;
				SubProductCountent.Text = product.SubProductCount.ToString() ?? string.Empty;
				var total = product.Quantity;
				double integerPart = Math.Floor(total); // Get the integer part
				double decimalPart = total - integerPart; // Calculate the decimal part
				ProductQuantaty.Text = integerPart.ToString();
				SubProuctQuantatity.Text = Math.Round(decimalPart * product.SubProductCount).ToString();
				SubProuctQuantatity.IsVisible = true;
				subpriceent.Text = product.SubProduct.ProudctPrice.ToString() ?? string.Empty;
           producttypeent.Text = product.SubProducttype ?? string.Empty;			//	SubProuctQuantatity.Text = product.SubProductCount.ToString() ?? string.Empty;
			}

			gomlapriceentry.Text = product.GomlaPrice.ToString() ?? string.Empty;


			PriceEntry.Text = product.ProudctPrice.ToString() ?? string.Empty;
			//	SubProuctQuantatity.Text = product.SubProductCount.ToString() ?? string.Empty;
			// ProductQuantityEntry.Text = product.Quantity.ToString() ?? string.Empty;
			BarcodeEntry.Text = product.ProudctId ?? string.Empty;
		}

		

	}

	 public static string Generate13DigitNumber()
    {
          Random random = new Random();
        string result = random.Next(100, 999).ToString(); // First 3 digits (non-zero)

        for (int i = 0; i < 5; i++) // Remaining 9 digits
        {
            result += random.Next(0, 10).ToString();
        }

        return result;
    }
	private async void AddProduct_Clicked(System.Object sender, System.EventArgs e)
	{

		var product = new Product()
		{
			ProudctId = BarcodeEntry.Text,
			ProudctName = ProductNameEntry.Text,
	
			// SubProduct = SubProductent.Text,

			// ProudctPrice = PriceEntry.Text
			// Quantity = ProductQuantityEntry.Text,
		};
		if (categoryPicker.SelectedIndex >= 0)
		{
			product.ProudctCategory = (ProductCategory)categoryPicker.SelectedIndex;
		}
		if (SupplierPicker.SelectedItem is string str)
		{
			product.SupplierName = str;
		}
		if (subcheckbox.IsChecked)
		{
			var SubProduct = new Product() { ProudctPrice = double.Parse(subpriceent.Text), ProudctName = subname.Text,ProudctId= SubProductbarcode.Text };
			var total = double.Parse(ProductQuantaty.Text) + ((double.Parse(SubProuctQuantatity.Text ?? "0") / double.Parse(SubProductCountent.Text ?? "1")));
			product.Quantity = total;
			product.SubProduct = SubProduct;
			product.SubProducttype = producttypeent.Text;
			product.SubProductCount = int.Parse(SubProductCountent.Text);

		}
		else
		{
			product.Quantity = double.Parse(ProductQuantaty.Text);
		}
		bool allowed = true;
		if (double.TryParse(PriceEntry.Text, out double price))
		{
			product.ProudctPrice = price;
		}
		else
		{

			await DisplayAlert("معذرة", "قم بادخال سعر صحيح", "اغلاق");
			allowed = false;
		}
		if (double.TryParse(RealPriceEntry.Text, out double Realprice))
		{
			product.RealPrice = Realprice;
		}
		else
		{

			await DisplayAlert("معذرة", "قم بادخال سعر صحيح", "اغلاق");
			allowed = false;
		}
		if (double.TryParse(gomlapriceentry.Text, out double Gomlaprice))
		{
			product.GomlaPrice = Gomlaprice;
		}
		else
		{

			await DisplayAlert("معذرة", "قم بادخال سعر الجملة صحيح", "اغلاق");
			allowed = false;
		}
		// if (double.TryParse(ProductQuantaty.Text, out double Quantity))
		// {
		// 	product.Quantity = Quantity;
		// }
		// else
		// {

		// 	await DisplayAlert("معذرة", "قم بادخال كمية صحيحة", "اغلاق");
		// 	allowed = false;
		// }
		// if (int.TryParse(SubProductent.Text, out int SubProductCount))
		// {
		// 	product.SubProductCount = SubProductCount;
		// }
		// else
		// {

		// 	await DisplayAlert("معذرة", "قم بادخال سعر الجملة صحيح", "اغلاق");
		// 	allowed = false;
		// }
		if (allowed)
		{
		var products=	HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
		var names = products?.Select(x => x.ProudctId).ToList();
			bool shouldadd = true;

			foreach (var l in products)
			{
				if (l.ProudctId == product.ProudctId && l.ProudctName == product.ProudctName)
				{
					l.ProudctPrice = product.ProudctPrice;
					l.Quantity = product.Quantity;
					l.quant = product.quant;
					l.RealPrice = product.SellCount;
					l.SupplierName = product.SupplierName;
					l.SubProduct = product.SubProduct;
					l.SubProductCount = product.SubProductCount;
					l.SubProducttype = product.SubProducttype;
					l.ProudctCategory = product.ProudctCategory;
					shouldadd = false;

					break;   

				}
			}
			if (shouldadd)
			{
				 	products.Add(product);
			}
			// if (names.Contains(product.ProudctId))
			// {

			// 	var ind = names.IndexOf(product.ProudctId);
			// 	products[ind] = product;
			// }
			// else
			// {
			// 	products.Add(product);
			// }
			HandleData.AddItemToStorage("Products", products);
			await Navigation.PopAsync();
		}


		// await Navigation.PushAsync(new AddProduct());
	}

	private void GenerateRandomBarcode_Clicked(object sender, EventArgs e)
	{
		

		BarcodeEntry.Text = Generate13DigitNumber();
	}
		private void GenerateRandomSubBarcode_Clicked(object sender, EventArgs e)
	{
		

		SubProductbarcode.Text = Generate13DigitNumber();
	}
	
	private async void PrintSticker_Clicked(object sender, EventArgs e)
	{

	}

	private void subcheckbox_CheckedChanged(object sender, CheckedChangedEventArgs e)
	{
		SubProcutIsVis.IsVisible = e.Value;
		SubProuctQuantatity.IsVisible = e.Value;
	}

	private void producttypeent_TextChanged(object sender, TextChangedEventArgs e)
	{
		SubProuctQuantatity.Placeholder = e.NewTextValue;
	}
	private async void Del_Clicked(object sender, EventArgs e)
	{
	var list =	 HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
	var newlist = list.Where(p=>p.ProudctId != productt.ProudctId).ToList();
	HandleData.AddItemToStorage("Products",newlist);
	await Navigation.PopAsync();
	}

	private async void Print_Clicked(object sender, EventArgs e)
	{
		var name = SubProductbarcode.Text;


	
		string result = "";
		 await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            result = await DisplayPromptAsync("Required", "ادخل الكمية");
        });
		if (double.TryParse(result, out double value)){
for (int i = 0; i < value; i++)
{
    var barcode = new BarcodePrinter(){};
	barcode.PrintBarcode(name ,.75,label:subname.Text);
}
		}
			
	}
	private async void PrintMain_Clicked(object sender, EventArgs eE)
	{
	
		var name = BarcodeEntry.Text;
			

	
		string result = "";
		 await MainThread.InvokeOnMainThreadAsync(async () =>
        {
            result = await DisplayPromptAsync("Required", "ادخل الكمية");
        });
		if (double.TryParse(result, out double value)){
			
			
for (int i = 0; i < value; i++)
{
    var barcode = new BarcodePrinter(){};
	barcode.PrintBarcode(name ,.75,label:ProductNameEntry.Text);
}
		}
			
    }
		private async void Back_Clicked(object sender, EventArgs e)
	{
		try{
			InventoryPage.Refresh = false;
await Navigation.PopAsync();
		}
		catch(Exception ex){

		}
	}

    private void categoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
    }
		
	}
