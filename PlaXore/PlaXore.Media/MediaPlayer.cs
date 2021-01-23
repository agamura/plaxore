#region Header
//+ <source name="MediaPlayer.cs" language="C#" begin="17-Sep-2013">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2013">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using System;
using System.IO;
#endregion

namespace PlaXore.Media
{
    /// <summary>
    /// Provides functionality for media playback.
    /// </summary>
    public class MediaPlayer : IDisposable
    {
        #region Fields
        #endregion

        #region Events
        /// <summary>
        /// Occurs when buffering has finisched.
        /// </summary>
        public EventArgs BufferingStarted;

        /// <summary>
        /// Occurs when buffering has started.
        /// </summary>
        public EventArgs BufferingEnded;

        /// <summary>
        /// Occurs when the media is opened.
        /// </summary>
        public EventArgs MediaOpened;

        /// <summary>
        /// Occurs when the media has finished playback.
        /// </summary>
        public EventArgs MediaEnded;

        /// <summary>
        /// Occurs when an error is encountered during media playback.
        /// </summary>
        public EventArgs MediaFailed;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPlayer"/> class.
        /// </summary>
        public MediaPlayer()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the balance between the left and right speaker volume.
        /// </summary>
        /// <value>
        /// The ratio of volume across the left and right speakers in a range
        /// between -1 and 1. The default is 0.
        /// </value>
        public double Balance
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the percentage of buffering completed for streaming content.
        /// </summary>
        /// <value>
        /// The percentage of buffering completed for streaming content represented
        /// by a value between 0 and 1.
        /// </value>
        public double BufferingProgress
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the duration of the media.
        /// </summary>
        /// <value>The duration of the media, in milliseconds.</value>
        public ulong Duration
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a a value indicating whether the media has audio output.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the media has audio output; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool HasAudio
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="MediaPlayer"/> is
        /// buffering.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if this <see cref="MediaPlayer"/> is buffering;
        /// otherwise, <see langword="false"/>.
        /// </value>
        public bool IsBuffering
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets a value indicating whether or not this <see cref="MediaPlayer"/>
        /// has been disposed of.
        /// </summary>
        /// <value>
        /// <see langword="true" /> if this <see cref="MediaPlayer"/> has been
        /// disposed of; otherwise, <see langword="false"/>.
        /// </value>
        public bool IsDisposed
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the media is looped.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the media is looped; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool IsLooped
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the media is muted.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if the media is muted; otherwise,
        /// <see langword="false"/>.
        /// </value>
        public bool IsMuted
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the current position of the media.
        /// </summary>
        /// <value>
        /// The current position of the media.
        /// </value>
        public TimeSpan Position
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the state of this <see cref="MediaPlayer"/>.
        /// </summary>
        /// <value>One of the <see cref="PlayerState"/> values.</value>
        public PlayerState State
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the copyright notice for the media.
        /// </summary>
        /// <value>The copyright notice for the media.</value>
        public string SourceCopyright
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the title of the media.
        /// </summary>
        /// <value>The title of the media.</value>
        public string SourceTitle
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the media source.
        /// </summary>
        /// <value>The media source.</value>
        public Stream Source
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the height of the video.
        /// </summary>
        /// <value>The height of the video, in pixels.</value>
        public int VideoHeight
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the width of the video.
        /// </summary>
        /// <value>The width of the video, in pixels.</value>
        public int VideoWidth
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the volume of the media.
        /// </summary>
        /// <value>
        /// The volume of the media represented on a linear scale between 0 and 1.
        /// The default is 0.5.
        /// </value>
        public float Volume
        {
            get;
            set;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Closes the underlying media source.
        /// </summary>
        public void Close()
        {
        }

        /// <summary>
        /// Disposes of this <see cref="MediaPlayer"/> and releases all the
        /// associated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by this <see cref="MediaPlayer"/>
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
                }

                // Release unmanaged resources
            }
        }

        /// <summary>
        /// Opens the specified media source for playback.
        /// </summary>
        /// <param name="source">The media source.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> is <see langword="null"/>.
        /// </exception>
        public void Open(Stream source)
        {
        }

        /// <summary>
        /// Pauses media playback.
        /// </summary>
        public void Pause()
        {
        }

        /// <summary>
        /// Plays media from the current <see cref="Position"/>.
        /// </summary>
        public void Play()
        {
        }

        /// <summary>
        /// Stops media playback.
        /// </summary>
        public void Stop()
        {
        }
        #endregion
    }
}
