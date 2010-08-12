﻿/*
 * Copyright 2010 Bastian Eicher
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Lesser Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU Lesser Public License for more details.
 * 
 * You should have received a copy of the GNU Lesser Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.IO;
using Common;
using Common.Download;
using ZeroInstall.Model;
using ZeroInstall.Store.Implementation;

namespace ZeroInstall.DownloadBroker
{
    /// <summary>
    /// Manages one or more <see cref="FetchRequest"/>s and keeps clients informed of the progress. Files are downloaded and added to <see cref="Store"/> automatically.
    /// </summary>
    public class Fetcher
    {
        #region Properties
        /// <summary>
        /// The location to store the downloaded and unpacked <see cref="Implementation"/>s in.
        /// </summary>
        public IStore Store { get; private set; }

        /// <summary>
        /// A callback object used when the the user is to be informed about progress.
        /// </summary>
        public IFetchHandler Handler { get; private set; }
        #endregion

        #region Constructor
        /// <summary>
        /// Creates a new download fetcher with a custom target <see cref="IStore"/>.
        /// </summary>
        /// <param name="handler">A callback object used when the the user is to be informed about progress.</param>
        /// <param name="store">The location to store the downloaded and unpacked <see cref="Implementation"/>s in.</param>
        public Fetcher(IFetchHandler handler, IStore store)
        {
            #region Sanity checks
            if (handler == null) throw new ArgumentNullException("handler");
            if (store == null) throw new ArgumentNullException("store");
            #endregion

            Handler = handler;
            Store = store;
        }

        /// <summary>
        /// Creates a new download fetcher with the default <see cref="IStore"/>.
        /// </summary>
        /// <param name="handler">A callback object used when the the user is to be informed about progress.</param>
        public Fetcher(IFetchHandler handler) : this(handler, StoreProvider.Default)
        {}
        #endregion
        
        //--------------------//

        /// <summary>
        /// Execute a complete request and block until it is done.
        /// </summary>
        public void RunSync(FetchRequest fetcherRequest)
        {
            #region Sanity checks
            if (fetcherRequest == null) throw new ArgumentNullException("fetcherRequest");
            #endregion

            foreach (var implementation in fetcherRequest.Implementations)
            {
                foreach (var archive in implementation.Archives)
                {
                    string tempArchive = Path.GetTempFileName();
                    FetchArchive(archive, tempArchive);
                    Store.AddArchive(new ArchiveFileInfo {Path = tempArchive, MimeType = archive.MimeType, SubDir = archive.Extract, StartOffset = archive.StartOffset}, implementation.ManifestDigest, null, null);
                    File.Delete(tempArchive);
                    return;
                }
                foreach (var recipe in implementation.Recipes)
                {
                    var archives = new List<ArchiveFileInfo>();
                    foreach (var currentArchive in recipe.Archives)
                    {
                        string tempArchive = Path.GetTempFileName();

                        FetchArchive(currentArchive, tempArchive);
                        archives.Add(new ArchiveFileInfo {Path = tempArchive, MimeType = currentArchive.MimeType, SubDir = currentArchive.Extract, StartOffset = currentArchive.StartOffset});
                    }
                    Store.AddMultipleArchives(archives, implementation.ManifestDigest, null, null);
                    return;
                }
                throw new InvalidOperationException("No working retrieval method.");
            }
        }

        private void FetchArchive(Archive archive, string destination)
        {
            #region Sanity checks
            if (archive == null) throw new ArgumentNullException("archive");
            if (string.IsNullOrEmpty(destination)) throw new ArgumentNullException("destination");
            #endregion

            if (archive.StartOffset != 0)
                WriteZerosIntoIgnoredPartOfFile(archive, destination);

            var downloadFile = new DownloadFile(archive.Location, destination);

            RejectRemoteFileOfDifferentSize(archive, downloadFile);
            try
            {
                Handler.StartingDownload(downloadFile);
                downloadFile.RunSync();
                RejectRemoteFileOfDifferentSize(archive, downloadFile);
            }
            catch (FetcherException)
            {
                File.Delete(destination);
                throw;
            }
        }

        private static void WriteZerosIntoIgnoredPartOfFile(Archive archive, string destination)
        {
            #region Sanity checks
            if (archive == null) throw new ArgumentNullException("archive");
            if (string.IsNullOrEmpty(destination)) throw new ArgumentNullException("destination");
            #endregion

            using (var tempArchiveStream = File.Create(destination))
            {
                tempArchiveStream.Seek(archive.StartOffset - 1, SeekOrigin.Begin);
                tempArchiveStream.WriteByte(0);
            }
        }

        /// <summary>
        /// Checks if the size of <paramref name="downloadFile"/> matches the <paramref name="archive"/>'s size and offset, otherwise throws a <see cref="FetcherException"/>.
        /// </summary>
        /// <remarks>Does not perform any check if size isn't known yet.</remarks>
        /// <exception cref="FetcherException">Thrown if the remote file is known to have different size than stated in <paramref name="archive"/>.
        /// </exception>
        private static void RejectRemoteFileOfDifferentSize(Archive archive, DownloadFile downloadFile)
        {
            #region Sanity checks
            if (archive == null) throw new ArgumentNullException("archive");
            if (downloadFile == null) throw new ArgumentNullException("downloadFile");
            #endregion

            long actualSize;
            if (downloadFile.State == ProgressState.Complete) actualSize = downloadFile.BytesReceived;
            else if (IsSizeKnown(downloadFile)) actualSize = downloadFile.BytesTotal;
            else return;

            if (actualSize != archive.Size + archive.StartOffset)
                throw new FetcherException("Invalid size of file downloaded from " + archive.Location.AbsolutePath);
        }
        
        /// <summary>
        /// Determines of the size of a <see cref="DownloadFile"/> is know prior to performing the download.
        /// </summary>
        private static bool IsSizeKnown(DownloadFile downloadFile)
        {
            #region Sanity checks
            if (downloadFile == null) throw new ArgumentNullException("downloadFile");
            #endregion

            return downloadFile.BytesTotal != -1;
        }
    }
}
