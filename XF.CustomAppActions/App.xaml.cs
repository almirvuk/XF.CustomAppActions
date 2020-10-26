using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XF.CustomAppActions.Services;
using XF.CustomAppActions.Views;

namespace XF.CustomAppActions
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new MainPage();

            // Subscribe for and event
            AppActions.OnAppAction += AppActions_OnAppAction;
        }

        void AppActions_OnAppAction(object sender, AppActionEventArgs e)
        {
            // Don't handle events fired for old application instances
            // and cleanup the old instance's event handler
            if (Application.Current != this && Application.Current is App app)
            {
                AppActions.OnAppAction -= app.AppActions_OnAppAction;
                return;
            }

            Device.BeginInvokeOnMainThread(async () =>
            {
                Page targetPage = null;

                // Determine the app action type or id which user selected
                switch (e.AppAction.Id)
                {
                    case Helpers.ActionType.AddNote:
                        targetPage = new NewItemPage();
                        break;

                    case Helpers.ActionType.ViewFavourites:
                        // targetPage = new FavouritesPage();
                        break;

                    // Other id cases:

                    default:
                        break;
                }

                // Handle the navigation to target page
                if (targetPage != null)
                {
                    await Current.MainPage.Navigation.PushModalAsync(new NavigationPage(new NewItemPage()));
                }
            });
        }

        protected override async void OnStart()
        {
            try
            {
                await AppActions.SetAsync(
                    new AppAction(id: Helpers.ActionType.AddNote, title: "Add new note", icon: "icon.png"),
                    new AppAction(id: Helpers.ActionType.ViewFavourites, title: "View favourites", icon: "icon.png"));
            }
            catch (FeatureNotSupportedException ex)
            {
                // Log error and handle this Exception properly
            }
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
