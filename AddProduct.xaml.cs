using System.Threading.Tasks;
using ATARAXIA;

namespace Cashier;

public partial class AddProduct : ContentPage
{
	public  Product product = new Product();
	bool IsProductDetail = false;
	public AddProduct(Product product = null)
	{
		InitializeComponent();
		if (product != null)
		{
			addproductbut.Text = "تعديل المنتج";
			IsProductDetail = true;
			ProductNameEntry.Text = product.ProudctName ?? string.Empty;
			RealPriceEntry.Text = product.RealPrice.ToString() ?? string.Empty;
			ProductQuantaty.Text = product.Quantity.ToString() ?? string.Empty;
			if (product.SubProduct != null)
			{
				subcheckbox.IsChecked = true;
				subname.Text = product.SubProduct.ProudctName;
				SubProductent.Text = product.SubProduct.ProudctName ?? string.Empty;
				SubProductCountent.Text = product.SubProductCount.ToString() ?? string.Empty;
				var total = product.Quantity;
				double integerPart = Math.Floor(total); // Get the integer part
				double decimalPart = total - integerPart; // Calculate the decimal part
				ProductQuantaty.Text = integerPart.ToString();
				SubProuctQuantatity.Text = Math.Round(decimalPart * product.SubProductCount).ToString();
				SubProuctQuantatity.IsVisible = true;
					subpriceent.Text = product.SubProduct.ProudctPrice.ToString() ?? string.Empty;
		//	SubProuctQuantatity.Text = product.SubProductCount.ToString() ?? string.Empty;
			}
			
			gomlapriceentry.Text = product.GomlaPrice.ToString() ?? string.Empty;
		
		
			PriceEntry.Text = product.ProudctPrice.ToString() ?? string.Empty;
		//	SubProuctQuantatity.Text = product.SubProductCount.ToString() ?? string.Empty;
			// ProductQuantityEntry.Text = product.Quantity.ToString() ?? string.Empty;
			BarcodeEntry.Text = product.ProudctId ?? string.Empty;
		}

	}
	public static string GenerateRandom8DigitNumber()
	{
		Random random = new Random();
		const string digits = "0123456789";
		char[] eightDigits = new char[8];

		// Ensure the first digit is not zero to avoid numbers like "01234567"
		eightDigits[0] = digits[random.Next(1, 10)]; // First digit (1-9)

		// Fill the remaining 7 digits with random numbers (0-9)
		for (int i = 1; i < 8; i++)
		{
			eightDigits[i] = digits[random.Next(10)]; // Digits (0-9)
		}

		return new string(eightDigits);
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
		if (subcheckbox.IsChecked)
		{
var SubProduct = new Product(){ProudctId = SubProductent.Text, ProudctPrice = double.Parse(subpriceent.Text),ProudctName = subname.Text };
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
		if (double.TryParse(RealPriceEntry.Text, out double Gomlaprice))
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
			if (names.Contains(product.ProudctId))
			{
				var ind = names.IndexOf(product.ProudctId);
				products[ind] = product;
			}
			else
			{
				products.Add(product);
		}
			HandleData.AddItemToStorage("Products", products);
			await Navigation.PopAsync();
		}


		// await Navigation.PushAsync(new AddProduct());
	}

	private void GenerateRandomBarcode_Clicked(object sender, EventArgs e)
	{

		BarcodeEntry.Text = GenerateRandom8DigitNumber();
	}
	private void GenerateRandoSubBarcode_Clicked(object sender, EventArgs e)
	{

		SubProductent.Text = GenerateRandom8DigitNumber();
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


	private void Print_Clicked(object sender, EventArgs e)
	{
		var name = SubProductent.Text;
	}
	private void PrintMain_Clicked(object sender, EventArgs e)
	{
		var name = BarcodeEntry.Text;
    }
		
	}
