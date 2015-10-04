using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Microsoft.TeamFoundation.Controls;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.VisualStudio.TeamFoundation.Git.Extensibility;
using TeamExplorer.Common;

namespace GitOperations.VS.UI
{
    [TeamExplorerPage(GitOperationsGuids.GitSubmodulesPage, Undockable = true)]
    public class GitSubmodulesPage : TeamExplorerBasePage
    {
        private static IGitExt gitService;
        private static ITeamExplorer teamExplorer;
        private static IVsOutputWindowPane outputWindow;
        private readonly GitSubmodulePageUI ui;

        [ImportingConstructor]
        public GitSubmodulesPage([Import(typeof (SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            Title = "Git Submodules";
            gitService = (IGitExt) serviceProvider.GetService(typeof (IGitExt));
            teamExplorer = (ITeamExplorer) serviceProvider.GetService(typeof (ITeamExplorer));
            gitService.PropertyChanged += OnGitServicePropertyChanged;

            var outWindow = Package.GetGlobalService(typeof (SVsOutputWindow)) as IVsOutputWindow;
            var customGuid = new Guid("08A48010-2A4A-4DB1-AA58-36674F667904");
            outWindow.CreatePane(ref customGuid, "Git Submodules", 1, 1);
            outWindow.GetPane(ref customGuid, out outputWindow);


            ui = new GitSubmodulePageUI();
            PageContent = ui;
        }

        public static IGitRepositoryInfo ActiveRepo
        {
            get { return gitService.ActiveRepositories.FirstOrDefault(); }
        }

        public static IVsOutputWindowPane OutputWindow
        {
            get { return outputWindow; }
        }

        public static string ActiveRepoPath
        {
            get { return ActiveRepo.RepositoryPath; }
        }

        public override void Refresh()
        {
            ui.Refresh();
        }

        private void OnGitServicePropertyChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
        {
            Refresh();
        }

        public static void ActiveOutputWindow()
        {
            OutputWindow.Activate();
        }

        public static void ShowPage(string page)
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background,
                new Action(() =>
                    teamExplorer.NavigateToPage(new Guid(page), null)));
        }
    }
}