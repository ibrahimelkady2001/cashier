// EmployeePage.xaml.cs


namespace Cashier;

public partial class EmployeePage : ContentPage
{
    public Employee CurrentEmployee { get; set; }

    public EmployeePage( Employee employee)
    {
        InitializeComponent();
        CurrentEmployee = employee;

        // --- بيانات تجريبية ---
        // في تطبيق حقيقي، ستحصل على هذه البيانات من قاعدة بيانات أو API
      

        // This is the most important line: it connects the XAML to our data object.
        // هذا هو السطر الأهم: يربط واجهة المستخدم الرسومية ببيانات الموظف
        this.BindingContext = CurrentEmployee;
    }
}