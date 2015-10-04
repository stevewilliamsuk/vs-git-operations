using GitOperations.VS.Service;
using Microsoft.VisualStudio.Shell.Interop;

namespace GitOperations.VS
{
    public class VsGitWrapper : GitWrapper
    {
        public VsGitWrapper(string repoPath, IVsOutputWindowPane outputWindow) : base(repoPath)
        {
            CommandOutputDataReceived += (o, args) => outputWindow.OutputStringThreadSafe(args.Output);
            CommandErrorDataReceived += (o, args) => outputWindow.OutputStringThreadSafe(args.Output);
        }
    }
}