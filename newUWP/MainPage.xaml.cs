using Windows.Media.Audio;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace newUWPAppServiceTest
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a <see cref="Frame">.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
        }
        private async void MyButton_OnClick(object sender, RoutedEventArgs e)
        {
            var resp = await AppServiceClient.GetLicenseInfo(SpatialAudioFormatSubtype.DolbyAtmosForHeadphones, null);
            if (resp != null)
                AppServiceStatus.Text = resp["Status"].ToString();
            else
            {
                AppServiceStatus.Text = "No response";
            }
        }
    }
}
