using System;
using Syncfusion.Maui.Toolkit.Picker;

namespace Cashier;

public partial class TransactionSummaryPage : ContentPage
{
    private readonly TransactionViewModel _viewModel;

    public TransactionSummaryPage()
    {
        InitializeComponent();
        _viewModel = new TransactionViewModel();
        BindingContext = _viewModel;
        
        // Set maximum date for end date picker to today
        EndDatePicker.MaximumDate = DateTime.Today;
    }

    private void OnTodayClicked(object sender, EventArgs e)
    {
        _viewModel.SetToday();
    }

    private void OnLast30DaysClicked(object sender, EventArgs e)
    {
        _viewModel.SetLast30Days();
    }

    private void OnLastMonthClicked(object sender, EventArgs e)
    {
        _viewModel.SetLastMonth();
    }

    private void OnStartDateChanged(object sender, DatePickerSelectionChangedEventArgs e)
    {
        if (e.NewValue.HasValue)
        {
            _viewModel.StartDate = e.NewValue.Value;
            _viewModel.FilterOrders();
        }
    }

    private void OnEndDateChanged(object sender, DatePickerSelectionChangedEventArgs e)
    {
        if (e.NewValue.HasValue)
        {
            _viewModel.EndDate = e.NewValue.Value;
            _viewModel.FilterOrders();
        }
    }
}