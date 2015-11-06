using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using NUnit.Framework;
using dotNetUnitTestsRunner;
using System.Windows.Automation;
using System.Diagnostics;
using System.Threading;
using White.Core;
using White.Core.Factory;
using White.Core.UIItems;
using White.Core.UIItems.WindowItems;
using White.Core.UIItems.WindowStripControls;
using White.Core.UIItems.MenuItems;
using White.Core.UIItems.TreeItems;
using White.Core.UIItems.Finders;
using White.Core.WindowsAPI;

namespace InstallerEditorUnitTests
{
    [TestFixture]
    public class UIUnitTests : EnUsUnitTests
    {
        [Test]
        public void TestMainMenu()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                Menus mainMenu = UIAutomation.Find<MenuBar>(mainWindow, "Application").TopLevelMenu;
                Assert.AreEqual(5, mainMenu.Count);
                Assert.AreEqual("File", mainMenu[0].Name);
                Assert.AreEqual("Edit", mainMenu[1].Name);
                Assert.AreEqual("View", mainMenu[2].Name);
                Assert.AreEqual("Tools", mainMenu[3].Name);
                Assert.AreEqual("Help", mainMenu[4].Name);
            }
        }

        [Test]
        public void TestMainMenuFile()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                Menu mainMenuFile = UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("File");
                mainMenuFile.Click();
                Assert.IsTrue(mainMenuFile.ChildMenus.Find("New").Enabled);
                Assert.IsTrue(mainMenuFile.ChildMenus.Find("Open...").Enabled);
                Assert.IsFalse(mainMenuFile.ChildMenus.Find("Close").Enabled);
                Assert.IsFalse(mainMenuFile.ChildMenus.Find("Save").Enabled);
                Assert.IsFalse(mainMenuFile.ChildMenus.Find("Save As...").Enabled);
                Assert.IsFalse(mainMenuFile.ChildMenus.Find("Close").Enabled);
                Assert.IsFalse(mainMenuFile.ChildMenus.Find("Edit With Notepad").Enabled);
                Assert.IsFalse(mainMenuFile.ChildMenus.Find("Create Exe...").Enabled);
                Assert.IsTrue(mainMenuFile.ChildMenus.Find("Exit").Enabled);
            }
        }

        [Test]
        public void TestMainMenuEdit()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                Menu mainMenuEdit = UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit");
                mainMenuEdit.Click();
                Assert.IsFalse(mainMenuEdit.ChildMenus.Find("Add").Enabled);
                Assert.IsFalse(mainMenuEdit.ChildMenus.Find("Move").Enabled);
                Assert.IsFalse(mainMenuEdit.ChildMenus.Find("Expand All").Enabled);
                Assert.IsFalse(mainMenuEdit.ChildMenus.Find("Collapse All").Enabled);
                Assert.IsFalse(mainMenuEdit.ChildMenus.Find("Delete").Enabled);
            }
        }

        [Test]
        public void TestMainMenuView()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                Menu mainMenuView = UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("View");
                mainMenuView.Click();
                Assert.IsTrue(mainMenuView.ChildMenus.Find("Refresh").Enabled);
            }
        }

        [Test]
        public void TestMainMenuHelp()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                Menu mainMenuHelp = UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Help");
                mainMenuHelp.Click();
                Assert.IsTrue(mainMenuHelp.ChildMenus.Find("Users Guide").Enabled);
                Assert.IsTrue(mainMenuHelp.ChildMenus.Find("Home Page").Enabled);
                Assert.IsTrue(mainMenuHelp.ChildMenus.Find("About").Enabled);
            }
        }

        [Test]
        public void TestMainMenuTools()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                Menu mainMenuTools = UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Tools");
                mainMenuTools.Click();
                Assert.IsTrue(mainMenuTools.ChildMenus.Find("Template For New Item").Enabled);
                Assert.IsTrue(mainMenuTools.ChildMenus.Find("Customize Templates").Enabled);
            }
        }

        [Test]
        public void TestStatusReady()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                StatusStrip statusStrip = UIAutomation.Find<StatusStrip>(mainWindow, "statusStrip");
                WinFormTextBox statusLabel = (WinFormTextBox)statusStrip.Items[0];
                Assert.AreEqual("Ready", statusLabel.Text);
            }
        }

        [Test]
        public void DumpControls()
        {
            InstallerEditorExeUtils.RunOptions options = new InstallerEditorExeUtils.RunOptions();
            using (Process p = InstallerEditorExeUtils.Detach(options))
            {
                Thread.Sleep(2000);
                p.WaitForInputIdle();
                UIAutomation.DumpControl(AutomationElement.FromHandle(p.MainWindowHandle));
                p.CloseMainWindow();
                p.WaitForExit();
            }
        }

        [Test]
        public void TestExpandCollapseRoot()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("File", "New").Click();
                Tree configurationTree = mainWindow.Get<Tree>("configurationTree");
                TreeNode configFileNode = configurationTree.SelectedNode;
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit", "Add", "Configurations", "Setup Configuration").Click();
                configFileNode.Select();
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit", "Add", "Configurations", "Web Configuration").Click();
                configFileNode.Select();
                Assert.IsTrue(configFileNode.IsExpanded());
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit", "Collapse All").Click();
                Assert.IsFalse(configFileNode.IsExpanded());
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit", "Expand All").Click();
                Assert.IsTrue(configFileNode.IsExpanded());
            }
        }

        [Test]
        public void TestExit()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("File", "Exit").Click();
                Assert.IsTrue(mainWindow.IsClosed);
            }
        }

        [Test]
        public void TestRefresh()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("File", "New").Click();
                Tree configurationTree = mainWindow.Get<Tree>("configurationTree");
                TreeNode configFileNode = configurationTree.SelectedNode;
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit", "Add", "Configurations", "Setup Configuration").Click();
                configFileNode.Select();
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Edit", "Add", "Configurations", "Web Configuration").Click();
                configFileNode.Select();
                configFileNode.Collapse();
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("View", "Refresh").Click();
                configFileNode = configurationTree.SelectedNode;
                Assert.IsTrue(configFileNode.IsExpanded());
            }
        }

        [Test]
        public void TestHelpAbout()
        {
            using (Application installerEditor = Application.Launch(InstallerEditorExeUtils.Executable))
            {
                Window mainWindow = installerEditor.GetWindow("Installer Editor", InitializeOption.NoCache);
                UIAutomation.Find<MenuBar>(mainWindow, "Application").MenuItem("Help", "About").Click();
                Window aboutWindow = mainWindow.ModalWindow("About");
                aboutWindow.Close();
            }
        }
    }
}
