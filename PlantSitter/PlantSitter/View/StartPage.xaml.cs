using PlantSitter.Common;
using PlantSitter.ViewModel;


namespace PlantSitter.View
{
    public sealed partial class StartPage : BasePage
    {
        public StartViewModel StartVM { get; set; }

        public StartPage()
        {
            this.InitializeComponent();
            this.DataContext = StartVM = new StartViewModel();
        }
    }
}
