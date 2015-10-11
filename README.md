# vs-git-operations
Plugin for Visual Studio (Team Explorer) that allows you to do some basic operations and work with Submodules.

I'm sorry the documentation is so sparse at this stage. I wrote this plugin in about 10 hours because I really need my team at work to start using submodules to save us all a massive headache with our core platform being used in so many projects.

It's basically stuff I think is missing from Visual Studio Team Explorer.

There's a reason most of the commands run in a Window - that's because when I tested them it wanted username/password and there doesn't seem to be a good way to "listen" for this and handle nicely. I tried. I tried for about 4 hours!

<b>Support</b>

Supports VS 2015 only.

<b>Current Features</b>

Initialise and pull-down your Submodules

Pull all remote branches and prune off any old tracked branches

<b>Usage</b>

Install the VSIX into Visual Studio

Go to Team Explorer

Provided you are in an active Git repo, the "Git Submodule" and "Git Operations" buttons should appear.

<b>Roadmap</b>

VS 2013 support

"Push All Tracked Branches" operational button

Add facility under Submodules

Try and migrate the git command runners to not need to run in a window
