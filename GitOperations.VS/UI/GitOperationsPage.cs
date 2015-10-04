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
    [TeamExplorerPage(GitOperationsGuids.GitOperationsPage, Undockable = true)]
    public class GitOperationsPage : TeamExplorerBasePage
    {
        private static IGitExt gitService;
        private static ITeamExplorer teamExplorer;
        private static IVsOutputWindowPane outputWindow;
        private readonly GitOperationsPageUI ui;

        [ImportingConstructor]
        public GitOperationsPage([Import(typeof (SVsServiceProvider))] IServiceProvider serviceProvider)
        {
            Title = "Git Operations";
            gitService = (IGitExt) serviceProvider.GetService(typeof (IGitExt));
            teamExplorer = (ITeamExplorer) serviceProvider.GetService(typeof (ITeamExplorer));
            gitService.PropertyChanged += OnGitServicePropertyChanged;

            var outWindow = Package.GetGlobalService(typeof (SVsOutputWindow)) as IVsOutputWindow;
            var customGuid = new Guid("A7C0163F-82E8-4D95-97C8-C62743D15DC3");
            outWindow.CreatePane(ref customGuid, "Git Operations", 1, 1);
            outWindow.GetPane(ref customGuid, out outputWindow);


            ui = new GitOperationsPageUI();
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