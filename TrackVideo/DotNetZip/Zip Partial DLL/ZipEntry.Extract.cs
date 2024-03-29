// ZipEntry.Extract.cs
// ------------------------------------------------------------------
//
// Copyright (c)  2009 Dino Chiesa
// All rights reserved.
//
// This code module is part of DotNetZip, a zipfile class library.
//
// ------------------------------------------------------------------
//
// This code is licensed under the Microsoft Public License. 
// See the file License.txt for the license details.
// More info on: http://dotnetzip.codeplex.com
//
// ------------------------------------------------------------------
//
// last saved (in emacs): 
// Time-stamp: <2009-August-25 12:15:01>
//
// ------------------------------------------------------------------
//
// This module defines logic for Extract methods on the ZipEntry class.
//
// 
// ------------------------------------------------------------------


using System;
using System.IO;

namespace Ionic.Zip
{

    public partial class ZipEntry
    {
        /// <summary>
        /// Extract the entry to the filesystem, starting at the current working directory. 
        /// </summary>
        /// 
        /// <overloads>
        /// This method has a bunch of overloads! One of them is sure to be
        /// the right one for you... If you don't like these, check out the 
        /// <c>ExtractWithPassword()</c> methods.
        /// </overloads>
        ///
        /// <seealso cref="Ionic.Zip.ZipEntry.ExtractExistingFile"/>
        /// <seealso cref="ZipEntry.Extract(ExtractExistingFileAction)"/>
        ///
        /// <remarks>
        ///
        /// <para> This method extracts an entry from a zip file into the current working
        /// directory.  The path of the entry as extracted is the full path as specified in
        /// the zip archive, relative to the current working directory.  After the file is
        /// extracted successfully, the file attributes and timestamps are set.  </para>
        ///
        /// <para>
        /// The action taken when extraction an entry would overwrite an existing file
        /// is determined by the <see cref="ExtractExistingFile" /> property. 
        /// </para>
        ///
        /// <para>
        /// See the remarks the <see cref="LastModified"/> property, for some details 
        /// about how the last modified time of the file is set after extraction.
        /// </para>
        ///
        /// </remarks>
        public void Extract()
        {
            InternalExtract(".", null, null);
        }

        /// <summary>
        /// Extract the entry to a file in the filesystem, potentially overwriting
        /// any existing file. This method is Obsolete.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This method is Obsolete, please don't use it.  Please use method <see
        /// cref="Extract(ExtractExistingFileAction)"/> instead.
        /// </para>
        /// <para>
        /// See the remarks on the <see cref="LastModified"/> property, for some details 
        /// about how the last modified time of the created file is set after extraction.
        /// </para>
        /// </remarks>
        /// <param name="overwrite">
        /// true if the caller wants to overwrite an existing bfile 
        /// by the same name in the filesystem.
        /// </param>
        /// <seealso cref="Extract(ExtractExistingFileAction)"/>
        [Obsolete("Please use method Extract(ExtractExistingFileAction)")]
        public void Extract(bool overwrite)
        {
            OverwriteOnExtract = overwrite;
            InternalExtract(".", null, null);
        }

        /// <summary>
        /// Extract the entry to a file in the filesystem, using the specified behavior 
        /// when extraction would overwrite an existing file.
        /// </summary>
        /// <remarks>
        /// <para>
        /// See the remarks on the <see cref="LastModified"/> property, for some details 
        /// about how the last modified time of the file is set after extraction.
        /// </para>
        /// </remarks>
        /// <param name="extractExistingFile">The action to take if extraction would 
        /// overwrite an existing file.</param>
        public void Extract(ExtractExistingFileAction extractExistingFile)
        {
            ExtractExistingFile = extractExistingFile;
            InternalExtract(".", null, null);
        }

