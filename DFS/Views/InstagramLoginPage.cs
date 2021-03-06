﻿using System;

using Xamarin.Forms;

namespace DFS.Views
{
    public class InstagramLoginPage : ContentPage
    {
        bool _flag;
        public InstagramLoginPage(bool flag)
        {
            _flag = flag;
            Title = "Instagram Profile";
            BackgroundColor = Color.White;

            var url = "https://api.instagram.com/oauth/authorize/?client_id=4573363c81554c7293268d33e1de8d09&redirect_uri=https://instagram.com&response_type=token&scope=basic";

            WebView webView = new WebView
            {
                Source = url,
                HeightRequest = 1
            };

            webView.Navigating+=WebView_Navigating;

            Content = webView;
        }

        async void WebView_Navigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url.StartsWith("https://instagram.com/",StringComparison.CurrentCulture)) {
                var arr = e.Url.Split('#');
                if (arr.Length > 0) {
                    var arr1 = arr[1].Split('=');
                    if (arr1.Length > 0)
                    {
                        var accessToken = arr1[1];
                        if (_flag)
                            MessagingCenter.Send<Object, string>(this, "InstagramLogin", accessToken);
                        else
                            MessagingCenter.Send<Object, string>(this, "InstagramMedia", accessToken);

                        await this.Navigation.PopAsync();
                    }
                }
            }
        }

    }
}

