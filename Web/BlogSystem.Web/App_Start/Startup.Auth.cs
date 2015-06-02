namespace BlogSystem.Web
{
    using System;
    using System.Security.Claims;

    using BlogSystem.Data;
    using BlogSystem.Models;
    using BlogSystem.Web.Models;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.Owin;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Cookies;
    using Microsoft.Owin.Security.Facebook;
    using Microsoft.Owin.Security.Google;

    using Owin;

    public partial class Startup
    {
        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureAuth(IAppBuilder app)
        {
            // Configure the db context, user manager and signin manager to use a single instance per request
            app.CreatePerOwinContext(BlogSystemDbContext.Create);
            app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            // Configure the sign in cookie
            app.UseCookieAuthentication(new CookieAuthenticationOptions
            {
                AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
                LoginPath = new PathString("/Account/Login"),
                Provider = new CookieAuthenticationProvider
                {
                    // Enables the application to validate the security stamp when the user logs in.
                    // This is a security feature which is used when you change a password or add an external login to your account.  
                    OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, User>(
                        validateInterval: TimeSpan.FromMinutes(30),
                        regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
                }
            });            
            app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            // Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
            // This is similar to the RememberMe option when you log in.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

            // Uncomment the following lines to enable logging in with third party login providers
            // app.UseMicrosoftAccountAuthentication(
            // clientId: "",
            // clientSecret: "");

            // app.UseTwitterAuthentication(
            // consumerKey: "",
            // consumerSecret: "");

            // app.UseFacebookAuthentication(
            // appId: "849267775181018",
            // appSecret: "f0c47ff020fc16ce1fce05c209cafea7");
            var facebookOptions = new Microsoft.Owin.Security.Facebook.FacebookAuthenticationOptions()
            {
                AppId = "1578306832421623",
                AppSecret = "dde1d4f4845b60cb7912da5d80f01936"
            };
            facebookOptions.Scope.Add("email");
            facebookOptions.Scope.Add("public_profile");
            app.UseFacebookAuthentication(facebookOptions);

            // app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
            // {
            // ClientId = "412149234571-i6g8ucnud004l6bl77gms7um3tvavnre.apps.googleusercontent.com",
            // ClientSecret = "gYjHheyuK15SpoQKUSnjmqpb"
            // });
            var googleOptions = new Microsoft.Owin.Security.Google.GoogleOAuth2AuthenticationOptions()
            {
                ClientId = "412149234571-i6g8ucnud004l6bl77gms7um3tvavnre.apps.googleusercontent.com",
                ClientSecret = "gYjHheyuK15SpoQKUSnjmqpb",
                CallbackPath = new PathString("/signin-google"),
            };
            googleOptions.Scope.Add("email");
            googleOptions.Scope.Add("profile");
            app.UseGoogleAuthentication(googleOptions);
        }
    }
}