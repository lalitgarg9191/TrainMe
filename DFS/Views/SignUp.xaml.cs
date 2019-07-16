﻿using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace DFS.Views
{
    public partial class SignUp : ContentPage
    {
        ViewModels.SignupViewModel signupViewModel;
        public SignUp(String _selectedView)
        {
            InitializeComponent();

            BindingContext = signupViewModel = new ViewModels.SignupViewModel();

            signupViewModel.SelectedView = _selectedView;

        }

        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
            if (signupViewModel.EmailAddress == null || signupViewModel.Password == null || signupViewModel.ConfirmPassword == null)
            {
                await DisplayAlert("Alert", "Please enter Username/Password.", "OK");
                return;
            }
            else if (!IsValidEmail(signupViewModel.EmailAddress))
            {
                await DisplayAlert("Alert", "Please enter the correct email id.", "OK");
                return;
            }
            else if (signupViewModel.Password != signupViewModel.ConfirmPassword)
            {
                await DisplayAlert("Alert", "Please enter the correct password to confirm.", "OK");
                return;
            }
            else
            {
                await this.Navigation.PushAsync(new UserInformationPage(signupViewModel));
            }
        }


        private bool IsValidEmail(string email)
        {
            if (Regex.Match(email, @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$").Success)
            {
                //Valid email 
                return true;
            }
            else
            {
                //Not valid email    
                return false;
            }
        }
    }
}
