using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using LibGit2Sharp;

namespace GitOperations.VS.Service
{
    public class GitWrapper
    {
        public delegate void CommandErrorReceivedEventHandler(object sender, CommandOutputEventArgs args);

        public delegate void CommandOutputReceivedEventHandler(object sender, CommandOutputEventArgs args);

        private const string DefaultValueRegExp = @"\[(.*?)\]";
        public static StringBuilder Output = new StringBuilder("");
        public static StringBuilder Error = new StringBuilder("");

        private readonly string repoDirectory;

        public GitWrapper(string repoDirectory)
        {
            this.repoDirectory = repoDirectory;
        }

        public string CurrentBranch
        {
            get
            {
                using (var repo = new Repository(repoDirectory))
                {
                    return repo.Head.Name;
                }
            }
        }

        public event CommandOutputReceivedEventHandler CommandOutputDataReceived;
        public event CommandErrorReceivedEventHandler CommandErrorDataReceived;

        public GitCommandResult Pull(IEnumerable<string> options = null)
        {
            if (options == null)
            {
                options = Enumerable.Empty<string>();
            }
            options = options.Select(x => " --" + x);
            var gitArguments = "pull" + string.Join(string.Empty, options);
            return RunGitCommandExternal(gitArguments);
        }

        public GitCommandResult Push()
        {
            var gitArguments = "push";
            return RunGitCommandExternal(gitArguments);
        }

        public GitCommandResult InitialiseSubmodules()
        {
            var gitArguments = "submodule init";
            return RunGitCommand(gitArguments);
        }

        public GitCommandResult UpdateSubmodules()
        {
            var gitArguments = "submodule update";
            return RunGitCommandExternal(gitArguments);
        }

        protected virtual void OnCommandOutputDataReceived(CommandOutputEventArgs e)
        {
            var handler = CommandOutputDataReceived;
            if (handler != null) handler(this, e);
        }

        protected virtual void OnCommandErrorDataReceived(CommandOutputEventArgs e)
        {
            var handler = CommandErrorDataReceived;
            if (handler != null) handler(this, e);
        }

        private static Process CreateGitProcess(string arguments, string repoDirectory)
        {
            var gitInstallationPath = GitHelper.GetGitInstallationPath();
            var pathToGit = Path.Combine(Path.Combine(gitInstallationPath, "bin\\git.exe"));
            return new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    FileName = pathToGit,
                    Arguments = arguments,
                    WorkingDirectory = repoDirectory
                }
            };
        }

        private static Process CreateExternalGitProcess(string arguments, string repoDirectory)
        {
            var gitInstallationPath = GitHelper.GetGitInstallationPath();
            var pathToGit = Path.Combine(Path.Combine(gitInstallationPath, "bin\\git.exe"));
            return new Process
            {
                StartInfo =
                {
                    CreateNoWindow = false,
                    UseShellExecute = true,
                    FileName = pathToGit,
                    Arguments = arguments,
                    WorkingDirectory = repoDirectory
                }
            };
        }


        private void OnOutputDataReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs.Data != null)
            {
                Output.Append(dataReceivedEventArgs.Data);
                Debug.WriteLine(dataReceivedEventArgs.Data);
                OnCommandOutputDataReceived(new CommandOutputEventArgs(dataReceivedEventArgs.Data + Environment.NewLine));
            }
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs dataReceivedEventArgs)
        {
            if (dataReceivedEventArgs.Data != null &&
                dataReceivedEventArgs.Data.StartsWith("fatal:", StringComparison.OrdinalIgnoreCase))
            {
                Error = new StringBuilder();
                Error.Append(dataReceivedEventArgs.Data);
                Debug.WriteLine(dataReceivedEventArgs.Data);
                OnCommandErrorDataReceived(new CommandOutputEventArgs(dataReceivedEventArgs.Data + Environment.NewLine));
            }
        }

        private GitCommandResult RunGitCommandExternal(string gitArguments)
        {
            Error = new StringBuilder("");

            using (var p = CreateExternalGitProcess(gitArguments, repoDirectory))
            {
                OnCommandOutputDataReceived(
                    new CommandOutputEventArgs("Running git " + p.StartInfo.Arguments + Environment.NewLine));
                p.Start();
                return new GitCommandResult(true, "Launched in external window");
            }
        }

        private GitCommandResult RunGitCommand(string gitArguments)
        {
            Error = new StringBuilder("");
            Output = new StringBuilder("");

            using (var p = CreateGitProcess(gitArguments, repoDirectory))
            {
                OnCommandOutputDataReceived(
                    new CommandOutputEventArgs("Running git " + p.StartInfo.Arguments + Environment.NewLine));
                p.Start();
                p.ErrorDataReceived += OnErrorReceived;
                p.OutputDataReceived += OnOutputDataReceived;
                p.BeginErrorReadLine();
                p.BeginOutputReadLine();
                p.WaitForExit(15000);
                if (!p.HasExited)
                {
                    OnCommandOutputDataReceived(
                        new CommandOutputEventArgs("The command is taking longer than expected\n"));

                    p.WaitForExit(15000);
                    if (!p.HasExited)
                    {
                        return new GitTimedOutCommandResult("git " + p.StartInfo.Arguments);
                    }
                }
                if (Error != null && Error.Length > 0)
                {
                    return new GitCommandResult(false, Error.ToString());
                }
                return new GitCommandResult(true, Output.ToString());
            }
        }
    }
}