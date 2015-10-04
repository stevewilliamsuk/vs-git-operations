using System.Windows.Controls;
using GitOperations.VS.UI.ViewModels;

namespace GitOperations.VS.UI
{
    /// <summary>
    ///     Interaction logic for GitOperationsPageUI.xaml
    /// </summary>
    public partial class GitOperationsPageUI : UserControl
    {
        private readonly GitOperationsViewModel model;

        public GitOperationsPageUI()
        {
            InitializeComponent();
            model = new GitOperationsViewModel();

            DataContext = model;
        }

        public void Refresh()
        {
            model.Update();
        }
    }
}