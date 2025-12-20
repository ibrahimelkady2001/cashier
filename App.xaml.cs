using System.Diagnostics;
using System.Net;
using ATARAXIA;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;

namespace Cashier;

public partial class App : Application
{
    public static List<Order> CurrentOrders = new List<Order>(); // List to store orders>
    public App()
    {
        InitializeComponent();
    }
    protected async override void OnStart()
    {
        await StartServerAsync();
        // base.OnStart();
    }
    public async void Init()
    {
        await StartServerAsync();
    }
       private string GetLocalIPAddress()
        {
            string localIP = "127.0.0.1"; // fallback
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());
                localIP = host
                    .AddressList
                    .FirstOrDefault(ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?
                    .ToString() ?? localIP;
            }
            catch(Exception ex)
        {
            Debug.WriteLine(ex);
                // handle exception if needed
            }
            return localIP;
        }
   private async Task StartServerAsync()
    {

        await Task.Factory.StartNew(async () =>
          {
              try
              {


                 string ip = GetLocalIPAddress();
            var url = $"http://{ip}:7084/";

                  var server = new WebServer(o => o
             .WithUrlPrefix(url)
             .WithMode(HttpListenerMode.EmbedIO)).WithWebApi("/api", m => m.WithController<MyApiControllerr>());
                  //  .WithStaticFolder("/", "wwwroot", true) // Serve static files from wwwroot folder
                  //   .WithAction("/", HttpVerbs.Get, async ctx => await ctx.SendStringAsync("Hello from .NET MAUI and EmbedIO!", "text/html", System.Text.Encoding.UTF8));

                  await server.RunAsync();
              }
              catch (Exception ex)
              {
                  Debug.WriteLine(ex);
              }
          });
    }


    public class MyApiControllerr : WebApiController
    {


        [Route(HttpVerbs.Post, "/AddOrder")]
        public bool AddOrder([JsonData] Order order)
        {
            CurrentOrders.Add(order);
            MainThread.BeginInvokeOnMainThread(() =>
            {

       
 Cashier.MainPage.OrdersCountlab.Text = "عدد الطلبات: " + App.CurrentOrders.Count();
      });
            return true;
        }
          [Route(HttpVerbs.Post, "/AddProduct")]
        public bool AddProduct([JsonData] Product product)
        {
           var products=	HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
		var names = products?.Select(x => x.ProudctId).ToList();
			if (names.Contains(product.ProudctId ) && products.Find(p=>p.ProudctName == product.ProudctName) != null)
			{
				var ind = names.IndexOf(product.ProudctId);
				products[ind] = product;
			}
			else
			{
				products.Add(product);
		}
			HandleData.AddItemToStorage("Products", products);
			

            
            return true;
        }
         [Route(HttpVerbs.Post, "/GetEmployees")]
        public List<string> GetEmployees()
        {
            var list = HandleData.GetElement<List<Employee>>.GetItem("Employees") ?? new List<Employee>(); ; // List to store orders>>

            return list.Select(x => x.EmployeeName).ToList();
        }

        public async void displayalaert(string barode)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert("Alert", barode, "OK");

            });
        }
          [Route(HttpVerbs.Post, "/print")]
        public bool print([JsonData] Dictionary<string,string> dic )
        {
var quantity =  dic["quantity"];
var barcodee = dic["barcode"];

		if (double.TryParse(quantity, out double value)){
for (int i = 0; i < value; i++)
{
    var barcode = new BarcodePrinter(){};
	barcode.PrintBarcode(barcodee ,.75);
}
		}
        return true;
			}
    
        
        [Route(HttpVerbs.Post, "/GetProductInfo")]
        public Product GetProductInfo([JsonData] Dictionary<string,string> dic )
        {
            var barcodeNumber = dic["Barcode"];
  //    displayalaert(barcodeNumber);
            var Products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
            foreach (var product in Products)
            {
                if (product.ProudctId == barcodeNumber)
                {
                    return product;
                }
                else if (product.SubProduct != null)
                {
                    if (product.SubProduct.ProudctName == barcodeNumber    )
                    {
                        return product.SubProduct;
                    }
                    else if (product.SubProduct.ProudctId  == barcodeNumber)
                    {
                          return product.SubProduct;
                    }

                }
            }
            return null;
        }
        



          [Route(HttpVerbs.Post, "/GetProducts")]
        public List< Product> GetProducts()
        {
           
            List<Product> AllProducts = new List<Product>();
  //    displayalaert(barcodeNumber);
            var Products = HandleData.GetElement<List<Product>>.GetItem("Products") ?? new List<Product>();
            foreach (var product in Products)
            {
                AllProducts.Add(product);
             if (product.SubProduct != null)
                {
                    var subproduct = new Product()
                    {
                        ProudctName = product.SubProduct.ProudctName,
                        ProudctPrice = product.SubProduct.ProudctPrice,
ProudctId = product.SubProduct.ProudctId

                    };
                    AllProducts.Add(subproduct);
                }
                
            }
            return AllProducts;
        }
        
}
    

    
    
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new NavigationPage(new MainPage()));
    }
}