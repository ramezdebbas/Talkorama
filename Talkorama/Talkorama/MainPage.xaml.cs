using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.ApplicationSettings;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Talkorama
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            SettingsPane.GetForCurrentView().CommandsRequested += MainPage_CommandsRequested;
        }

        void MainPage_CommandsRequested(SettingsPane sender, SettingsPaneCommandsRequestedEventArgs args)
        {
            bool afound = false;
            bool sfound = false;
            bool pfound = false;
            foreach (var command in args.Request.ApplicationCommands.ToList())
            {
                if (command.Label == "About")
                {
                    afound = true;
                }
                if (command.Label == "Settings")
                {
                    sfound = true;
                }
                if (command.Label == "Policy")
                {
                    pfound = true;
                }
            }
            if (!afound)
                args.Request.ApplicationCommands.Add(new SettingsCommand("s", "About", (p) => { cfoAbout.IsOpen = true; }));
            //if (!sfound)
            //    args.Request.ApplicationCommands.Add(new SettingsCommand("s", "Settings", (p) => { cfoSettings.IsOpen = true; }));
            //if (!pfound)
            //    args.Request.ApplicationCommands.Add(new SettingsCommand("s", "Policy", (p) => { cfoPolicy.IsOpen = true; }));
            args.Request.ApplicationCommands.Add(new SettingsCommand("privacypolicy", "Privacy policy", OpenPrivacyPolicy));
        }

        private async void OpenPrivacyPolicy(IUICommand command)
        {
            var uri = new Uri("http://www.thatslink.com/privacy-statment/ ");
            await Launcher.LaunchUriAsync(uri);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void BtnTalk_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            
        }

        public string Talk(string value)
        {
            var result = Post("http://bot.devride.com/talk.aspx?message=" + value, null);

            return result;
        }

        public string Post(string url, IDictionary<string, object> parameters)
        {

            //string postData = ParametersToWWWFormURLEncoded(parameters);

            //byte[] postDataBytes = Encoding.UTF8.GetBytes(postData);

            WebRequest webRequest = WebRequest.Create(url);

            webRequest.Method = "GET";

            //webRequest.ContentType = "application/x-www-form-urlencoded";       

            //Stream outputStream = await webRequest.GetRequestStreamAsync();

            //outputStream.Write(postDataBytes, 0, postDataBytes.Length);           

            var response = webRequest.GetResponseAsync();

            StreamReader responseStreamReader = new StreamReader(response.Result.GetResponseStream());

            return responseStreamReader.ReadToEnd().Trim();

        }

        public static string ParametersToWWWFormURLEncoded(IDictionary<string, object> parameters)
        {

            string wwwFormUrlEncoded = null;

            foreach (string parameterKey in parameters.Keys)
            {

                string parameterValue = parameters[parameterKey].ToString();

                string parameter = string.Format("{0}={1}", System.Net.WebUtility.UrlEncode(parameterKey), System.Net.WebUtility.UrlEncode(parameterValue));

                if (wwwFormUrlEncoded == null)
                {
                    wwwFormUrlEncoded = parameter;
                }
                else
                {
                    wwwFormUrlEncoded = string.Format("{0}&{1}", wwwFormUrlEncoded, parameter);
                }

            }

            return wwwFormUrlEncoded;

        }

        private void BtnTalk_OnClick(object sender, RoutedEventArgs e)
        {
            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                var result = Talk(Message.Text);
                Message.Text = "";
                taloramasays.Text = result;
                Message.Focus(FocusState.Pointer);
               
            });
        }
    }
}
