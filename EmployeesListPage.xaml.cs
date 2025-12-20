// EmployeesListPage.xaml.cs


using ATARAXIA;

namespace Cashier;

public partial class EmployeesListPage : ContentPage
{
    public EmployeesListPage()
    {
        InitializeComponent();
   
        // تم تعيين BindingContext في XAML، لذا لا حاجة لفعله هنا
    }
    protected override void OnAppearing()
    {
             var list = HandleData.GetElement<List<Employee>>.GetItem("Employees") ?? new List<Employee>();
        ListV.ItemsSource = list;
        base.OnAppearing();
    }


    private async void OnEmployeeSelected(object sender, SelectionChangedEventArgs e)
    {
        // التأكد من أن هناك عنصر تم اختياره
        if (e.CurrentSelection.FirstOrDefault() is  Employee selectedEmployee)
        {
            await Navigation.PushAsync(new EmployeePage(selectedEmployee));
        }

        // هنا يمكنك الانتقال إلى صفحة تفاصيل الموظف
        // في تطبيق حقيقي، ستقوم بتمرير كائن الموظف المختار للصفحة التالية


        // قم بإلغاء تحديد العنصر للسماح باختياره مرة أخرى
        ((CollectionView)sender).SelectedItem = null;
    }
        private async void OnAddNewEmployeeClicked(object sender, EventArgs e)
    {
        // الانتقال إلى صفحة إضافة موظف جديد
       await Navigation.PushAsync ( new AddEmployeePage());
    }
}