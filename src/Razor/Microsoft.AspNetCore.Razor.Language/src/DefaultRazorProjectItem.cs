// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.IO;

namespace Microsoft.AspNetCore.Razor.Language
{
    internal class DefaultRazorProjectItem : RazorProjectItem
    {
        private readonly string _fileKind;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultRazorProjectItem"/>.
        /// </summary>
        /// <param name="basePath">The base path.</param>
        /// <param name="relativePhysicalPath">The physical path of the base path.</param>
        /// <param name="filePath">The path.</param>
        /// <param name="fileKind">The file kind. If null, the document kind will be inferred from the file extension.</param>
        /// <param name="file">The <see cref="FileInfo"/>.</param>
        public DefaultRazorProjectItem(string basePath, string filePath, string relativePhysicalPath, string fileKind, FileInfo file)
        {
            BasePath = basePath;
            FilePath = filePath;
            RelativePhysicalPath = relativePhysicalPath;
            _fileKind = fileKind;
            File = file;
        }

        public FileInfo File { get; }

        public override string BasePath { get; }

        public override string FilePath { get; }

        public override bool Exists => File.Exists;

        public override string PhysicalPath => File.FullName;

        public override string RelativePhysicalPath { get; }

        public override string FileKind => _fileKind ?? base.FileKind;

        public override Stream Read() => new FileStream(PhysicalPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite | FileShare.Delete);
    }
}