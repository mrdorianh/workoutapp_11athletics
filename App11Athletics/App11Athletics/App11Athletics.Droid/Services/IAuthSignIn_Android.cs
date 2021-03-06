using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using App11Athletics.Droid;
using App11Athletics.Droid.Services;
using App11Athletics.Helpers;
using App11Athletics.Models;
using Auth0.SDK;
using Xamarin.Forms;
using Xamarin.Auth;
using Xamarin.Android;
using Xamarin.Forms.Platform.Android;
using Application = Xamarin.Forms.Application;

[assembly: Xamarin.Forms.Dependency(typeof(IAuthSignIn_Android))]

namespace App11Athletics.Droid.Services
{
    public class IAuthSignIn_Android : IAuthSignIn
    {
        public Auth0Client client = new Auth0.SDK.Auth0Client("app11athletics.auth0.com",
            "yPZw5Yv1WZbLdASc6Rkaz2Ib9e4iDqGW");

        public IAuthSignIn_Android() { }

        #region Implementation of IAuthSignIn

        public async Task AuthSignIn()
        {
            var context = Forms.Context as Activity;
            try
            {
                await this.client.LoginAsync(context, withRefreshToken: true);
                var user = this.client.CurrentUser;
                if (!string.IsNullOrEmpty(user.RefreshToken))
                {
                    App.IsUserLoggedIn = true;
                    await SaveUserAttributes(user);
                }
                else
                    App.IsUserLoggedIn = false;
            }
            catch (Exception exception)
            {
                Settings.UserRefreshToken = string.Empty;
            }
        }

        public async Task AuthLogOut()
        {
            App.IsUserLoggedIn = false;
            Settings.UserRefreshToken = string.Empty;
            await Task.Run(() => { client.Logout(); });
        }

        public async Task AuthRefresh()
        {
            var context = Forms.Context as Activity;
            try
            {
                var rt = Settings.UserRefreshToken;
                await client.RefreshToken(rt);
                var user = this.client.CurrentUser;
                if (!string.IsNullOrEmpty(user?.RefreshToken))
                {
                    App.IsUserLoggedIn = true;
                    await SaveUserAttributes(user);
                }
                else
                    App.IsUserLoggedIn = false;
            }
            catch (Exception e)
            {
                try
                {
                    await this.client.LoginAsync(context, withRefreshToken: true);
                    var user = this.client.CurrentUser;
                    if (!string.IsNullOrEmpty(user.RefreshToken))
                    {
                        App.IsUserLoggedIn = true;
                        await SaveUserAttributes(user);
                    }
                    else
                        App.IsUserLoggedIn = false;
                }
                catch (Exception exception)
                {
                    Settings.UserRefreshToken = string.Empty;
                }
            }
        }

        #endregion

        public Task SaveUserAttributes(Auth0User user)
        {

            Settings.UserRefreshToken = user.RefreshToken;
            Settings.UserEmail = user.Profile["email"]?.ToString();
            var t = user.Profile["identities"];
            var c = t.Children();
            Settings.UserProvider = c["provider"]?.ToString();
            Settings.UserGivenName = user.Profile["given_name"]?.ToString();
            Settings.UserFamilyName = user.Profile["family_name"]?.ToString();
            Settings.UserName = user.Profile["name"]?.ToString();
            string pic = user.Profile["picture_large"]?.ToString();
            if (string.IsNullOrEmpty(pic))
                pic = user.Profile["picture"]?.ToString();
            Settings.UserPictureOriginal = pic;
            if (string.IsNullOrEmpty(Settings.UserPicture) || (Settings.UserPicture == "iconbevel.png"))
                Settings.UserPicture = pic;
            if (string.IsNullOrEmpty(Settings.UserAge))
                Settings.UserAge = user.Profile["age"]?.ToString();
            if (string.IsNullOrEmpty(Settings.UserGender))
                Settings.UserGender = user.Profile["gender"]?.ToString();
            return Task.CompletedTask;
        }
    }
}