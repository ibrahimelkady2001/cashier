using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using ATARAXIA;
using System.Collections.Generic;
using System.Linq;

namespace Cashier;


public partial class OrderDetailPageHistory : ContentPage

{
	public List<Product> Oldproducts = new List<Product>();
	public Order CurrentOrder = null;
	public OrderDetailPageHistory(Order order, bool Ishistory)
	{
		InitializeComponent();
		Oldproducts = order.Products.ToArray().ToList();

		CurrentOrder = order;
		this.BindingContext = order;
	}

	private async void cancel_Clicked(object sender, EventArgs e)
	{
		var ind = App.CurrentOrders.Select(p => p.OrderId).ToList().IndexOf(CurrentOrder.OrderId);
		// HandleData.AddToListInStorage<Order>("OrderHistory", CurrentOrder);
		App.CurrentOrders.RemoveAt(ind);
		await Navigation.PopToRootAsync();
	}

	private async void CreateBut_Clicked(object sender, EventArgs e)
	{

		// Compare Products 

	var items = RefundCalculator.GetOrderChanges(Oldproducts, CurrentOrder.Products);
		var list = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
		foreach (var item in items)
		{
			foreach (var product in list)
			{
				if (product.ProudctId == item.ProudctId)
				{
					var count = product.Quantity + double.Parse(item.quant);
					if (count >= 0)
					{
						product.Quantity = count;
					}
					else
					{
						await DisplayAlert($"Error", " {product.ProudctId} لا يوجد كمية كافية", "OK");
					}

				}
				else if (product.SubProduct != null)
				{
					if (product.SubProduct.ProudctId == item.ProudctId)
					{
						var count = product.SubProduct.Quantity - double.Parse(item.quant);
						if (count >= 0)
						{
							product.SubProduct.Quantity = count;
						}
						else
						{
							await DisplayAlert($"Error", " {product.SubProduct.ProudctId}لا يوجد كمية كافية", "OK");
						}

					}
				}
			}
		}
		// var Employees=	HandleData.GetElement<List<Employee>>.GetItem("Employees") ?? new List<Employee>();
		// var emp =	Employees.Find(p => p.EmployeeName == CurrentOrder.EmployeeName);
		// 	if (emp != null)
		// 	{
		// 	emp.OwedMoney = emp.OwedMoney + CurrentOrder.Total *.005;
		// 	HandleData.AddItemToStorage("Employees", Employees);
		// }

		var ind = App.CurrentOrders.Select(p => p.OrderId).ToList().IndexOf(CurrentOrder.OrderId);

		HandleData.AddToListInStorage<Order>("OrderHistory", CurrentOrder);
		App.CurrentOrders.RemoveAt(ind);
		await Navigation.PopToRootAsync();
	}
	async void PrintReceipt()
	{

	}
	private async void Back_Clicked(object sender, EventArgs e)
	{
		try
		{
			await Navigation.PopAsync();
		}
		catch (Exception ex)
		{

		}
	}

}