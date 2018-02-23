﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileSystemStorageManager.cs" company="nGratis">
//  The MIT License (MIT)
//
//  Copyright (c) 2014 - 2015 Cahya Ong
//
//  Permission is hereby granted, free of charge, to any person obtaining a copy
//  of this software and associated documentation files (the "Software"), to deal
//  in the Software without restriction, including without limitation the rights
//  to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
//  copies of the Software, and to permit persons to whom the Software is
//  furnished to do so, subject to the following conditions:
//
//  The above copyright notice and this permission notice shall be included in all
//  copies or substantial portions of the Software.
//
//  THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//  IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
//  AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
//  LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
//  OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
//  SOFTWARE.
// </copyright>
// <author>Cahya Ong - cahya.ong@gmail.com</author>
// <creation_timestamp>Friday, 3 April 2015 12:10:59 AM UTC</creation_timestamp>
// --------------------------------------------------------------------------------------------------------------------

namespace nGratis.Cop.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using nGratis.Cop.Core.Contract;

    public class FileSystemStorageManager : IStorageManager
    {
        public FileSystemStorageManager(Uri rootFolderUri)
        {
            Guard.Require.IsNotNull(rootFolderUri);
            Guard.Require.IsFolder(rootFolderUri);

            this.RootFolderUri = rootFolderUri;

            if (!Directory.Exists(rootFolderUri.LocalPath))
            {
                Directory.CreateDirectory(rootFolderUri.LocalPath);
            }
        }

        public Uri RootFolderUri { get; }

        public bool IsAvailable => Directory.Exists(this.RootFolderUri.LocalPath);

        public IEnumerable<DataInfo> FindEntries(string pattern, Mime mime)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                pattern = "*";
            }

            return Directory
                .GetFiles(
                    this.RootFolderUri.LocalPath,
                    $"{pattern}{mime.ToFileExtension()}",
                    SearchOption.TopDirectoryOnly)
                .Select(path => new FileInfo(path))
                .Select(info => new DataInfo(Path.GetFileNameWithoutExtension(info.Name), mime)
                {
                    CreatedTimestamp = new DateTimeOffset(info.CreationTimeUtc, TimeSpan.Zero)
                });
        }

        public bool HasEntry(DataSpec dataSpec)
        {
            Guard.Require.IsNotNull(dataSpec);

            var filePath = Path.Combine(this.RootFolderUri.LocalPath, dataSpec.GetFileName());

            return File.Exists(filePath);
        }

        public Stream LoadEntry(DataSpec dataSpec)
        {
            Guard.Require.IsNotNull(dataSpec);

            return File.Open(
                Path.Combine(this.RootFolderUri.LocalPath, dataSpec.GetFileName()),
                FileMode.Open);
        }

        public void SaveEntry(DataSpec dataSpec, Stream dataStream)
        {
            Guard.Require.IsNotNull(dataSpec);
            Guard.Require.IsNotNull(dataStream);

            var filePath = Path.Combine(this.RootFolderUri.LocalPath, dataSpec.GetFileName());
            Guard.Require.IsFileNotExist(filePath);

            dataStream.Position = 0;

            if (dataSpec.ContentMime.IsTextDocument())
            {
                using (var reader = new StreamReader(dataStream))
                using (var writer = new StreamWriter(File.OpenWrite(filePath), Encoding.UTF8))
                {
                    writer.Write(reader.ReadToEnd());
                    writer.Flush();
                }
            }
            else
            {
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    dataStream.CopyTo(fileStream);
                    dataStream.Flush();
                }
            }
        }
    }
}