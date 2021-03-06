﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExpenseApp.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.ObjectModel;

namespace ExpenseApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EntryPage : ContentPage
    {

        EntryPageViewModel viewModel = new EntryPageViewModel();
        //int month;
        public string month1;
        
        public EntryPage()
        {
            InitializeComponent();
            BindingContext = viewModel;

            viewModel.currentMonth = DateTime.Now.Month;
            viewModel.expenses = new ObservableCollection<Expense>();
            viewModel.transactions = new ObservableCollection<Transaction>();
            viewModel.envelope = new ObservableCollection<Transaction>();
            listview.ItemsSource = viewModel.expenses;
            Appearing += (sender, args) => CheckBudget();
            Appearing += (sender, args) => CheckExpense();
        }


        private async void CheckBudget()
        {
           
            var hasBudget = File.Exists(App.budget_filename);
            if (!hasBudget)
            {
                // instead using PushAsync directly, use BeginInvokeOnMainThread will avoid crash
                Device.BeginInvokeOnMainThread(async () => await Navigation.PushAsync(new AddBudgetPage
                {
                    BindingContext = new Budget()
                }));
            }
            // else we can just read the budget from the budget file
            else
            {
                viewModel.MonthlyPlan = float.Parse(File.ReadAllText(App.budget_filename));
            }
            if (viewModel.MonthlyPlan == 0)
            {
                await DisplayAlert("Alert", "Set Monthly Budget cannot be 0. Please set your monthly budget.", "OK");
                return;
            }

        }
        private async void CheckExpense()
        {
            var hasTransaction = File.Exists(App.transaction_filemane);

            if (!hasTransaction)
            {
                File.Create(App.transaction_filemane);
            }
            else
            {
                //read transactions
                EntryPageViewModel.ReadAllTransactions(viewModel.transactions, App.transaction_filemane);
                viewModel.UpdateExpenses(viewModel.expenses, viewModel.currentMonth, viewModel.transactions);
            }

            MonthPicker.SelectedIndex = viewModel.currentMonth - 1;
        }

        async void OnBudgetAddedClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AddBudgetPage
            {
                BindingContext = new Budget()
            });
        }
        
    private void OnSetButton_Clicked(object sender, EventArgs e)
    {

    }


   async void OnListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
           //Expense selectedItem = e.SelectedItem as Expense;
           // string month1 = selectedItem.ToString();
            //move to below, OnListViewItemTapped
        }
        async void OnListViewItemTapped(object sender, ItemTappedEventArgs e)

        {

            var val = sender as ListView;

            string selectedenvelope = val.SelectedItem.ToString();

            viewModel.transactionsByMonth(viewModel.transactions, viewModel.currentMonth, selectedenvelope, viewModel.envelope);

            var categoryPage = new CategoryPage(viewModel,selectedenvelope,month1);

            await Navigation.PushAsync(categoryPage);

        }
        /*async void OnListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
                var val = sender as ListView;
                string selectedenvelope = val.SelectedItem.ToString();
                viewModel.transactionsByMonth(viewModel.transactions, viewModel.currentMonth, selectedenvelope, viewModel.envelope);
                var categoryPage = new CategoryPage(viewModel,selectedenvelope, month1);
                string p = month1;
                await Navigation.PushAsync(categoryPage);
            }*/

        async void OnAddButton_Clicked(object sender, EventArgs e)
      {
          await Navigation.PushAsync(new TransactionPage
            {
                BindingContext = new Transaction()
            });
     }

        public void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            var P = sender as Picker;
            month1 = P.SelectedItem.ToString();
            viewModel.currentMonth = (P.SelectedIndex+1);
            
            viewModel.UpdateExpenses(viewModel.expenses, viewModel.currentMonth, viewModel.transactions);
            
        }
    }
}