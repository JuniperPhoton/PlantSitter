using PlantSitter.Common;
using PlantSitter.ViewModel;


namespace PlantSitter.View
{
    public sealed partial class RegisterLoginPage : BasePage
    {
        public LoginViewModel LoginVM { get; set; }

        public RegisterLoginPage()
        {
            this.InitializeComponent();
            this.DataContext = LoginVM = new LoginViewModel();
        }
    }
}
