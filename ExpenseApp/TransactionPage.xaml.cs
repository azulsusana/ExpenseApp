﻿using System;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ExpenseApp.Model;

namespace ExpenseApp
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TransactionPage : ContentPage
    {

        public TransactionPage()
        {
            InitializeComponent();
        }

        public async void OnAddanotherExpense_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new TransactionPage
            {
                BindingContext = new Transaction()
            });
        }



        public async void OnSaveButton_Clicked_1(object sender, EventArgs e)
        {
            var T = (Transaction)BindingContext;
            T.Envelope = Picker_E.SelectedItem.ToString();
            T.WriteToFile();
            //NavigationPage.CurrentPageProperty;
        }

       public async void OnCancelButton_Clicked_2(object sender, EventArgs e)
        {
            await Navigation.PopAsync();
        }
    }

}