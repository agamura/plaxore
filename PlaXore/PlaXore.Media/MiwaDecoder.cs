#region Header
//+ <source name="MiwaDecoder.cs" language="C#" begin="28-Aug-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.Text;
using System.IO;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Represents a delegate that is called when content metadata is available.
    /// </summary>
    /// <param name="blockType">One of <see cref="BlockType"/> values.</param>
    /// <param name="metadata">The available content metadata.</param>
    /// <returns>
    /// <see langword="true"/> to continue decoding the current media stream; otherwise, 
    /// <see langword="false"/>, in which case <see cref="MiwaDecoder.Decode(TrackAvailable)"/>
    /// has to be invoked again to resume decoding.
    /// </returns>
    internal delegate bool MetadataAvailable(BlockType blockType, byte[] metadata);

    /// <summary>
    /// Implements a Motion Imaging With Audio (MIWA) decoder.
    /// </summary>
    public class MiwaDecoder : IDecoder
    {
        #region Fields
        private const string Name = "miwa";

        private MiwaReader miwaReader;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MiwaDecoder"/> with the
        /// spcified input stream.
        /// </summary>
        /// <param name="input">The input stream.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="input"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidContentException">
        /// <see cref="Input"/> is either invalid or not supported.
        /// </exception>
        public MiwaDecoder(Stream input)
        {
            if (input == null) { throw new ArgumentNullException("input"); }

            AssemblyInfo assemblyInfo = new AssemblyInfo();
            CodecInfo = new CodecInfo(Name, new SimpleVersion(
                (ushort) assemblyInfo.Version.Major,
                (ushort) assemblyInfo.Version.Minor));

            Input = input;
            miwaReader = new MiwaReader(input, Encoding.UTF8, true);

            InitializeContentAttributes();
            miwaReader.Input.Seek(0, SeekOrigin.Begin);
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets information that describes this <see cref="MiwaDecoder"/>.
        /// </summary>
        /// <value>Information that describes this <see cref="MiwaDecoder"/>.</value>
        public CodecInfo CodecInfo
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a <see cref="ContentAttributeCollection"/> that describes the content
        /// decoded by this <see cref="MiwaDecoder"/>.
        /// </summary>
        /// <value>
        /// A <see cref="ContentAttributeCollection"/> that describes the content
        /// decoded by this <see cref="MiwaDecoder"/>.
        /// </value>
        public ContentAttributeCollection ContentAttributes
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the input stream of this <see cref="MiwaDecoder"/>.
        /// </summary>
        /// <value>The input stream of this <see cref="MiwaDecoder"/>.</value>
        public Stream Input
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="MiwaDecoder"/>
        /// has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="MiwaDecoder"/> has been
        /// disposed of; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDisposed
        {
            get;
            private set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Disposes of this <see cref="MiwaDecoder"/> and releases all the
        /// associated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="MiwaDecoder"/>
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
                    miwaReader.Close();
                    Input.Close();
                    Input = null;
                }

                // Release unmanaged resources

                IsDisposed = true;
            }
        }

        /// <summary>
        /// Decodes <see cref="Input"/> and invokes <paramref name="trackAvailable"/>
        /// when a track is available for playback.
        /// </summary>
        /// <param name="trackAvailable">The delegate that handles available tracks.</param>
        /// <returns>
        /// <see langword="true"/> if decoding is complete and there are no tracks left;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaDecoder"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trackAvailable"/> is <see langword="null"/>.
        /// </exception>
        /// <remarks>
        /// The term <b>track</b> refers to the frames and audio block that make up
        /// one second of playback.
        /// </remarks>
        /// <seealso cref="TrackAvailable"/>
        public bool Decode(TrackAvailable trackAvailable)
        {
            return Decode(trackAvailable, null);
        }

        /// <summary>
        /// Decodes <see cref="Input"/> and invokes <paramref name="trackAvailable"/>
        /// when a track is available for playback.
        /// </summary>
        /// <param name="trackAvailable">The delegate that handles available tracks.</param>
        /// <param name="metadataAvailable">The delegate that handles available content metadata.</param>
        /// <returns>
        /// <see langword="true"/> if decoding is complete and there are no tracks left;
        /// otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ObjectDisposedException">
        /// This <see cref="MiwaDecoder"/> has already been disposed of.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="trackAvailable"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidContentException">
        /// <see cref="Input"/> is either invalid or not supported.
        /// </exception>
        /// <remarks>
        /// <paramref name="metadataAvailable"/> might be <see langword="null"/>, in which case
        /// no content metadata is handled.
        /// </remarks>
        internal bool Decode(TrackAvailable trackAvailable, MetadataAvailable metadataAvailable)
        {
            if (IsDisposed) { throw new ObjectDisposedException(GetType().FullName); }
            if (trackAvailable == null) { throw new ArgumentNullException("trackAvailable"); }

            int frameRate = ContentAttributes[ContentAttributeId.FrameRate];
            int frameCount = 0;

            bool done = true;
            byte[] audioBlock = null;
            byte[][] frames = new byte[frameRate][]; Array.Clear(frames, 0, frameRate);
            BlockHeader blockHeader;

            while (Input.Position < Input.Length) {
                blockHeader = miwaReader.ReadBlockHeader();

                switch (blockHeader.BlockType) {
                    case BlockType.GlobalMetadata:
                        if (metadataAvailable != null) {
                            GlobalMetadata globalMetadata = miwaReader.ReadGlobalMetadata();
                            done = metadataAvailable(blockHeader.BlockType, globalMetadata.ToByteArray());
                        }
                        break;
                    case BlockType.FrameMetadata:
                        if (metadataAvailable != null) {
                            FrameMetadata frameMetadata = miwaReader.ReadFrameMetadata();
                            done = metadataAvailable(blockHeader.BlockType, frameMetadata.ToByteArray());
                        }
                        break;
                    case BlockType.AudioMetadata:
                        AudioMetadata audioMetadata = miwaReader.ReadAudioMetadata();
                        SetContentAttributes(audioMetadata);
                        if (metadataAvailable != null) {
                            done = metadataAvailable(blockHeader.BlockType, audioMetadata.ToByteArray(Endianness.LittleEndian));
                        }
                        break;
                    case BlockType.Audio:
                        audioBlock = miwaReader.ReadBlock(blockHeader);
                        break;
                    case BlockType.Frame:
                        frames[frameCount++] = miwaReader.ReadBlock(blockHeader);
                        if (Input.Position == Input.Length) {
                            Array.Resize(ref frames, frameCount);
                            frameCount = frameRate;
                        }
                        if (frameCount == frameRate) {
                            frameCount = 0;
                            done = trackAvailable(frames, audioBlock);
                            audioBlock = null;
                            Array.Clear(frames, 0, frameRate);
                        }
                        break;
                    default:
                        throw new InvalidContentException("Input stream is either invalid or not supported.");
                }

                if (!done) { break; }
            }

            return done;
        }

        /// <summary>
        /// Initializes content attributes.
        /// </summary>
        /// <exception cref="InvalidContentException">
        /// <see cref="Input"/> is either invalid or not supported.
        /// </exception>
        private void InitializeContentAttributes()
        {
            ContentAttributes = new ContentAttributeCollection();

            for (BlockHeader blockHeader = miwaReader.ReadBlockHeader(); ; blockHeader = miwaReader.ReadBlockHeader()) {
                switch (blockHeader.BlockType) {
                    case BlockType.GlobalMetadata:
                        SetContentAttributes(miwaReader.ReadGlobalMetadata());
                        break;
                    case BlockType.FrameMetadata:
                        SetContentAttributes(miwaReader.ReadFrameMetadata());
                        if (0 == (uint) ContentAttributes[ContentAttributeId.AudioBlockCount]) { return; }
                        break;
                    case BlockType.AudioMetadata:
                        SetContentAttributes(miwaReader.ReadAudioMetadata());
                        return;
                    default:
                        throw new InvalidContentException("Input stream is either invalid or not supported.");
                }
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
            ContentAttributes[ContentAttributeId.Codec] = new ContentAttribute(ContentAttributeId.Codec, globalMetadata.CodecInfo.Name);
            ContentAttributes[ContentAttributeId.CodecVersion] = new ContentAttribute(ContentAttributeId.CodecVersion, (uint) globalMetadata.CodecInfo.Version);
            ContentAttributes[ContentAttributeId.FrameCount] = new ContentAttribute(ContentAttributeId.FrameCount, globalMetadata.FrameCount);
            ContentAttributes[ContentAttributeId.AudioBlockCount] = new ContentAttribute(ContentAttributeId.AudioBlockCount, globalMetadata.AudioBlockCount);
            ContentAttributes[ContentAttributeId.Duration] = new ContentAttribute(ContentAttributeId.Duration, globalMetadata.Duration);
            ContentAttributes[ContentAttributeId.Title] = new ContentAttribute(ContentAttributeId.Title, globalMetadata.Title);
            ContentAttributes[ContentAttributeId.Copyright] = new ContentAttribute(ContentAttributeId.Copyright, globalMetadata.Copyright);
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
            ContentAttributes[ContentAttributeId.FrameFormat] = new ContentAttribute(ContentAttributeId.FrameFormat, frameMetadata.Format);
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
