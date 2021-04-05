﻿using System;
 using System.Linq.Expressions;
 using System.Text.Json;
 using System.Threading.Tasks;
 using System.Windows.Forms.VisualStyles;
 using System.Xml.Linq;
 using Melville.FileSystem.FileSystem;
 using Melville.MVVM.Wpf.EventBindings.SearchTree;
 using Melville.MVVM.Wpf.MvvmDialogs;
 using Melville.MVVM.Wpf.RootWindows;
 using Moq;
 using WebDashboard.SecretManager.Models;
 using WebDashboard.SecretManager.Views;
 using WebDashboard.Startup;
 using Xunit;

namespace WebDashboard.Test.Views
{
    public class FileViewModelTest
    {
        private readonly Mock<IStartupData> startUp = new Mock<IStartupData>();
        private readonly Mock<IOpenSaveFile> fileDlg = new Mock<IOpenSaveFile>();
        private readonly Mock<IRootModelFactory> modelFact = new Mock<IRootModelFactory>();
        private readonly Mock<INavigationWindow> nav = new Mock<INavigationWindow>();
        private readonly Mock<IFile> file = new Mock<IFile>();
        
        private readonly FileLoadViewModel sut;

        public FileViewModelTest()
        {
            sut = new FileLoadViewModel(fileDlg.Object, startUp.Object, nav.Object,
                new IFileViewerFactory[]
                {
                    new SecretManagerViewModelFactory(modelFact.Object, rm=>new RootViewModel(rm))
                });
        }

        [Theory]
        [InlineData(null, 1)]
        [InlineData("Foo",1)]
        [InlineData("ihweaxihde.csproj",1)]
        [InlineData("ihweaxihde.exd",1)]
        [InlineData("ihweaxihde.pubxml",0)]
        [InlineData("ihweaxihde.PuBXmL",0)]
        public async Task Setup(string? cmdLineFile, int loadDb)
        {
            Expression<Func<IOpenSaveFile, IFile>> loadFileExpr = 
                i=>i.GetLoadFile(null, "pubxml", "Project or Deploy File|*.pubxml;*.csproj;*.props", "Pick a Deploy file");
            modelFact.Setup(i => i.Create(It.IsAny<IFile>())).ReturnsAsync(
                (IFile i) => new RootModel(new PublishFileHolder(i, new XElement("XElt")),
                    new ProjectFileHolder(i, new XElement("XElt")),
                    new SecretFileHolder(i, JsonDocument.Parse("{}")),
                    new SecretFileHolder(i, JsonDocument.Parse("{}"))));
            file.Setup(i => i.Exists()).Returns(true);
            file.SetupGet(i => i.Path).Returns(cmdLineFile??"xxx.csproj");
            fileDlg.Setup(loadFileExpr).Returns(file.Object);
            startUp.Setup(i => i.ArgumentAsFile(0)).Returns(file.Object);
            startUp.SetupGet(i => i.CommandLineArguments).Returns(
                cmdLineFile == null?new string[0]:new[]{cmdLineFile});
             await sut.Setup();
            fileDlg.Verify(loadFileExpr, Times.Exactly(loadDb));
            nav.Verify(i=>i.NavigateTo(It.IsAny<RootViewModel>()));
        }

        [Fact]
        public async Task SetupFail()
        {
            file.Setup(i => i.Exists()).Returns(false);
            startUp.Setup(i => i.ArgumentAsFile(0)).Returns(file.Object);
            startUp.SetupGet(i => i.CommandLineArguments).Returns(new []{"xxyy.pubxml"});
            await sut.Setup();
            nav.VerifyNoOtherCalls();
            modelFact.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task SetupDialogNullFail()
        {
            startUp.Setup(i => i.ArgumentAsFile(0)).Returns(file.Object);
            startUp.SetupGet(i => i.CommandLineArguments).Returns(new string[0]);
            await sut.Setup();
            nav.VerifyNoOtherCalls();
            modelFact.VerifyNoOtherCalls();
        }
        [Fact]
        public async Task SetupDialogEmptyFail()
        {
            Expression<Func<IOpenSaveFile, IFile>> loadFileExpr = 
                i=>i.GetLoadFile(null, "pubxml", "Deploy File (*.pubxml)|*.pubxml", "Pick a Deploy file");
            file.Setup(i => i.Exists()).Returns(false);
            fileDlg.Setup(loadFileExpr).Returns(file.Object);
            startUp.Setup(i => i.ArgumentAsFile(0)).Returns(file.Object);
            startUp.SetupGet(i => i.CommandLineArguments).Returns(new string[0]);
            await sut.Setup();
            nav.VerifyNoOtherCalls();
            modelFact.VerifyNoOtherCalls();
        }
    }
}