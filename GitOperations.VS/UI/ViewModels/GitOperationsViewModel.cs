using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;

namespace GitOperations.VS.UI.ViewModels
{
    public class GitOperationsViewModel
    {
        public GitOperationsViewModel()
        {
            PullAndPruneCommand = new RelayCommand(x => PullAndPrune());
        }

        public ICommand PullAndPruneCommand { get; private set; }

        public void PullAndPrune()
        {
            var wrapper = new VsGitWrapper(GitOperationsPage.ActiveRepoPath, GitOperationsPage.OutputWindow);
            wrapper.PullAndPrune();
        }

        public void Update()
        {
        }
    }
}