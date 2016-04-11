using PlantSitter.Common;
using PlantSitter.ViewModel;
using Windows.UI.Xaml.Navigation;

namespace PlantSitter.View
{
    public sealed partial class MainPage : BasePage
    {
        public MainViewModel MainVM { get; set; }

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = MainVM = new MainViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            NavigationService.RootFrame.BackStack.Clear();
        }
    }
}
