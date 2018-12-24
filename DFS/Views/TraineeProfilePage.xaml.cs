﻿using System;
using System.Collections.Generic;
using DFS.Utils;
using DFS.ViewModels;
using Xamarin.Forms;

namespace DFS.Views
{
    public partial class TraineeProfilePage : ContentPage
    {
        RootPage root;
        TraineeProfileViewModel traineeProfileViewModel;
        public TraineeProfilePage(RootPage root)
        {
            InitializeComponent();

            this.root = root;

            BindingContext =traineeProfileViewModel= new TraineeProfileViewModel();
        }

        void Handle_Tapped(object sender, System.EventArgs e)
        {
            //this.Navigation.PushAsync(new UserInformationPage(new ViewModels.SignupViewModel()));
            App.Current.MainPage = new UserInformationPage(new ViewModels.SignupViewModel());
        }

        void Handle_TraineMe(object sender, System.EventArgs e)
        {
            this.root.NavigateAsync((int)MenuType.CoachList);
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            MessagingCenter.Subscribe<Object, string>(this, "InstagramMedia", async (arg1, arg2) =>
            {
                try
                {
                    if (!string.IsNullOrEmpty(arg2))
                    {
                        traineeProfileViewModel.IsServiceInProgress = true;
                        var result = await App.TodoManager.GetInstagramMedia(arg2);

                        if (result.Equals("Success"))
                        {
                            CredentialsService.SaveCredentials(instagramMedia: App.InstagramMedia);
                            Application.Current.MainPage = new RootPage(App.SelectedView);
                        }
                        else
                        {
                            await DisplayAlert("Alert", result, "Ok");
                        }
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Alert", "something went wrong", "Ok");
                }
                traineeProfileViewModel.IsServiceInProgress = false;
            });

        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            MessagingCenter.Unsubscribe<InstagramLoginPage, string>(this, "InstagramMedia");
        }

        async void Handle_Tapped_1(object sender, System.EventArgs e)
        {
            await this.Navigation.PushAsync(new InstagramLoginPage(false));
        }
    }
}