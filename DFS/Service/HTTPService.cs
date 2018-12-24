﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using DFS.Models;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace DFS
{
    public class HTTPService : IRestService
    {
        HttpClient client;

        public HTTPService()
        {
            client = new HttpClient();
            client.MaxResponseContentBufferSize = 256000;

        }


        public async Task<string> GetInstagramInfo(string accessToken)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var url = "https://api.instagram.com/v1/users/self/?access_token=" + accessToken;

                var uri = new Uri(url);

                try
                {

                    HttpResponseMessage response = null;

                    response = await client.GetAsync(uri);


                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;

                        var instagramUser = JsonConvert.DeserializeObject<InstagramUser>(responseJson);
                        App.InstagramUser = instagramUser;

                        return "Success";
                    }
                    else
                    {
                        return "Internal Server Error. Please try again.";
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return "Internal Server Error. Please try again.";
                }
            }
            else
            {
                return "Internet Connectivity error. Please try again.";
            }
        }

        public async Task<string> GetInstagramMedia(string accessToken)
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                var url = "https://api.instagram.com/v1/users/self/media/recent/?access_token=" + accessToken;

                var uri = new Uri(url);

                try
                {

                    HttpResponseMessage response = null;

                    response = await client.GetAsync(uri);


                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;

                        var instagramMedia = JsonConvert.DeserializeObject<InstagramMedia>(responseJson);
                        App.InstagramMedia = instagramMedia;

                        return "Success";
                    }
                    else
                    {
                        return "Internal Server Error. Please try again.";
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return "Internal Server Error. Please try again.";
                }
            }
            else
            {
                return "Internet Connectivity error. Please try again.";
            }

        }
        public async Task<string> GetFacebookInfo()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                String access_token = await GetAccessToken();

                if (access_token == "Internal Server Error. Please try again.")
                {
                    return "Internal Server Error. Please try again.";
                }


                String url = "https://graph.facebook.com/v3.2/me?fields=name,picture,age_range,birthday,devices,email,first_name,last_name,gender,languages&access_token=" + access_token;

                var uri = new Uri(url);

                try
                {

                    HttpResponseMessage response = null;

                    response = await client.GetAsync(uri);


                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        //FacebookProfile facebook = JsonConvert.DeserializeObject<FacebookProfile>(responseJson);

                        //App.FacebookProfile = facebook;

                        return "Success";
                    }
                    else
                    {
                        return "Internal Server Error. Please try again.";
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return "Internal Server Error. Please try again.";
                }
            }
            else
            {
                return "Internet Connectivity error. Please try again.";
            }

        }


        public async Task<string> GetAccessToken()
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                String url = "https://graph.facebook.com/v3.2/oauth/access_token?client_id=1699986470106189&redirect_uri=https://www.facebook.com/connect/login_success.html&client_secret=8eec9d2368947acef2839159f6410863&code=" + App.access_code;

                var uri = new Uri(url);

                try
                {
                    HttpResponseMessage response = null;
                    response = await client.GetAsync(uri);


                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        FacebookAccessTokenModel facebookAccessTokenModel = JsonConvert.DeserializeObject<FacebookAccessTokenModel>(responseJson);

                        return facebookAccessTokenModel.AccessToken;
                    }
                    else
                    {
                        return "Internal Server Error. Please try again.";
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return "Internal Server Error. Please try again.";
                }
            }
            else
            {
                return "Internet Connectivity error. Please try again.";
            }

        }


        public async Task<TrainerListModel> FetchTrainerList()
        {
            TrainerListModel trainerListModel = new TrainerListModel();

            if (CrossConnectivity.Current.IsConnected)
            {
                var uri = new Uri("http://104.238.81.169:4080/FitnessApp/manageservices/v1/members/trainerlist");

                try
                {
                    var content = new StringContent("{}", Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    response = await client.PostAsync(uri, content);


                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        trainerListModel = JsonConvert.DeserializeObject<TrainerListModel>(responseJson);

                        return trainerListModel;
                    }
                    else
                    {
                        return trainerListModel;
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return trainerListModel;
                }
            }
            else
            {
                return trainerListModel;
            }

        }


        public async Task<string> SignUpAsync(TraineeSignupModel signupModel)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                var uri = new Uri("http://104.238.81.169:4080/FitnessApp/manageservices/v1/members/signup");

                try
                {
                    var json = JsonConvert.SerializeObject(signupModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    response = await client.PostAsync(uri, content);


                    if (response.IsSuccessStatusCode)
                    {

                        LoginRequestModel loginRequestModel = new LoginRequestModel("App", signupModel.email, App.SelectedView, signupModel.password);
                        var message = await App.TodoManager.Login(loginRequestModel);

                        if (message == "Success")
                        {
                            return "Success";
                        }
                        else
                        {
                            return "Internal Server Error. Please try again.";
                        }

                    }
                    else
                    {
                        return "Internal Server Error. Please try again.";
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return "Internal Server Error. Please try again.";
                }
            }
            else
            {
                return "Internet Connectivity error. Please try again.";
            }

        }

        public async Task<string> LoginAsync(LoginRequestModel loginRequestModel)
        {
            if (CrossConnectivity.Current.IsConnected)
            {

                var uri = new Uri("http://104.238.81.169:4080/FitnessApp/manageservices/v1/members/validateMember");
                //var uri = new Uri("https://trainmeapp.in:8443/FitnessApp/manageservices/v1/members/validateMember");
                try
                {
                    var json = JsonConvert.SerializeObject(loginRequestModel);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    response = await client.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        LoginResponse responseItem = JsonConvert.DeserializeObject<Models.LoginResponse>(responseJson);

                        foreach (var item in responseItem.member)
                        {
                            if (item.Profile == App.SelectedView)
                            {
                                App.LoginResponse = item;
                            }
                            else if (loginRequestModel.password == "qwertyqazxcvbnm" && item.Profile == "Trainer")
                            {
                                App.TrainerData = item;
                            }
                        }

                        return "Success";

                    }
                    else
                    {
                        return "Internal Server Error. Please try again.";
                    }


                }
                catch (Exception ex)
                {
                    Debug.WriteLine(@"ERROR {0}", ex.Message);
                    return "Internal Server Error. Please try again.";
                }
            }
            else
            {
                return "Internet Connectivity error. Please try again.";
            }

        }
        /*
         * Not implementated methods
        */

        public LoginResponse.SyncLoginResponse LoginResponse(string SelectedInput)
        {
            throw new NotImplementedException();
        }

        public async Task<SetTimeSlotResponseModel> SetCalenderEvent(SetTimeSlotsRequestModel setTimeSlots)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    var uri = new Uri("http://104.238.81.169:4080/FitnessApp/manageservices/v1/members/addTimeSlots");
                    var json = JsonConvert.SerializeObject(setTimeSlots);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    response = await client.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<SetTimeSlotResponseModel>(responseJson);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }

        public async Task<GetTimeSlotResponse> GetTimeSlots(GetTimeSlotRequest getTimeSlotRequest)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    var uri = new Uri("http://104.238.81.169:4080/FitnessApp/manageservices/v1/members/getTimeSlots");
                    var json = JsonConvert.SerializeObject(getTimeSlotRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    response = await client.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<GetTimeSlotResponse>(responseJson);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }


        public async Task<PaymentResponse> StartPayment(PaymentRequest paymentRequest)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                try
                {
                    var uri = new Uri("http://104.238.81.169:4080/FitnessApp/manageservices/v1/members/startPayment");
                    var json = JsonConvert.SerializeObject(paymentRequest);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = null;

                    response = await client.PostAsync(uri, content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = response.Content.ReadAsStringAsync().Result;
                        var result = JsonConvert.DeserializeObject<PaymentResponse>(responseJson);
                        return result;
                    }
                }
                catch (Exception ex)
                {
                }
            }
            return null;
        }
    }
}