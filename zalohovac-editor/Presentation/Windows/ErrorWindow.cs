using GradeRegisterAdminApp.Presentation.Components;

namespace GradeRegisterAdminApp.Presentation.Windows
{
    public class ErrorWindow : BaseWindow
    {
        private Label _errorLabel;
        private Button _okButton;

        public ErrorWindow(string title, string error, Application application, IWindow returnWindow)
            : base(title, application, returnWindow)
        {
            _errorLabel = new Label(error);
            _okButton = new Button("Ok", true);

            RegisterComponent(_errorLabel);
            RegisterComponent(new EmptyLine());
            RegisterComponent(_okButton);

            _okButton.Clicked += Close;
        }
    }
}
