// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;
using Xunit;

namespace Microsoft.AspNetCore.Razor.Language
{
    public class DefaultRazorProjectItemTest
    {
        private static string TestFolder { get; } = Path.Combine(
            TestProject.GetProjectDirectory(typeof(DefaultRazorProjectItemTest)), 
            "TestFiles",
            "DefaultRazorProjectFileSystem");

        [Fact]
        public void DefaultRazorProjectItem_SetsProperties()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(TestFolder, "Home.cshtml"));

            // Act
            var projectItem = new DefaultRazorProjectItem("/", "/Home.cshtml", "Home.cshtml", "test", fileInfo);

            // Assert
            Assert.Equal("/Home.cshtml", projectItem.FilePath);
            Assert.Equal("/", projectItem.BasePath);
            Assert.True(projectItem.Exists);
            Assert.Equal("Home.cshtml", projectItem.FileName);
            Assert.Equal("test", projectItem.FileKind);
            Assert.Equal(fileInfo.FullName, projectItem.PhysicalPath);
            Assert.Equal("Home.cshtml", projectItem.RelativePhysicalPath);
        }

        [Fact]
        public void DefaultRazorProjectItem_InfersFileKind_Component()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(TestFolder, "Home.cshtml"));

            // Act
            var projectItem = new DefaultRazorProjectItem("/", "/Home.razor", "Home.cshtml", fileKind: null, fileInfo);

            // Assert
            Assert.Equal(FileKinds.Component, projectItem.FileKind);
        }

        [Fact]
        public void DefaultRazorProjectItem_InfersFileKind_Legacy()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(TestFolder, "Home.cshtml"));

            // Act
            var projectItem = new DefaultRazorProjectItem("/", "/Home.cshtml", "Home.cshtml", fileKind: null, fileInfo);

            // Assert
            Assert.Equal(FileKinds.Legacy, projectItem.FileKind);
        }

        [Fact]
        public void DefaultRazorProjectItem_InfersFileKind_Null()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(TestFolder, "Home.cshtml"));

            // Act
            var projectItem = new DefaultRazorProjectItem("/", filePath: null, "Home.cshtml", fileKind: null, fileInfo);

            // Assert
            Assert.Null(projectItem.FileKind);
        }

        [Fact]
        public void Exists_ReturnsFalseWhenFileDoesNotExist()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(TestFolder, "Views", "FileDoesNotExist.cshtml"));

            // Act
            var projectItem = new DefaultRazorProjectItem("/Views", "/FileDoesNotExist.cshtml", Path.Combine("Views", "FileDoesNotExist.cshtml"), "test", fileInfo);

            // Assert
            Assert.False(projectItem.Exists);
        }

        [Fact]
        public void Read_ReturnsReadStream()
        {
            // Arrange
            var fileInfo = new FileInfo(Path.Combine(TestFolder, "Home.cshtml"));
            var projectItem = new DefaultRazorProjectItem("/", "/Home.cshtml", "Home.cshtml", "test", fileInfo);

            // Act
            var stream = projectItem.Read();

            // Assert
            Assert.Equal("home-content", new StreamReader(stream).ReadToEnd());
        }
    }
}
