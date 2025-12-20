// AddEmployeePage.xaml.cs


using ATARAXIA;

namespace Cashier;

public partial class AddEmployeePage : ContentPage
{
    public Employee NewEmployee { get; set; }

    public AddEmployeePage()
    {
        InitializeComponent();

        // إنشاء كائن جديد لربط البيانات
        NewEmployee = new Employee
        {
            MoneyDay = "1", // قيمة افتراضية
            OwedMoney = "0",
Salary ="0",
        };

        // تعيين سياق الربط
        this.BindingContext = this;
    }

    private async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        // تحقق بسيط من صحة البيانات
        if (string.IsNullOrWhiteSpace(NewEmployee.EmployeeName))
        {
            await DisplayAlert("خطأ", "الرجاء إدخال اسم الموظف وراتب صحيح.", "موافق");
            return;
        }
        HandleData.AddToListInStorage<Employee>("Employees", NewEmployee);

        // إرسال رسالة تحتوي على الموظف الجديد إلى أي صفحة تستمع


        // العودة إلى الصفحة السابقة (صفحة القائمة)
        await Navigation.PopAsync();
    }

    private async void OnCancelButtonClicked(object sender, EventArgs e)
    {
        // العودة إلى الصفحة السابقة دون حفظ
       await Navigation.PopAsync();
    }
}