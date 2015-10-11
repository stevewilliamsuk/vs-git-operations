using System.Collections.Generic;
using System.Windows.Input;
using Microsoft.TeamFoundation.MVVM;

namespace GitOperations.VS.UI.ViewModels
{
    public class GitOperationsViewModel
    {
        public GitOperationsViewModel()
        {
            PullCommand = new RelayCommand(x => GitPull());
            PushCommand = new RelayCommand(x => GitPush());
        }

        public ICommand PullCommand { get; private set; }
        public ICommand PushCommand { get; private set; } 

        public void GitPull()
        {
            var wrapper = new VsGitWrapper(GitOperationsPage.ActiveRepoPath, GitOperationsPage.OutputWindow);
            List<string> options = new List<string>()
            {
                {"all"}
            };

            if (PruneOption)
            {
                options.Add("prune");
            }

            wrapper.Pull(options);
        }

        public void GitPush()
        {
            var wrapper = new VsGitWrapper(GitOperationsPage.ActiveRepoPath, GitOperationsPage.OutputWindow);
            wrapper.Push();
        }

        public bool PruneOption { get; set; }

        public void Update()
        {
        }
    }
}