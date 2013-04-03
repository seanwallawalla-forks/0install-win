﻿/*
 * Copyright 2010-2013 Bastian Eicher
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
using System.ComponentModel;
using System.Xml.Serialization;
using Common.Utils;

namespace ZeroInstall.Model
{
    /// <summary>
    /// Represents an archive to be downloaded and extracted.
    /// </summary>
    [Serializable]
    [XmlType("archive", Namespace = Feed.XmlNamespace)]
    public sealed class Archive : DownloadRetrievalMethod, IEquatable<Archive>
    {
        #region Properties
        /// <summary>
        /// The type of the archive as a MIME type. If missing, the type is guessed from the extension on the <see cref="DownloadRetrievalMethod.Location"/> attribute. This value is case-insensitive.
        /// </summary>
        [Description("The type of the archive as a MIME type. If missing, the type is guessed from the extension on the location attribute. This value is case-insensitive.")]
        [XmlAttribute("type"), DefaultValue("")]
        public string MimeType { get; set; }

        /// <summary>
        /// The number of bytes at the beginning of the file which should be ignored. The value in <see cref="DownloadRetrievalMethod.Size"/> does not include the skipped bytes. 
        /// </summary>
        /// <remarks>This is useful for some self-extracting archives which are made up of a shell script followed by a normal archive in a single file.</remarks>
        [Description("The number of bytes at the beginning of the file which should be ignored. The value in the size attribute does not include the skipped bytes.")]
        [XmlAttribute("start-offset"), DefaultValue(0L)]
        public long StartOffset { get; set; }

        /// <inheritdoc/>
        public override long DownloadSize { get { return Size + StartOffset; } }

        /// <summary>
        /// The name of the subdirectory in the archive to extract; <see langword="null"/> or <see cref="string.Empty"/> for entire archive.
        /// </summary>
        [Description("The name of the subdirectory in the archive to extract; null for entire archive.")]
        [XmlAttribute("extract"), DefaultValue("")]
        public string Extract { get; set; }

        /// <summary>
        /// The subdirectory within the implementation directory to extract this archive to; may be <see langword="null"/>.
        /// </summary>
        [Description("The subdirectory within the implementation directory to extract this archive to; may be null.")]
        [XmlAttribute("dest")]
        public string Destination { get; set; }
        #endregion

        //--------------------//

        #region Normalize
        /// <inheritdoc/>
        public override void Normalize()
        {
            // If the MIME type is already set or the location is missing, we have nothing to do here
            if (!string.IsNullOrEmpty(MimeType) || string.IsNullOrEmpty(LocationString)) return;

            // Guess the MIME type based on the file extension
            MimeType = ArchiveUtils.GuessMimeType(LocationString);
        }
        #endregion

        //--------------------//

        #region Conversion
        /// <summary>
        /// Returns the archive in the form "Archive: Location (MimeType, Size + StartOffset, Extract) => Destination". Not safe for parsing!
        /// </summary>
        public override string ToString()
        {
            string result = string.Format("Archive: {0} ({1}, {2} + {3}, {4})", Location, MimeType, Size, StartOffset, Extract);
            if (!string.IsNullOrEmpty(Destination)) result += " => " + Destination;
            return result;
        }
        #endregion

        #region Clone
        /// <summary>
        /// Creates a deep copy of this <see cref="Archive"/> instance.
        /// </summary>
        /// <returns>The new copy of the <see cref="Archive"/>.</returns>
        private Archive CloneArchive()
        {
            return new Archive {UnknownAttributes = UnknownAttributes, UnknownElements = UnknownElements, IfZeroInstallVersion = IfZeroInstallVersion, Location = Location, Size = Size, MimeType = MimeType, StartOffset = StartOffset, Extract = Extract, Destination = Destination};
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="Archive"/> instance.
        /// </summary>
        /// <returns>The new copy of the <see cref="Archive"/>.</returns>
        public override IRecipeStep CloneRecipeStep()
        {
            return CloneArchive();
        }

        /// <summary>
        /// Creates a deep copy of this <see cref="Archive"/> instance.
        /// </summary>
        /// <returns>The new copy of the <see cref="Archive"/>.</returns>
        public override RetrievalMethod Clone()
        {
            return CloneArchive();
        }
        #endregion

        #region Equality
        /// <inheritdoc/>
        public bool Equals(Archive other)
        {
            if (other == null) return false;
            return base.Equals(other) && StringUtils.EqualsIgnoreCase(other.MimeType, MimeType) && other.StartOffset == StartOffset && other.Extract == Extract && other.Destination == Destination;
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj is Archive && Equals((Archive)obj);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                int result = base.GetHashCode();
                if (MimeType != null) result = (result * 397) ^ StringComparer.OrdinalIgnoreCase.GetHashCode(MimeType);
                result = (result * 397) ^ StartOffset.GetHashCode();
                if (Extract != null) result = (result * 397) ^ Extract.GetHashCode();
                if (Destination != null) result = (result * 397) ^ Destination.GetHashCode();
                return result;
            }
        }
        #endregion
    }
}
