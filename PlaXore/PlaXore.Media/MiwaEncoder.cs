#region Header
//+ <source name="MiwaEncoder.cs" language="C#" begin="24-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using PlaXore.Extensions;
using PlaXore.Media.Extensions;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Implements a Motion Imaging With Audio (MIWA) encoder.
    /// </summary>
    public class MiwaEncoder : IEncoder
    {
        #region Fields
        private const string Name = "miwa";
        private const string FrameFormat = "jpeg";
        private const byte MinFrameRate = 8;
        private const byte MaxFrameRate = 60;
        private const byte DefaultFrameRate = 30;
        private const byte MinFrameQuality = 0;
        private const byte MaxFrameQuality = 100;
        private static readonly byte[] JpegHeader;

        private int frameWidth;
        private int frameHeight;
        private uint audioBlockSize;
        private Queue<Stream> audioStreams;
        private ContentReader audioReader;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes read-only members.
        /// </summary>
        static MiwaEncoder()
        {
            JpegHeader = new byte[] { 0xff, 0xd8 };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaEncoder"/> class with
        /// the specified output stream.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> is <see langword="null"/>.
        /// </exception>
        public MiwaEncoder(Stream output)
            : this(output, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaEncoder"/> class with
        /// the specified output stream and content attributes.
        /// </summary>
        /// <param name="output">The output stream.</param>
        /// <param name="contentAttributes">The content attributes to set.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="output"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or more of the specified content attributes are not valid. Invalid attributes
        /// are those that cannot be initialized since they are determined automatically during
        /// the encoding process or those that have invalid values.
        /// </exception>
        public MiwaEncoder(Stream output, params ContentAttribute[] contentAttributes)
        {
            if (output == null) { throw new ArgumentNullException("output"); }
            Output = output;

            AssemblyInfo assemblyInfo = new AssemblyInfo();
            CodecInfo = new CodecInfo(Name, new SimpleVersion(
                (ushort) assemblyInfo.Version.Major,
                (ushort) assemblyInfo.Version.Minor));

            InitializeContentAttributes(contentAttributes);
            audioStreams = new Queue<Stream>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets information that describes this <see cref="MiwaEncoder"/>.
        /// </summary>
        /// <value>Information that describes this <see cref="MiwaEncoder"/>.</value>
        public CodecInfo CodecInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a <see cref="ContentAttributeCollection"/> that describes the content
        /// encoded by this <see cref="MiwaEncoder"/>.
        /// </summary>
        /// <value>
        /// A <see cref="ContentAttributeCollection"/> that describes the content
        /// encoded by this <see cref="MiwaEncoder"/>.
        /// </value>
        public ContentAttributeCollection ContentAttributes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="MiwaEncoder"/>
        /// has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="MiwaEncoder"/> has been
        /// disposed of; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the output stream of this <see cref="MiwaDecoder"/>.
        /// </summary>
        /// <value>The output stream of this <see cref="MiwaDecoder"/>.</value>
        public Stream Output
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disposes of this <see cref="MiwaEncoder"/> and releases all the
        /// associated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="MiwaEncoder"/>
        /// and optionally disposes of the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources;
        /// <see langword="false"/> to release unmanaged resources only.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!IsDisposed) {
                if (disposing) {
                    // Release managed resources
                    Output.Close();
                    Output = null;
                }

                // Release unmanaged resources

                IsDisposed = true;
            }
        }

        /// <summary>
        /// Encodes the streams returned by <paramref name="nextFrame"/> and
        /// <paramref name="nextAudioBlock"/> into <see cref="Output"/>.
        /// </summary>
        /// <param name="nextFrame">The delegate that returns the next frame to encode.</param>
        /// <param name="nextAudioBlock">The delegate that returns the next audio block to encode.</param>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaEncoder"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="nextFrame"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidContentException">
        /// <paramref name="nextFrame"/> is either invalid or not supported.
        /// </exception>
        /// <remarks>
        /// <paramref name="nextAudioBlock"/> might be <see langword="null"/>, in which case
        /// no audio is encoded.
        /// </remarks>
        /// <seealso cref="NextInputStream"/>
        public void Encode(NextInputStream nextFrame, NextInputStream nextAudioBlock)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            if (nextFrame == null) { throw new ArgumentNullException("nextFrame"); }

            if (nextAudioBlock != null) {
                for (Stream audioStream = nextAudioBlock(); audioStream != null; audioStream = nextAudioBlock()) {
                    audioStreams.Enqueue(audioStream);
                }
            }

            uint audioBlockCount = 0, frameCount = 0, frameRateCount = 1;
            byte frameRate = (byte) ContentAttributes[ContentAttributeId.FrameRate];

            // Open file for temporary output
            FileStream tempFile = new FileStream(
                Path.Combine(Path.GetTempPath(), DateTime.Now.ToString("yyyyMMddHHmmssffff") + ".tmp"),
                FileMode.Create);

            try {
                using (MiwaWriter miwaWriter = new MiwaWriter(tempFile, Encoding.UTF8, true)) {
                    for (Stream frame = nextFrame(); frame != null; frame = nextFrame()) {
                        // If audio is available, encode an amount of data that corresponds to
                        // the frame rate
                        if (audioStreams.Count > 0 || audioReader != null) {
                            if (--frameRateCount == 0) {
                                EncodeAudioBlock(miwaWriter); audioBlockCount++; frameRateCount = frameRate;
                            }
                        }

                        // Encode the current frame
                        EncodeFrame(frame, miwaWriter); frameCount++;
                    }

                    miwaWriter.Seek(0, SeekOrigin.Begin);
                }

                // Create global metadata
                GlobalMetadata globalMetadata;
                globalMetadata.Guid = Guid.NewGuid();
                globalMetadata.CodecInfo = CodecInfo;
                globalMetadata.FrameCount = frameCount;
                globalMetadata.AudioBlockCount = audioBlockCount;
                globalMetadata.Duration = (ulong) Math.Round(((decimal) frameCount / (decimal) frameRate) * 1000);
                globalMetadata.Title = ContentAttributes[ContentAttributeId.Title];
                globalMetadata.Copyright = ContentAttributes[ContentAttributeId.Copyright];
                globalMetadata.Timestamp = (ulong) (DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond);
                SetContentAttributes(globalMetadata);

                // Create frame metadata
                FrameMetadata frameMetadata;
                frameMetadata.Format = FrameFormat;
                Variant variant = ContentAttributes[ContentAttributeId.FrameQuality];
                frameMetadata.Height = (ushort) frameHeight;
                frameMetadata.Width = (ushort) frameWidth;
                frameMetadata.Quality = variant != null ? (byte) variant : (byte) 100;
                frameMetadata.Rate = frameRate;
                SetContentAttributes(frameMetadata);

                // Audio metadata is created and encoded before the first block of the
                // current audio stream - new metadata is created for each audio stream

                // Write the final output consisting of global metadata, frame metadata
                // audio metadata - one chunk per file, audio blocks, and frames
                using (MiwaWriter miwaWriter = new MiwaWriter(Output, Encoding.UTF8, true)) {
                    miwaWriter.Write(globalMetadata);
                    miwaWriter.Write(frameMetadata);
                    tempFile.CopyTo(Output);
                }
            } finally {
                try {
                    tempFile.Close();
                    File.Delete(tempFile.Name);
                } catch { }
            }
        }

        /// <summary>
        /// Encodes a block from the current audio stream, using the specified <see cref="MiwaWriter"/>.
        /// </summary>
        /// <param name="miwaWriter">A <see cref="MiwaWriter"/> instance.</param>
        /// <exception cref="InvalidContentException">
        /// Current audio format is either invalid or not supported.
        /// </exception>
        private void EncodeAudioBlock(MiwaWriter miwaWriter)
        {
            if (audioReader == null) {
                AudioMetadata audioMetadata = new AudioMetadata();
                audioReader = new ContentReader(audioStreams.Dequeue(), miwaWriter.Encoding, true);
                audioMetadata.ChunkId = audioReader.ReadQuickString(sizeof(uint));

                if (!audioMetadata.ChunkId.Equals("RIFF")) {
                    throw new InvalidContentException(String.Format("Chunk id {0} is not supported.", audioMetadata.ChunkId));
                }

                audioMetadata.ChunkSize = audioReader.ReadBaseType(sizeof(uint), Endianness.LittleEndian);
                audioMetadata.RiffType = audioReader.ReadQuickString(sizeof(uint));

                if (!audioMetadata.RiffType.Equals("WAVE")) {
                    throw new InvalidContentException(String.Format("Riff type {0} is not supported.", audioMetadata.RiffType));
                }

                audioMetadata.FormatId = audioReader.ReadQuickString(sizeof(uint));
                audioMetadata.FormatSize = audioReader.ReadBaseType(sizeof(uint), Endianness.LittleEndian);
                audioMetadata.CompressionType = audioReader.ReadBaseType(sizeof(ushort), Endianness.LittleEndian);
                audioMetadata.Channels = audioReader.ReadBaseType(sizeof(ushort), Endianness.LittleEndian);
                audioMetadata.SampleRate = audioReader.ReadBaseType(sizeof(uint), Endianness.LittleEndian);
                audioMetadata.AverageBytesPerSecond = audioBlockSize = audioReader.ReadBaseType(sizeof(uint), Endianness.LittleEndian);
                audioMetadata.BlockAlign = audioReader.ReadBaseType(sizeof(ushort), Endianness.LittleEndian);
                audioMetadata.BitDepth = audioReader.ReadBaseType(sizeof(ushort), Endianness.LittleEndian);

                if (audioMetadata.FormatSize == 18) {
                    audioMetadata.ExtraFormatData = audioReader.ReadBytes(audioReader.ReadBaseType(sizeof(ushort), Endianness.LittleEndian));
                }

                // Skip no data chunks
                try {
                    while (true) {
                        audioMetadata.DataId = audioReader.ReadQuickString(sizeof(uint));
                        audioMetadata.DataSize = audioReader.ReadBaseType(sizeof(uint), Endianness.LittleEndian);
                        if (audioMetadata.DataId.Equals("data")) { break; }
                        audioMetadata.ChunkSize -= audioMetadata.DataSize;
                        audioReader.Input.Seek(audioMetadata.DataSize, SeekOrigin.Current);
                    }
                } catch {
                    throw new InvalidContentException("No data chunk was found.");
                }

                miwaWriter.Write(audioMetadata);
                SetContentAttributes(audioMetadata);
            }

            miwaWriter.Write(
                BlockType.Audio,
                audioReader.ReadBytes((int) audioBlockSize));

            if (audioReader.Input.Position >= audioReader.Input.Length) {
                audioReader.Close();
                audioReader = null;
            }
        }

        /// <summary>
        /// Encodes the specified frame, using the specified <see cref="MiwaWriter"/>.
        /// </summary>
        /// <param name="frame">The frame to encode.</param>
        /// <param name="miwaWriter">A <see cref="MiwaWriter"/> instance.</param>
        /// <exception cref="InvalidContentException">
        /// <paramref name="frame"/> is either invalid or not supported.
        /// </exception>
        private void EncodeFrame(Stream frame, MiwaWriter miwaWriter)
        {
            byte[] header = new byte[JpegHeader.Length];
            int bytesRead = frame.Read(header, 0, header.Length);

            if (bytesRead < header.Length || !Utilities.BufferUtility.Equals(header, JpegHeader)) {
                throw new InvalidContentException("Frame is either invalid or not supported.");
            }

            using (frame) {
                Image image = new Bitmap(frame);

                if (frameWidth == 0 || frameHeight == 0) {
                    frameWidth = ContentAttributes[ContentAttributeId.FrameWidth];
                    frameHeight = ContentAttributes[ContentAttributeId.FrameHeight];

                    // If no frame width and height are specified, peek them from the
                    // first frame
                    if (frameWidth == 0) { frameWidth = image.Width; }
                    if (frameHeight == 0) { frameHeight = image.Height; }
                }

                if (image.Width != frameWidth || image.Height != frameHeight) {
                    image = image.Resize(frameWidth, frameHeight);
                }

                // Add code for motion detection here

                MemoryStream stream = new MemoryStream();

                if (ContentAttributes[ContentAttributeId.FrameQuality] == null) {
                    image.Save(stream, ImageFormat.Jpeg);
                } else {
                    image.Save(stream, ImageFormat.Jpeg, ContentAttributes[ContentAttributeId.FrameQuality]);
                }

                miwaWriter.Write(BlockType.Frame, stream.ToByteArray());
            }
        }

        /// <summary>
        /// Initializes content attributes.
        /// </summary>
        /// <param name="contentAttributes">The content attributes to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// One or more of the specified content attributes are invalid or cannot be set.
        /// </exception>
        private void InitializeContentAttributes(params ContentAttribute[] contentAttributes)
        {
            ContentAttributes = new ContentAttributeCollection();

            IDictionary<ContentAttributeId, ContentAttribute> dictionary = ContentAttributes.Dictionary;
            dictionary[ContentAttributeId.Codec] = new ContentAttribute(ContentAttributeId.Codec, CodecInfo.Name);
            dictionary[ContentAttributeId.CodecVersion] = new ContentAttribute(ContentAttributeId.CodecVersion, (uint) CodecInfo.Version);
            dictionary[ContentAttributeId.FrameFormat] = new ContentAttribute(ContentAttributeId.FrameFormat, FrameFormat);
            dictionary[ContentAttributeId.FrameRate] = new ContentAttribute(ContentAttributeId.FrameRate, DefaultFrameRate);

            if (contentAttributes != null) {
                foreach (ContentAttribute contentAttribute in contentAttributes) {
                    SetContentAttribute(contentAttribute);
                }
            }
        }

        /// <summary>
        /// Sets the specified content attribute.
        /// </summary>
        /// <param name="contentAttribute">The <see cref="ContentAttribute"/> to set.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="contentAttribute"/> is invalid or cannot be set.
        /// </exception>
        private void SetContentAttribute(ContentAttribute contentAttribute)
        {
            if (contentAttribute.Id == ContentAttributeId.FrameQuality) {
                if ((byte) contentAttribute < MinFrameQuality &&
                    (byte) contentAttribute > MaxFrameQuality) {
                    throw new ArgumentOutOfRangeException("contentAttribute",
                        String.Format("{0} must be between {1} and {2}.",
                        contentAttribute.Id, MinFrameQuality, MaxFrameQuality));
                }
            }

            if (contentAttribute.Id == ContentAttributeId.FrameRate) {
                if ((byte) contentAttribute < MinFrameRate &&
                    (byte) contentAttribute > MaxFrameRate) {
                    throw new ArgumentOutOfRangeException("contentAttribute",
                        String.Format("{0} must be between {1} and {2}.",
                        contentAttribute.Id, MinFrameRate, MaxFrameRate));
                }
            }

            switch (contentAttribute.Id) {
                case ContentAttributeId.FrameWidth:
                case ContentAttributeId.FrameHeight:
                case ContentAttributeId.FrameQuality:
                case ContentAttributeId.FrameRate:
                case ContentAttributeId.Title:
                case ContentAttributeId.Copyright:
                    ContentAttributes.Dictionary[contentAttribute.Id] = contentAttribute;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("contentAttribute",
                        String.Format("Attribute {0} cannot be set.",
                        contentAttribute.Id));
            }
        }

        /// <summary>
        /// Sets global content attributes.
        /// </summary>
        /// <param name="globalMetadata">
        /// The <see cref="GlobalMetadata"/> to set global attributes from.
        /// </param>
        private void SetContentAttributes(GlobalMetadata globalMetadata)
        {
            ContentAttributes[ContentAttributeId.Guid] = new ContentAttribute(ContentAttributeId.Guid, globalMetadata.Guid.ToByteArray());
            ContentAttributes[ContentAttributeId.FrameCount] = new ContentAttribute(ContentAttributeId.FrameCount, globalMetadata.FrameCount);
            ContentAttributes[ContentAttributeId.AudioBlockCount] = new ContentAttribute(ContentAttributeId.AudioBlockCount, globalMetadata.AudioBlockCount);
            ContentAttributes[ContentAttributeId.Duration] = new ContentAttribute(ContentAttributeId.Duration, globalMetadata.Duration);
            ContentAttributes[ContentAttributeId.Timestamp] = new ContentAttribute(ContentAttributeId.Timestamp, globalMetadata.Timestamp);
        }

        /// <summary>
        /// Sets frame content attributes.
        /// </summary>
        /// <param name="frameMetadata">
        /// The <see cref="FrameMetadata"/> to set frame attributes from.
        /// </param>
        private void SetContentAttributes(FrameMetadata frameMetadata)
        {
            ContentAttributes[ContentAttributeId.FrameHeight] = new ContentAttribute(ContentAttributeId.FrameHeight, frameMetadata.Height);
            ContentAttributes[ContentAttributeId.FrameWidth] = new ContentAttribute(ContentAttributeId.FrameWidth, frameMetadata.Width);
            ContentAttributes[ContentAttributeId.FrameQuality] = new ContentAttribute(ContentAttributeId.FrameQuality, frameMetadata.Quality);
            ContentAttributes[ContentAttributeId.FrameRate] = new ContentAttribute(ContentAttributeId.FrameRate, frameMetadata.Rate);
        }

        /// <summary>
        /// Sets audio content attributes.
        /// </summary>
        /// <param name="audioMetadata">
        /// The <see cref="AudioMetadata"/> to set audio attributes from.
        /// </param>
        private void SetContentAttributes(AudioMetadata audioMetadata)
        {
            ContentAttributes[ContentAttributeId.AudioFormat] = new ContentAttribute(ContentAttributeId.AudioFormat, audioMetadata.RiffType);
            ContentAttributes[ContentAttributeId.AudioChannels] = new ContentAttribute(ContentAttributeId.AudioChannels, audioMetadata.Channels);
            ContentAttributes[ContentAttributeId.AudioSampleRate] = new ContentAttribute(ContentAttributeId.AudioSampleRate, audioMetadata.SampleRate);
        }
        #endregion
    }
}