        /// <summary>
        /// Extracts the entry to the specified stream. 
        /// </summary>
        /// 
        /// <remarks>
        /// 
        /// <para>
        /// The caller can specify any write-able stream, for example <see
        /// cref="System.Console.OpenStandardOutput()"/>, a <see
        /// cref="System.IO.FileStream"/>, a <see cref="System.IO.MemoryStream"/>, or
        /// ASP.NET's <c>Response.OutputStream</c>.
        /// The content will be decrypted and decompressed as necessary. If the entry is
        /// encrypted and no password is provided, this method will throw.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <param name="stream">the stream to which the entry should be extracted.  </param>
        /// 
        public void Extract(Stream stream)
        {
            InternalExtract(null, stream, null);
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory. 
        /// </summary>
        /// 
        /// <param name="baseDirectory">the pathname of the base directory</param>
        /// 
        /// <seealso cref="Ionic.Zip.ZipEntry.ExtractExistingFile"/>
        /// <seealso cref="Ionic.Zip.ZipEntry.Extract(string, ExtractExistingFileAction)"/>
        /// <seealso cref="Ionic.Zip.ZipFile.Extract(string)"/>
        /// 
        /// <example>
        /// This example extracts only the entries in a zip file that are .txt files, 
        /// into a directory called "textfiles".
        /// <code lang="C#">
        /// using (ZipFile zip = ZipFile.Read("PackedDocuments.zip"))
        /// {
        ///   foreach (string s1 in zip.EntryFilenames)
        ///   {
        ///     if (s1.EndsWith(".txt")) 
        ///     {
        ///       zip[s1].Extract("textfiles");
        ///     }
        ///   }
        /// }
        /// </code>
        /// <code lang="VB">
        ///   Using zip As ZipFile = ZipFile.Read("PackedDocuments.zip")
        ///       Dim s1 As String
        ///       For Each s1 In zip.EntryFilenames
        ///           If s1.EndsWith(".txt") Then
        ///               zip(s1).Extract("textfiles")
        ///           End If
        ///       Next
        ///   End Using
        /// </code>
        /// </example>
        /// 
        /// <remarks>
        ///
        /// <para> Using this method, existing entries in the filesystem will not be
        /// overwritten. If you would like to force the overwrite of existing files, see the
        /// <see cref="ExtractExistingFile"/> property, or call <see cref="Extract(string,
        /// ExtractExistingFileAction)"/>. </para>
        ///
        /// <para>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </para>
        /// </remarks>
        public void Extract(string baseDirectory)
        {
            InternalExtract(baseDirectory, null, null);
        }


        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory, 
        /// and potentially overwriting existing files in the filesystem. 
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="baseDirectory">the pathname of the base directory</param>
        /// <param name="overwrite">If true, overwrite any existing files if necessary
        /// upon extraction.</param>
        ///
        /// <seealso cref="Extract(String,ExtractExistingFileAction)"/>
        [Obsolete("Please use method Extract(String,ExtractExistingFileAction)")]
        public void Extract(string baseDirectory, bool overwrite)
        {
            OverwriteOnExtract = overwrite;
            InternalExtract(baseDirectory, null, null);
        }


        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory, and
        /// using the specified behavior when extraction would overwrite an existing file.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// See the remarks on the <see cref="LastModified"/> property, for some details 
        /// about how the last modified time of the created file is set.
        /// </para>
        /// </remarks>
        ///
        /// <example>
        /// <code lang="C#">
        /// String sZipPath = "Airborne.zip";
        /// String sFilePath = "Readme.txt";
        /// String sRootFolder = "Digado";
        /// using (ZipFile zip = ZipFile.Read(sZipPath))
        /// {
        ///   if (zip.EntryFileNames.Contains(sFilePath))
        ///   {
        ///     // use the string indexer on the zip file
        ///     zip[sFileName].Extract(sRootFolder,
        ///                            ExtractExistingFileAction.OverwriteSilently);
        ///   }
        /// }
        /// </code>
        /// 
        /// <code lang="VB">
        /// Dim sZipPath as String = "Airborne.zip"
        /// Dim sFilePath As String = "Readme.txt"
        /// Dim sRootFolder As String = "Digado"
        /// Using zip As ZipFile = ZipFile.Read(sZipPath)
        ///   If zip.EntryFileNames.Contains(sFilePath)
        ///     ' use the string indexer on the zip file
        ///     zip(sFilePath).Extract(sRootFolder, _
        ///                            ExtractExistingFileAction.OverwriteSilently)
        ///   End If
        /// End Using
        /// </code>
        /// </example>
        ///
        /// <param name="baseDirectory">the pathname of the base directory</param>
        /// <param name="extractExistingFile">
        /// The action to take if extraction would overwrite an existing file.
        /// </param>
        public void Extract(string baseDirectory, ExtractExistingFileAction extractExistingFile)
        {
            ExtractExistingFile = extractExistingFile;
            InternalExtract(baseDirectory, null, null);
        }


        /// <summary>
        /// Extract the entry to the filesystem, using the current working directory
        /// and the specified password. 
        /// </summary>
        ///
        /// <overloads>
        /// This method has a bunch of overloads! One of them is sure to be
        /// the right one for you...
        /// </overloads>
        ///         
        /// <seealso cref="Ionic.Zip.ZipEntry.ExtractExistingFile"/>
        /// <seealso cref="Ionic.Zip.ZipEntry.ExtractWithPassword(ExtractExistingFileAction, string)"/>
        ///
        /// <remarks>
        ///
        /// <para> Existing entries in the filesystem will not be overwritten. If you would
        /// like to force the overwrite of existing files, see the <see
        /// cref="Ionic.Zip.ZipEntry.ExtractExistingFile"/>property, or call <see
        /// cref="ExtractWithPassword(ExtractExistingFileAction,string)"/>.</para>
        ///
        /// <para>
        /// See the remarks on the <see cref="LastModified"/> property for some details 
        /// about how the "last modified" time of the created file is set.
        /// </para>
        /// </remarks>
        /// 
        /// <example>
        /// In this example, entries that use encryption are extracted using a particular password.
        /// <code>
        /// using (var zip = ZipFile.Read(FilePath))
        /// {
        ///     foreach (ZipEntry e in zip)
        ///     {
        ///         if (e.UsesEncryption)
        ///             e.ExtractWithPassword("Secret!");
        ///         else
        ///             e.Extract();
        ///     }
        /// }
        /// </code>
        /// <code lang="VB">
        /// Using zip As ZipFile = ZipFile.Read(FilePath)
        ///     Dim e As ZipEntry
        ///     For Each e In zip
        ///         If (e.UsesEncryption)
        ///           e.ExtractWithPassword("Secret!")
        ///         Else
        ///           e.Extract
        ///         End If 
        ///     Next
        /// End Using
        /// </code>
        /// </example>
        /// <param name="password">The Password to use for decrypting the entry.</param>
        public void ExtractWithPassword(string password)
        {
            InternalExtract(".", null, password);
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory,
        /// and using the specified password. 
        /// </summary>
        /// 
        /// <seealso cref="Ionic.Zip.ZipEntry.ExtractExistingFile"/>
        /// <seealso cref="Ionic.Zip.ZipEntry.ExtractWithPassword(string, ExtractExistingFileAction, string)"/>
        ///
        /// <remarks>
        /// <para> Existing entries in the filesystem will not be overwritten. If you would
        /// like to force the overwrite of existing files, see the <see
        /// cref="Ionic.Zip.ZipEntry.ExtractExistingFile"/>property, or call <see
        /// cref="ExtractWithPassword(ExtractExistingFileAction,string)"/>.</para>
        ///
        /// <para>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="baseDirectory">The pathname of the base directory.</param>
        /// <param name="password">The Password to use for decrypting the entry.</param>
        public void ExtractWithPassword(string baseDirectory, string password)
        {
            InternalExtract(baseDirectory, null, password);
        }

        /// <summary>
        /// Extract the entry to a file in the filesystem, potentially overwriting
        /// any existing file.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="overwrite">true if the caller wants to overwrite an existing 
        /// file by the same name in the filesystem.</param>
        /// <param name="password">The Password to use for decrypting the entry.</param>
        /// <seealso cref="ExtractWithPassword(ExtractExistingFileAction,String)"/>
        [Obsolete("Please use method ExtractWithPassword(ExtractExistingFileAction,String)")]
        public void ExtractWithPassword(bool overwrite, string password)
        {
            OverwriteOnExtract = overwrite;
            InternalExtract(".", null, password);
        }


        /// <summary>
        /// Extract the entry to a file in the filesystem, relative to the current directory,
        /// using the specified behavior when extraction would overwrite an existing file.
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="password">The Password to use for decrypting the entry.</param>
        /// 
        /// <param name="extractExistingFile">
        /// The action to take if extraction would overwrite an existing file.
        /// </param>
        public void ExtractWithPassword(ExtractExistingFileAction extractExistingFile, string password)
        {
            ExtractExistingFile = extractExistingFile;
            InternalExtract(".", null, password);
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory, 
        /// and potentially overwriting existing files in the filesystem. 
        /// </summary>
        /// 
        /// <remarks>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </remarks>
        ///
        /// <param name="baseDirectory">the pathname of the base directory</param>
        ///
        /// <param name="overwrite">If true, overwrite any existing files if necessary
        /// upon extraction.</param>
        ///
        /// <param name="password">The Password to use for decrypting the entry.</param>
        ///
        /// <seealso cref="ExtractWithPassword(String,ExtractExistingFileAction,String)"/>
        [Obsolete("Please use method ExtractWithPassword(String,ExtractExistingFileAction,String)")]
        public void ExtractWithPassword(string baseDirectory, bool overwrite, string password)
        {
            OverwriteOnExtract = overwrite;
            InternalExtract(baseDirectory, null, password);
        }

        /// <summary>
        /// Extract the entry to the filesystem, starting at the specified base directory, and
        /// using the specified behavior when extraction would overwrite an existing file.
        /// </summary>
        /// 
        /// <remarks>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </remarks>
        ///
        /// <param name="baseDirectory">the pathname of the base directory</param>
        ///
        /// <param name="extractExistingFile">The action to take if extraction would
        /// overwrite an existing file.</param>
        ///
        /// <param name="password">The Password to use for decrypting the entry.</param>
        public void ExtractWithPassword(string baseDirectory, ExtractExistingFileAction extractExistingFile, string password)
        {
            ExtractExistingFile = extractExistingFile;
            InternalExtract(baseDirectory, null, password);
        }

        /// <summary>
        /// Extracts the entry to the specified stream, using the specified Password.
        /// For example, the caller could extract to Console.Out, or to a MemoryStream.
        /// </summary>
        /// 
        /// <remarks>
        /// See the remarks on the LastModified property, for some details 
        /// about how the last modified time of the created file is set.
        /// </remarks>
        /// 
        /// <param name="stream">the stream to which the entry should be extracted.  </param>
        /// <param name="password">The password to use for decrypting the entry.</param>
        public void ExtractWithPassword(Stream stream, string password)
        {
            InternalExtract(null, stream, password);
        }


        /// <summary>
        /// Opens the backing stream for the zip entry in the archive, for reading. 
        /// </summary>
        /// 
        /// <remarks>
        ///
        /// <para>
        /// DotNetZip offers a variety of ways to extract entries from a zip file.  This
        /// method allows an application to extract and entry by reading a Stream. 
        /// </para>
        ///
        /// <para>
        /// The return value is a <see cref="Ionic.Zlib.CrcCalculatorStream"/>.  Use it
        /// as you would any stream for reading.  The data you get by calling <see
        /// cref="Stream.Read(byte[], int, int)"/> on that stream will be decrypted and
        /// decompressed.
        /// </para>
        /// 
        /// <para>
        /// CrcCalculatorStream adds one additional feature: it keeps a CRC32 checksum
        /// on the bytes of the stream as it is read.  The CRC value is available in the
        /// <see cref="Ionic.Zlib.CrcCalculatorStream.Crc"/> property on the
        /// <c>CrcCalculatorStream</c>.  When the read is complete, this CRC
        /// <em>should</em> be checked against the <see cref="ZipEntry.Crc"/> property
        /// on the <c>ZipEntry</c> to validate the content of the ZipEntry.  You don't
        /// have to validate the entry using the CRC, but you should. Check the example
        /// for how to do this.
        /// </para>
        /// 
        /// <para>
        /// If the entry is protected with a password, then you need to provide a
        /// password prior to calling <see cref="OpenReader()"/>, either by setting the
        /// <see cref="Password"/> property on the entry, or the <see
        /// cref="ZipFile.Password"/> property on the <c>ZipFile</c> itself. Or, you can
        /// use <see cref="OpenReader(String)" />, the overload of OpenReader that
        /// accepts a password parameter.
        /// </para>
        /// 
        /// <para>
        /// If you want to extract entry data into a stream that is already opened, like
        /// a <see cref="System.IO.FileStream"/>, consider the <see
        /// cref="Extract(Stream)"/> method.
        /// </para>
        /// 
        /// </remarks>
        /// 
        /// <example>
        /// This example shows how to open a zip archive, then read in a named entry via
        /// a stream.  After the read loop is complete, the code compares the calculated
        /// during the read loop with the expected CRC on the <c>ZipEntry</c>, to verify
        /// the extraction.
        /// <code>
        /// using (ZipFile zip = new ZipFile(ZipFileToRead))
        /// {
        ///   ZipEntry e1= zip["Elevation.mp3"];
        ///   using (Ionic.Zlib.CrcCalculatorStream s = e1.OpenReader())
        ///   {
        ///     byte[] buffer = new byte[4096];
        ///     int n, totalBytesRead= 0;
        ///     do {
        ///       n = s.Read(buffer,0, buffer.Length);
        ///       totalBytesRead+=n; 
        ///     } while (n&gt;0);
        ///      if (s.Crc32 != e1.Crc32)
        ///       throw new Exception(string.Format("The Zip Entry failed the CRC Check. (0x{0:X8}!=0x{1:X8})", s.Crc32, e1.Crc32));
        ///      if (totalBytesRead != e1.UncompressedSize)
        ///       throw new Exception(string.Format("We read an unexpected number of bytes. ({0}!={1})", totalBytesRead, e1.UncompressedSize));
        ///   }
        /// }
        /// </code>
        /// <code lang="VB">
        ///   Using zip As New ZipFile(ZipFileToRead)
        ///       Dim e1 As ZipEntry = zip.Item("Elevation.mp3")
        ///       Using s As Ionic.Zlib.CrcCalculatorStream = e1.OpenReader
        ///           Dim n As Integer
        ///           Dim buffer As Byte() = New Byte(4096) {}
        ///           Dim totalBytesRead As Integer = 0
        ///           Do
        ///               n = s.Read(buffer, 0, buffer.Length)
        ///               totalBytesRead = (totalBytesRead + n)
        ///           Loop While (n &gt; 0)
        ///           If (s.Crc32 &lt;&gt; e1.Crc32) Then
        ///               Throw New Exception(String.Format("The Zip Entry failed the CRC Check. (0x{0:X8}!=0x{1:X8})", s.Crc32, e1.Crc32))
        ///           End If
        ///           If (totalBytesRead &lt;&gt; e1.UncompressedSize) Then
        ///               Throw New Exception(String.Format("We read an unexpected number of bytes. ({0}!={1})", totalBytesRead, e1.UncompressedSize))
        ///           End If
        ///       End Using
        ///   End Using
        /// </code>
        /// </example>
        /// <seealso cref="Ionic.Zip.ZipEntry.Extract(System.IO.Stream)"/>
        /// <returns>The Stream for reading.</returns>
        public Ionic.Zlib.CrcCalculatorStream OpenReader()
        {
            // use the entry password if it is non-null, else use the zipfile password, which is possibly null
            return InternalOpenReader(this._Password ?? this._zipfile._Password);
        }

        /// <summary>
        /// Opens the backing stream for an encrypted zip entry in the archive, for reading. 
        /// </summary>
        /// 
        /// <remarks>
        /// <para>
        /// See the documentation on the <see cref="OpenReader()"/> method for full
        /// details.  This overload allows the application to specify a password for the
        /// <c>ZipEntry</c> to be read.
        /// </para>
        /// </remarks>
        /// 
        /// <param name="password">The password to use for decrypting the entry.</param>
        /// <returns>The Stream for reading.</returns>
        public Ionic.Zlib.CrcCalculatorStream OpenReader(string password)
        {
            return InternalOpenReader(password);
        }



        private Ionic.Zlib.CrcCalculatorStream InternalOpenReader(string password)
        {
            ValidateCompression();
            ValidateEncryption();
            SetupCrypto(password);

            // workitem 7958
            if (this._Source != ZipEntrySource.ZipFile)
                throw new BadStateException("You must call ZipFile.Save before calling OpenReader.");

            Stream input = this.ArchiveStream;

            // change for workitem 8098
            // this.ArchiveStream.Seek(this.FileDataPosition, SeekOrigin.Begin);
            this._zipfile.SeekFromOrigin(this.FileDataPosition);

            // get a stream that either decrypts or not.
            Stream input2 = input;
            if (Encryption == EncryptionAlgorithm.PkzipWeak)
                input2 = new ZipCipherStream(input, _zipCrypto, CryptoMode.Decrypt);

#if AESCRYPTO
            else if (Encryption == EncryptionAlgorithm.WinZipAes128 ||
                     Encryption == EncryptionAlgorithm.WinZipAes256)
            {
                input2 = new WinZipAesCipherStream(input, _aesCrypto, _CompressedFileDataSize, CryptoMode.Decrypt);
            }
#endif
            return new Ionic.Zlib.CrcCalculatorStream((CompressionMethod == 0x08)
                                                      ? new Ionic.Zlib.DeflateStream(input2, Ionic.Zlib.CompressionMode.Decompress, true)
                                                      : input2,
                                                      _UncompressedSize);
        }


        internal Stream ArchiveStream
        {
            get
            {
                if (_archiveStream == null)
                {
                    if (_zipfile != null)
                    {
                        _zipfile.Reset();
                        _archiveStream = _zipfile.ReadStream;
                    }
                }
                return _archiveStream;
            }
        }


        private void OnExtractProgress(Int64 bytesWritten, Int64 totalBytesToWrite)
        {
            _ioOperationCanceled = _zipfile.OnExtractBlock(this, bytesWritten, totalBytesToWrite);
        }


        private void OnBeforeExtract(string path)
        {
            // When in the context of a ZipFile.ExtractAll, the events are generated from 
            // the ZipFile method, not from within the ZipEntry instance. (why?)
            // Therefore we suppress the events originating from the ZipEntry method.
            if (!_zipfile._inExtractAll)
            {
                _ioOperationCanceled = _zipfile.OnSingleEntryExtract(this, path, true);
            }
        }

        private void OnAfterExtract(string path)
        {
            // When in the context of a ZipFile.ExtractAll, the events are generated from 
            // the ZipFile method, not from within the ZipEntry instance. (why?)
            // Therefore we suppress the events originating from the ZipEntry method.
            if (!_zipfile._inExtractAll)
            {
                _zipfile.OnSingleEntryExtract(this, path, false);
            }
        }

        private void OnExtractExisting(string path)
        {
            _ioOperationCanceled = _zipfile.OnExtractExisting(this, path);
        }

        private void OnWriteBlock(Int64 bytesXferred, Int64 totalBytesToXfer)
        {
            _ioOperationCanceled = _zipfile.OnSaveBlock(this, bytesXferred, totalBytesToXfer);
        }

        private static void ReallyDelete(string fileName)
        {
            // workitem 7881
            // reset ReadOnly bit if necessary
#if NETCF
            if ( (NetCfFile.GetAttributes(fileName) & (uint)FileAttributes.ReadOnly) == (uint)FileAttributes.ReadOnly)
                NetCfFile.SetAttributes(fileName, (uint)FileAttributes.Normal);
#else
            if ((File.GetAttributes(fileName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                File.SetAttributes(fileName, FileAttributes.Normal);
#endif
            File.Delete(fileName);
        }

        // Pass in either basedir or s, but not both. 
        // In other words, you can extract to a stream or to a directory (filesystem), but not both!
        // The Password param is required for encrypted entries.
        private void InternalExtract(string baseDir, Stream outstream, string password)
        {
            // workitem 7958
            if (_zipfile == null)
                throw new BadStateException("This ZipEntry is an orphan.");

            _zipfile.Reset();
            if (this._Source != ZipEntrySource.ZipFile)
                throw new BadStateException("You must call ZipFile.Save before calling any Extract method.");

            OnBeforeExtract(baseDir);
            _ioOperationCanceled = false;
            string TargetFile = null;
            Stream output = null;
            bool fileExistsBeforeExtraction = false;
            bool checkLaterForResetDirTimes = false;
            try
            {
                ValidateCompression();
                ValidateEncryption();

                if (ValidateOutput(baseDir, outstream, out TargetFile))
                {
                    if (_zipfile.Verbose) _zipfile.StatusMessageTextWriter.WriteLine("extract dir {0}...", TargetFile);
                    // if true, then the entry was a directory and has been created.
                    // We need to fire the Extract Event.
                    OnAfterExtract(baseDir);
                    return;
                }

                // if no password explicitly specified, use the password on the entry itself,
                // or on the zipfile itself.
                string p = password ?? this._Password ?? this._zipfile._Password;
                if (UsesEncryption)
                {
                    if (p == null)
                        throw new BadPasswordException();
                    SetupCrypto(p);
                }

                // set up the output stream
                if (TargetFile != null)
                {
                    if (_zipfile.Verbose) _zipfile.StatusMessageTextWriter.WriteLine("extract file {0}...", TargetFile);
                    // ensure the target path exists
                    if (!Directory.Exists(Path.GetDirectoryName(TargetFile)))
                    {
                        // we create the directory here, but we do not set the
                        // create/modified/accessed times on it because it is being
                        // created implicitly, not explcitly. There's no entry in the
                        // zip archive for the directory. 
                        Directory.CreateDirectory(Path.GetDirectoryName(TargetFile));
                    }
                    else
                        checkLaterForResetDirTimes = _zipfile._inExtractAll;  // workitem 8264


                    // Take care of the behavior when extraction would overwrite an existing file
                    if (File.Exists(TargetFile))
                    {
                        fileExistsBeforeExtraction = true;
                        int rc = CheckExtractExistingFile(baseDir, TargetFile);
                        if (rc == 2) goto ExitTry; // cancel
                        if (rc == 1) return;
                    }
                    output = new FileStream(TargetFile, FileMode.CreateNew);
                }
                else
                {
                    if (_zipfile.Verbose) _zipfile.StatusMessageTextWriter.WriteLine("extract entry {0} to stream...", FileName);
                    output = outstream;
                }


                if (_ioOperationCanceled)
                    goto ExitTry;

                Int32 ActualCrc32 = _ExtractOne(output);

                if (_ioOperationCanceled)
                    goto ExitTry;
                            
#if AESCRYPTO
                // After extracting, Validate the CRC32
                if (ActualCrc32 != _Crc32)
                {
                    // CRC is not meaningful with WinZipAES and AES method 2 (AE-2)
                    if ((Encryption != EncryptionAlgorithm.WinZipAes128 &&
                         Encryption != EncryptionAlgorithm.WinZipAes256)
                        || _WinZipAesMethod != 0x02)
                        throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " +
                                                  String.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", _Crc32, ActualCrc32));
                }

                // Read the MAC if appropriate
                if (Encryption == EncryptionAlgorithm.WinZipAes128 ||
                    Encryption == EncryptionAlgorithm.WinZipAes256)
                {
                    _aesCrypto.ReadAndVerifyMac(this.ArchiveStream); // throws if MAC is bad
                    // side effect: advances file position.
                }
#else
                if (ActualCrc32 != _Crc32)
                    throw new BadCrcException("CRC error: the file being extracted appears to be corrupted. " +
                                              String.Format("Expected 0x{0:X8}, Actual 0x{1:X8}", _Crc32, ActualCrc32));
#endif


                if (TargetFile != null)
                {
                    output.Close();
                    output = null;

                    _SetTimes(TargetFile, true);

                    // workitem 8264
                    if (checkLaterForResetDirTimes)
                    {
                        // This is sort of a hack.  What I do here is set the time on the parent
                        // directory, every time a file is extracted into it.  If there is a
                        // directory with 1000 files, then I set the time on the dir, 1000
                        // times. This allows the directory to have times that reflect the
                        // actual time on the entry in the zip archive. 

                        // String.Contains is not available on .NET CF 2.0
                        if (this.FileName.IndexOf('/') != -1)
                        {
                            string dirname = Path.GetDirectoryName(this.FileName);
                            //Console.WriteLine("Checking for dir '{0}'", dirname);
                            if (this._zipfile[dirname] == null)
                            {
                                //Console.WriteLine("found no dir '{0}', setting times", dirname);
                                _SetTimes(Path.GetDirectoryName(TargetFile), false);
                            }
                        }
                    }

#if NETCF
                    // workitem 7926 - version made by OS can be zero or 10
                    if ((_VersionMadeBy & 0xFF00) == 0x0a00 || (_VersionMadeBy & 0xFF00) == 0x0000)
                        NetCfFile.SetAttributes(TargetFile, (uint)_ExternalFileAttrs);

#else
                    // workitem 7071
                    // We can only apply attributes if they are relevant to the NTFS OS. 
                    // Must do this LAST because it may involve a ReadOnly bit, which would prevent
                    // us from setting the time, etc. 
                    // workitem 7926 - version made by OS can be zero (FAT) or 10 (NTFS)
                    if ((_VersionMadeBy & 0xFF00) == 0x0a00 || (_VersionMadeBy & 0xFF00) == 0x0000)
                        File.SetAttributes(TargetFile, (FileAttributes)_ExternalFileAttrs);
#endif
                }

                OnAfterExtract(baseDir);
                
                ExitTry: ;
            }
            catch (Exception ex1)
            {
                _ioOperationCanceled = true;
                if (ex1 as Ionic.Zip.ZipException == null)
                    // wrap the original exception and throw
                    throw new ZipException("Cannot extract", ex1);
                else
                    throw;
            }
            finally
            {
                if (_ioOperationCanceled)
                {
                    if (TargetFile != null)
                    {
                        try
                        {
                            if (output != null) output.Close();
                            // An exception has occurred.
                            // If the file exists, check to see if it existed before we tried extracting.
                            // If it did not, or if we were overwriting the file, attempt to remove the target file.
                            if (File.Exists(TargetFile))
                            {
                                if (!fileExistsBeforeExtraction || (ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently))
                                    File.Delete(TargetFile);
                            }
                        }
                        finally { }
                    }
                }
            }
        }


        
        
        private int CheckExtractExistingFile(string baseDir, string TargetFile)
        {
            int loop = 0;
            // returns: 0 == extract, 1 = don't, 2 = cancel
            do
            {
                switch (ExtractExistingFile)
                {
                    case ExtractExistingFileAction.OverwriteSilently:
                            if (_zipfile.Verbose)
                                _zipfile.StatusMessageTextWriter.WriteLine("the file {0} exists; deleting it...", TargetFile);

                            //File.Delete(TargetFile);
                            ReallyDelete(TargetFile);
                            return 0;

                    case ExtractExistingFileAction.DoNotOverwrite:
                        if (_zipfile.Verbose)
                            _zipfile.StatusMessageTextWriter.WriteLine("the file {0} exists; not extracting entry...", FileName);
                        OnAfterExtract(baseDir);
                        return 1;

                    case ExtractExistingFileAction.InvokeExtractProgressEvent:
                        if (loop>0)
                            throw new ZipException(String.Format("The file {0} already exists.", TargetFile));
                        OnExtractExisting(baseDir);
                        if (_ioOperationCanceled)
                            return 2;

                        // loop around
                        break;
                    
                    case ExtractExistingFileAction.Throw:
                    default:
                        throw new ZipException(String.Format("The file {0} already exists.", TargetFile));
                }
                loop++;
            }
            while (true);
        }




        private void _CheckRead(int nbytes)
        {
            if (nbytes == 0)
                throw new BadReadException(String.Format("bad read of entry {0} from compressed archive.",
                             this.FileName));
        }



        private Int32 _ExtractOne(Stream output)
        {
            Stream input = this.ArchiveStream;

            // change for workitem 8098
            //input.Seek(this.FileDataPosition, SeekOrigin.Begin);
            this._zipfile.SeekFromOrigin(this.FileDataPosition);

            // to validate the CRC. 
            Int32 CrcResult = 0;

            byte[] bytes = new byte[BufferSize];

            // The extraction process varies depending on how the entry was stored.
            // It could have been encrypted, and it coould have been compressed, or both, or
            // neither. So we need to check both the encryption flag and the compression flag,
            // and take the proper action in all cases.  

            Int64 LeftToRead = (CompressionMethod == 0x08) ? this.UncompressedSize : this._CompressedFileDataSize;

            // Get a stream that either decrypts or not.
            Stream input2 = null;
            if (Encryption == EncryptionAlgorithm.PkzipWeak)
                input2 = new ZipCipherStream(input, _zipCrypto, CryptoMode.Decrypt);

#if AESCRYPTO

            else if (Encryption == EncryptionAlgorithm.WinZipAes128 ||
                 Encryption == EncryptionAlgorithm.WinZipAes256)
                input2 = new WinZipAesCipherStream(input, _aesCrypto, _CompressedFileDataSize, CryptoMode.Decrypt);
#endif

            else
                // Thu, 18 Jun 2009  01:45
                // why do I need a CrcCalculatorStream?  couldn't I just assign input1 ?
                //input2 = new Ionic.Zlib.CrcCalculatorStream(input, _CompressedFileDataSize);
                input2 = input;


            //Stream input2a = new TraceStream(input2);

            // Using the above, now we get a stream that either decompresses or not.
            Stream input3 = (CompressionMethod == 0x08)
                ? new Ionic.Zlib.DeflateStream(input2, Ionic.Zlib.CompressionMode.Decompress, true)
                : input2;

            Int64 bytesWritten = 0;
            // As we read, we maybe decrypt, and then we maybe decompress. Then we write.
            using (var s1 = new Ionic.Zlib.CrcCalculatorStream(input3))
            {
                while (LeftToRead > 0)
                {
                    //Console.WriteLine("ExtractOne: LeftToRead {0}", LeftToRead);


                    // Casting LeftToRead down to an int is ok here in the else clause,
                    // because that only happens when it is less than bytes.Length,
                    // which is much less than MAX_INT.
                    int len = (LeftToRead > bytes.Length) ? bytes.Length : (int)LeftToRead;
                    int n = s1.Read(bytes, 0, len);

                    // must check data read - essential for detecting corrupt zip files
                    _CheckRead(n);

                    output.Write(bytes, 0, n);
                    LeftToRead -= n;
                    bytesWritten += n;

                    // fire the progress event, check for cancels
                    OnExtractProgress(bytesWritten, UncompressedSize);
                    if (_ioOperationCanceled)
                    {
                        break;
                    }
                }

                CrcResult = s1.Crc;


#if AESCRYPTO
                // Read the MAC if appropriate
                if (Encryption == EncryptionAlgorithm.WinZipAes128 ||
                    Encryption == EncryptionAlgorithm.WinZipAes256)
                {
                    var wzs = input2 as WinZipAesCipherStream;
                    _aesCrypto.CalculatedMac = wzs.FinalAuthentication;
                }
#endif
            }

            return CrcResult;
        }




        internal void _SetTimes(string fileOrDirectory, bool isFile)
        {

            //             Console.WriteLine("_SetTimeS({0}): m({1}) a({2}) c({3})",
            //                               fileOrDirectory,
            //                               Mtime.ToString("yyyy/MM/dd HH:mm:ss"),
            //                               Atime.ToString("yyyy/MM/dd HH:mm:ss"),
            //                               Ctime.ToString("yyyy/MM/dd HH:mm:ss"));

            if (_ntfsTimesAreSet)
            {
#if NETCF
                // workitem 7944: set time should not be a fatal error on CF
                int rc = NetCfFile.SetTimes(fileOrDirectory, _Ctime, _Atime, _Mtime);
                if ( rc != 0)
                {
                    if (_zipfile.Verbose)
                        _zipfile.StatusMessageTextWriter.WriteLine("Warning: SetTimes failed.  entry({0})  file({1})  rc({2})",
                                                                   FileName, fileOrDirectory, rc);
                }
#else
                if (isFile)
                {
                    // It's possible that the extract was cancelled, in which case,
                    // the file does not exist.
                    if (File.Exists(fileOrDirectory))
                    {
                        File.SetCreationTimeUtc(fileOrDirectory, _Ctime);
                        File.SetLastAccessTimeUtc(fileOrDirectory, _Atime);
                        File.SetLastWriteTimeUtc(fileOrDirectory, _Mtime);
                    }
                }
                else
                {
                    // It's possible that the extract was cancelled, in which case,
                    // the directory does not exist.
                    if (Directory.Exists(fileOrDirectory))
                    {
                        Directory.SetCreationTimeUtc(fileOrDirectory, _Ctime);
                        Directory.SetLastAccessTimeUtc(fileOrDirectory, _Atime);
                        Directory.SetLastWriteTimeUtc(fileOrDirectory, _Mtime);
                    }
                }
#endif
            }
            else
            {
                // workitem 6191
                DateTime AdjustedLastModified = Ionic.Zip.SharedUtilities.AdjustTime_DotNetToWin32(LastModified);

#if NETCF
                int rc = NetCfFile.SetLastWriteTime(fileOrDirectory, AdjustedLastModified);
                        
                if ( rc != 0)
                {
                    if (_zipfile.Verbose)
                        _zipfile.StatusMessageTextWriter.WriteLine("Warning: SetLastWriteTime failed.  entry({0})  file({1})  rc({2})",
                                                                   FileName, fileOrDirectory, rc);
                }
#else
                if (isFile)
                    File.SetLastWriteTime(fileOrDirectory, AdjustedLastModified);
                else
                    Directory.SetLastWriteTime(fileOrDirectory, AdjustedLastModified);
#endif
            }
        }


        #region Support methods


        // workitem 7968
        private string UnsupportedAlgorithm
        {
            get
            {
                string alg = String.Empty;
                switch (_UnsupportedAlgorithmId)
                {
                    case 0:
                        alg = "--";
                        break;
                    case 0x6601:
                        alg = "DES";
                        break;
                    case 0x6602: // - RC2 (version needed to extract < 5.2)
                        alg = "RC2";
                        break;
                    case 0x6603: // - 3DES 168
                        alg = "3DES-168";
                        break;
                    case 0x6609: // - 3DES 112
                        alg = "3DES-112";
                        break;
                    case 0x660E: // - AES 128
                        alg = "PKWare AES128";
                        break;
                    case 0x660F: // - AES 192
                        alg = "PKWare AES192";
                        break;
                    case 0x6610: // - AES 256
                        alg = "PKWare AES256";
                        break;
                    case 0x6702: // - RC2 (version needed to extract >= 5.2)
                        alg = "RC2";
                        break;
                    case 0x6720: // - Blowfish
                        alg = "Blowfish";
                        break;
                    case 0x6721: // - Twofish
                        alg = "Twofish";
                        break;
                    case 0x6801: // - RC4
                        alg = "RC4";
                        break;
                    case 0xFFFF: // - Unknown algorithm
                    default:
                        alg = String.Format("Unknown (0x{0:X4})", _UnsupportedAlgorithmId);
                        break;
                }
                return alg;
            }
        }

        // workitem 7968
        private string UnsupportedCompressionMethod
        {
            get
            {
                string meth = String.Empty;
                switch (_CompressionMethod)
                {
                    case 0:
                        meth = "Store";
                        break;
                    case 1:
                        meth = "Shrink";
                        break;
                    case 8:
                        meth = "DEFLATE";
                        break;
                    case 9:
                        meth = "Deflate64";
                        break;
                    case 14:
                        meth = "LZMA";
                        break;
                    case 19:
                        meth = "LZ77";
                        break;
                    case 98:
                        meth = "PPMd";
                        break;
                    default:
                        meth = String.Format("Unknown (0x{0:X4})", _CompressionMethod);
                        break;
                }
                return meth;
            }
        }


        private void ValidateEncryption()
        {
            if (Encryption != EncryptionAlgorithm.PkzipWeak &&
#if AESCRYPTO
 Encryption != EncryptionAlgorithm.WinZipAes128 &&
                Encryption != EncryptionAlgorithm.WinZipAes256 &&
#endif
 Encryption != EncryptionAlgorithm.None)
            {
                // workitem 7968
                if (_UnsupportedAlgorithmId != 0)
                    throw new ZipException(String.Format("Cannot extract: Entry {0} is encrypted with an algorithm not supported by DotNetZip: {1}",
                                                         FileName, UnsupportedAlgorithm));
                else
                    throw new ZipException(String.Format("Cannot extract: Entry {0} uses an unsupported encryption algorithm ({1:X2})",
                                                         FileName, (int)Encryption));
            }
        }


        private void ValidateCompression()
        {
            if ((CompressionMethod != 0) && (CompressionMethod != 0x08))  // deflate
                throw new ZipException(String.Format("Entry {0} uses an unsupported compression method (0x{1:X2}, {2})",
                                                          FileName, CompressionMethod, UnsupportedCompressionMethod));
        }


        private void SetupCrypto(string password)
        {
            if (password == null)
                return;

            if (Encryption == EncryptionAlgorithm.PkzipWeak)
            {
                // change for workitem 8098
                //this.ArchiveStream.Seek(this.FileDataPosition - 12, SeekOrigin.Begin);
                this._zipfile.SeekFromOrigin(this.FileDataPosition - 12);
                _zipCrypto = ZipCrypto.ForRead(password, this);
            }

#if AESCRYPTO
            else if (Encryption == EncryptionAlgorithm.WinZipAes128 ||
                 Encryption == EncryptionAlgorithm.WinZipAes256)
            {

                // if we already have a WinZipAesCrypto object in place, use it.
                if (_aesCrypto != null)
                {
                    _aesCrypto.Password = password;
                }
                else
                {
                    int sizeOfSaltAndPv = LengthOfCryptoHeaderBytes;
                    // change for workitem 8098
                    //this.ArchiveStream.Seek(this.FileDataPosition - sizeOfSaltAndPv, SeekOrigin.Begin);
                    this._zipfile.SeekFromOrigin(this.FileDataPosition - sizeOfSaltAndPv);
                    _aesCrypto = WinZipAesCrypto.ReadFromStream(password, _KeyStrengthInBits, this.ArchiveStream);

                }
            }
#endif

        }


        /// <summary>
        /// Validates that the args are consistent.  
        /// </summary>
        /// <remarks>
        /// Only one of {baseDir, outStream} can be non-null.
        /// If baseDir is non-null, then the outputFile is created.
        /// </remarks>
        private bool ValidateOutput(string basedir, Stream outstream, out string OutputFile)
        {
            if (basedir != null)
            {
                // Sometimes the name on the entry starts with a slash.
                // Rather than unpack to the root of the volume, we're going to 
                // drop the slash and unpack to the specified base directory. 
                string f = this.FileName;
                if (f.StartsWith("/"))
                    f= this.FileName.Substring(1);

                // String.Contains is not available on .NET CF 2.0

                if (_zipfile.FlattenFoldersOnExtract)
                    OutputFile = Path.Combine(basedir,
                                              (f.IndexOf('/') != -1) ? Path.GetFileName(f) : f);
                else 
                    OutputFile = Path.Combine(basedir, f);


                // check if it is a directory
                if ((IsDirectory) || (FileName.EndsWith("/")))
                {
                    if (!Directory.Exists(OutputFile))
                    {
                        Directory.CreateDirectory(OutputFile);

                        _SetTimes(OutputFile, false);
                    }
                    else
                    {
                        // the dir exists, maybe we want to overwrite times. 
                        if (ExtractExistingFile == ExtractExistingFileAction.OverwriteSilently)
                            _SetTimes(OutputFile, false);
                    }
                    return true;  // true == all done, caller will return 
                }
                return false;  // false == work to do by caller.
            }

            if (outstream != null)
            {
                OutputFile = null;
                if ((IsDirectory) || (FileName.EndsWith("/")))
                {
                    // extract a directory to streamwriter?  nothing to do!
                    return true;  // true == all done!  caller can return
                }
                return false;
            }

            throw new ZipException("Cannot extract.", new ArgumentException("Invalid input.", "outstream"));
        }


        #endregion

    }
}
