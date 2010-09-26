﻿/***************************************************************************

Copyright (c) Microsoft Corporation. All rights reserved.
This code is licensed under the Visual Studio SDK license terms.
THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

***************************************************************************/

using System;
using System.ComponentModel.Design;
using System.Runtime.InteropServices;
using Microsoft.ClojureExtension;
using Microsoft.ClojureExtension.Repl;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Microsoft.VisualStudio.Project.Samples.CustomProject
{
    [Guid(PackageGuid)]
    [PackageRegistration(UseManagedResourcesOnly = true)]
    [DefaultRegistryRoot("Software\\Microsoft\\VisualStudio\\10.0")]
    [ProvideObject(typeof (GeneralPropertyPage))]
    [ProvideProjectFactory(typeof (ClojureProjectFactory), "Clojure", "Clojure Project Files (*.cljproj);*.cljproj", "cljproj", "cljproj", @"Templates\Projects\Clojure", LanguageVsTemplate = "Clojure", NewProjectRequireNewFolderVsTemplate = false)]
    [ProvideProjectItem(typeof (ClojureProjectFactory), "Clojure Items", @"Templates\ProjectItems\Clojure", 500)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    [ProvideToolWindow(typeof (ReplToolWindow))]
    public sealed class ClojurePackage : ProjectPackage
    {
        public const string PackageGuid = "40953a10-3425-499c-8162-a90059792d13";

        protected override void Initialize()
        {
            base.Initialize();
            intializeMenuItems();
            RegisterProjectFactory(new ClojureProjectFactory(this));
        }

        private void intializeMenuItems()
        {
            OleMenuCommandService mcs = GetService(typeof (IMenuCommandService)) as OleMenuCommandService;

            mcs.AddCommand(
                new MenuCommand(
                    menuItemClick,
                    new CommandID(GuidList.GuidClojureExtensionCmdSet, ClojurePackageCommandId.LoadFileIntoActiveRepl)));

            mcs.AddCommand(
                new MenuCommand(
                    menuItemClick,
                    new CommandID(GuidList.GuidClojureExtensionCmdSet, ClojurePackageCommandId.LoadProjectIntoActiveRepl)));

            mcs.AddCommand(
                new MenuCommand(
                    menuItemClick,
                    new CommandID(GuidList.GuidClojureExtensionCmdSet, ClojurePackageCommandId.StartReplUsingProjectVersion)));
        }

        private void menuItemClick(object sender, EventArgs args)
        {
            ReplToolWindow window = (ReplToolWindow) FindToolWindow(typeof (ReplToolWindow), 0, true);
            IVsWindowFrame windowFrame = (IVsWindowFrame) window.Frame;
            ErrorHandler.ThrowOnFailure(windowFrame.Show());
            window.CreateNewRepl();
        }

        public override string ProductUserContext
        {
            get { return "ClojureProj"; }
        }
    }
}