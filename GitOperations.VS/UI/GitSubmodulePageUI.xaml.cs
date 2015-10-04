using System.Windows.Controls;
using GitOperations.VS.UI.ViewModels;

namespace GitOperations.VS.UI
{
    /// <summary>
    ///     Interaction logic for GitSubmodulePageUI.xaml
    /// </summary>
    public partial class GitSubmodulePageUI : UserControl
    {
        private readonly GitSubmodulesViewModel model;

        public GitSubmodulePageUI()
        {
            InitializeComponent();
            model = new GitSubmodulesViewModel();

            DataContext = model;
        }

        public void Refresh()
        {
            model.Update();
        }
    }
}