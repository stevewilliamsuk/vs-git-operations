using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;

namespace GitOperations.VS.UI.ViewModels
{
    public class GitSubmodulesViewModel
    {
        public GitSubmodulesViewModel()
        {
            InitialiseSubmodulesCommand = new RelayCommand(x => InitialiseSubmodules());
            UpdateSubmodulesCommand = new RelayCommand(x => UpdateSubmodules());
        }

        public ICommand InitialiseSubmodulesCommand { get; private set; }
        public ICommand UpdateSubmodulesCommand { get; private set; }

        public void InitialiseSubmodules()
        {
            var wrapper = new VsGitWrapper(GitSubmodulesPage.ActiveRepoPath, GitSubmodulesPage.OutputWindow);
            wrapper.InitialiseSubmodules();
        }

        public void UpdateSubmodules()
        {
            var wrapper = new VsGitWrapper(GitSubmodulesPage.ActiveRepoPath, GitSubmodulesPage.OutputWindow);
            wrapper.UpdateSubmodules();
        }

        public void Update()
        {
        }
    }
}