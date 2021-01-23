#region Header
//+ <source name="GameHost.cs" language="C#" begin="25-Mar-2012">
//+ <author href="mailto:giuseppe.greco@agamura.com">Giuseppe Greco</author>
//+ <copyright year="2012">
//+ <holder web="http://www.agamura.com">Agamura, Inc.</holder>
//+ </copyright>
//+ <legalnotice>All rights reserved.</legalnotice>
//+ </source>
#endregion

#region References
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using PlaXore.GameFramework.Input;
using System;
using System.Collections.Generic;
#if WINDOWS_PHONE
using Microsoft.Phone.Shell;
#endif
#endregion

namespace PlaXore.GameFramework
{
    /// <summary>
    /// Represents a delegate that is called to determine whether or not the
    /// specified <see cref="GameObject"/> should be filtered out when processing
    /// <see cref="GameObjects"/>.
    /// </summary>
    /// <param name="gameObject">The <see cref="GameObject"/> to evaluate.</param>
    /// <returns>
    /// <see langword="true"/> if <paramref name="gameObject"/> should be filtered
    /// out; otherwise, <see langword="false"/>.
    /// </returns>
    public delegate bool Filter(GameObject gameObject);

    /// <summary>
    /// Implements game logic and provides rendering code. 
    /// </summary>
    [System.Runtime.Serialization.DataContract]
    public class GameHost : Microsoft.Xna.Framework.Game
    {
        #region Fields
        private GameObject[] objectArray;
        private Stack<List<GameObject>> gameObjectsStack;
        private BlendState preSpriteBlendState = BlendState.Opaque;
        private DepthStencilState preSpriteDepthStencilState = DepthStencilState.Default;
        private RasterizerState preSpriteRasterizerState = RasterizerState.CullCounterClockwise;
        private SamplerState preSpriteSamplerState = SamplerState.LinearWrap;
        private Dictionary<string, SpriteFont> fonts;
        private HighScores highScores;
        private License license;
        private Dictionary<string, Model> models;
        private SettingsManager settingsManager;
        private Dictionary<string, Song> songs;
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, Texture2D> textures;
        private TouchIndicator touchIndicator;
        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="GameHost"/> class.
        /// </summary>
        public GameHost()
        {
            gameObjectsStack = new Stack<List<GameObject>>();
            GameObjects = new List<GameObject>();
            GameInput = new GameInput();
            GameState = new GameState();

#if WINDOWS_PHONE
            PhoneApplicationService.Current.Launching += OnGameLaunching;
            PhoneApplicationService.Current.Closing += OnGameClosing;
            PhoneApplicationService.Current.Deactivated += OnGameDeactivated;
            PhoneApplicationService.Current.Activated += OnGameActivated;
#else
            GameLaunching();
#endif
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the game camera.
        /// </summary>
        /// <value>The game camera.</value>
        public MatrixCameraObject Camera
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dictionary that holds the game fonts.
        /// </summary>
        /// <value>The dictionary that holds the game fonts.</value>
        public Dictionary<string, SpriteFont> Fonts
        {
            get {
                if (fonts == null) {
                   fonts = new Dictionary<string, SpriteFont>();
                }

                return fonts;
            }
        }

        /// <summary>
        /// Gets the <see cref="GameInput"/> of this <see cref="GameHost"/>.
        /// </summary>
        /// <value>
        /// The <see cref="GameInput"/> of this <see cref="GameHost"/>.
        /// </value>
        public GameInput GameInput
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the list that holds the game objects.
        /// </summary>
        /// <value>The list that holds the game objects.</value>
        public List<GameObject> GameObjects
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the <see cref="GameState"/> of this <see cref="GameHost"/>.
        /// </summary>
        /// <value>
        /// The <<see cref="GameState"/> of this <see cref="GameHost"/>.
        /// </value>
        public GameState GameState
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the game high scores.
        /// </summary>
        /// <value>The game high scores.</value>
        public HighScores HighScores
        {
            get {
                if (highScores == null) {
                    highScores = new HighScores(this);
                }

                return highScores;
            }
        }

        /// <summary>
        /// Gets the game license.
        /// </summary>
        /// <value>The game license.</value>
        public License License
        {
            get {
                if (license == null) {
                    license = new License();
                }

                return license;
            }
        }

        /// <summary>
        /// Gets the dictionary that holds the game models.
        /// </summary>
        /// <value>The dictionary that holds the game models.</value>
        public Dictionary<string, Model> Models
        {
            get {
                if (models == null) {
                    models = new Dictionary<string, Model>();
                }

                return models;
            }
        }

        /// <summary>
        /// Gets the game settings manager.
        /// </summary>
        /// <value>The game settings manager.</value>
        public SettingsManager SettingsManager
        {
            get {
                if (settingsManager == null) {
                    settingsManager = new SettingsManager(this);

                }

                return settingsManager;
            }
        }

        /// <summary>
        /// Gets or sets the game skybox.
        /// </summary>
        /// <value>The game skybox.</value>
        public MatrixSkyboxObject Skybox
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the dictionary that holds the game songs.
        /// </summary>
        /// <value>The dictionary that holds the game songs.</value>
        public Dictionary<string, Song> Songs
        {
            get {
                if (songs == null) {
                    songs = new Dictionary<string, Song>();
                }

                return songs;
            }
        }

        /// <summary>
        /// Gets the dictionary that holds the game sound effects.
        /// </summary>
        /// <value>The dictionary that holds the game sound effects.</value>
        public Dictionary<string, SoundEffect> SoundEffects
        {
            get {
                if (soundEffects == null) {
                    soundEffects = new Dictionary<string, SoundEffect>();
                }

                return soundEffects;
            }
        }

        /// <summary>
        /// Gets the dictionary that holds the game textures.
        /// </summary>
        /// <value>The dictionary that holds the game textures.</value>
        public Dictionary<string, Texture2D> Textures
        {
            get {
                if (textures == null) {
                    textures = new Dictionary<string, Texture2D>();
                }

                return textures;
            }
        }

        /// <summary>
        /// Gets the touch indicator.
        /// </summary>
        /// <value>The touch indicator.</value>
        public TouchIndicator TouchIndicator
        {
            get {
                if (touchIndicator == null) {
                    touchIndicator = new TouchIndicator(this);
                }

                return touchIndicator;
            }
        }
        #endregion

        #region Methods
#if WINDOWS_PHONE
        /// <summary>
        /// Handles the <b>Launching</b> event.
        /// </summary>
        /// <param name="sender">The <b>PhoneApplicationService</b> that launched this <see cref="GameHost"/>.</param>
        /// <param name="args">The event data.</param>
        private void OnGameLaunching(object sender, LaunchingEventArgs args)
        {
            GameLaunching();
        }

        /// <summary>
        /// Handles the <b>Closing</b> event.
        /// </summary>
        /// <param name="sender">The <b>PhoneApplicationService</b> that closed this <see cref="GameHost"/>.</param>
        /// <param name="args">The event data.</param>
        private void OnGameClosing(object sender, ClosingEventArgs args)
        {
            GameClosing();
        }

        /// <summary>
        /// Handles the <b>Activated</b> event.
        /// </summary>
        /// <param name="sender">The <b>PhoneApplicationService</b> that activated this <see cref="GameHost"/>.</param>
        /// <param name="args">The event data.</param>
        private void OnGameActivated(object sender, ActivatedEventArgs ars)
        {
            GameActivated();
        }

        /// <summary>
        /// Handles the <b>Deactivated</b> event.
        /// </summary>
        /// <param name="sender">The <b>PhoneApplicationService</b> that deactivated this <see cref="GameHost"/>.</param>
        /// <param name="args">The event data.</param>
        private void OnGameDeactivated(object sender, DeactivatedEventArgs args)
        {
            GameDeactivated();
        }
#endif

        /// <summary>
        /// Called when this <see cref="GameHost"/> is being launched.
        /// </summary>
        /// <remarks>
        /// Derived classes can override <see cref="GameLaunching"/> to perform any
        /// action when the <see cref="GameHost"/> is being launched.
        /// </remarks>
        protected virtual void GameLaunching()
        {
        }

        /// <summary>
        /// Called when this <see cref="GameHost"/> is being closed.
        /// </summary>
        /// <remarks>
        /// Derived classes can override <see cref="GameClosing"/> to perform any
        /// action when the <see cref="GameHost"/> is being closed.
        /// </remarks>
        protected virtual void GameClosing()
        {
        }

        /// <summary>
        /// Called when this <see cref="GameHost"/> is being activated.
        /// </summary>
        /// <remarks>
        /// Derived classes can override <see cref="GameActivated"/> to perform any
        /// action when the <see cref="GameHost"/> is being activated.
        /// </remarks>
        protected virtual void GameActivated()
        {
        }

        /// <summary>
        /// Called when this <see cref="GameHost"/> is being deactivated.
        /// </summary>
        /// <remarks>
        /// Derived classes can override <see cref="GameDeactivated"/> to perform any
        /// action when the <see cref="GameHost"/> is being deactivated.
        /// </remarks>
        protected virtual void GameDeactivated()
        {
        }

        /// <summary>
        /// Called before game logic is processed to perform state initialization.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>BeginUpdate</b>.</param>
        protected virtual void BeginUpdate(GameTime gameTime)
        {
            GameInput.BeginUpdate();
        }

        /// <summary>
        /// Called when game logic is processed. This might include state management,
        /// user input processing, or data updating.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>EndUpdate</b>.</param>>
        protected virtual void EndUpdate(GameTime gameTime)
        {
            GameState.Update(gameTime);

            if (objectArray == null) {
                objectArray = new GameObject[(int) MathHelper.Max(20, GameObjects.Count * 1.2f)];
            } else if (GameObjects.Count > objectArray.Length) {
                objectArray = new GameObject[(int) (GameObjects.Count * 1.2f)];
            }

            int objectCount = GameObjects.Count;

            for (int i = 0; i < objectArray.Length; i++) {
                if (i < objectCount) {
                    if (GameObjects[i].IsDisposed) {
                        GameObjects.RemoveAt(i);
                        i--; objectCount--;
                    } else {
                        objectArray[i] = GameObjects[i];
                    }
                } else {
                    objectArray[i] = null;
                }
            }

            for (int i = 0; i < objectCount; i++) {
                if (objectArray[i].IsDisposed) { continue; }
                if (!objectArray[i].IsVisible && !objectArray[i].UpdateWhenHidden) { continue; }
                objectArray[i].Update(gameTime);
            }

            if (Camera != null && (Camera.IsVisible || Camera.UpdateWhenHidden)) {
                Camera.Update(gameTime);
            }

            if (Skybox != null && (Skybox.IsVisible || Skybox.UpdateWhenHidden)) {
                Skybox.Update(gameTime);
            }

            GameInput.EndUpdate();
            base.Update(gameTime);
        }

        /// <summary>
        /// Inserts <see cref="GameObjects"/> at the top of the game stack and
        /// reinitializes it with an empty list.
        /// </summary>
        public void PushGameObjects()
        {
            gameObjectsStack.Push(GameObjects);
            GameObjects = new List<GameObject>();
        }

        /// <summary>
        /// Removes the list of <see cref="GameObject"/> instances at the top of the
        /// game stack and assigns it to <see cref="GameObjects"/>.
        /// </summary>
        public void PopGameObjects()
        {
            GameObjects = gameObjectsStack.Pop();
        }

        /// <summary>
        /// Returns the <see cref="GameObject"/> identified by the specified tag.
        /// </summary>
        /// <param name="tag">
        /// The tag that identifies the <see cref="GameObject"/> to return.
        /// </param>
        /// <returns>
        /// The <see cref="GameObject"/> identified by <paramref name="tag"/> if found;
        /// otherwise, <see langword="null"/>.
        /// </returns>
        public GameObject GetObjectByTag(string tag)
        {
            foreach (GameObject gameObject in GameObjects) {
                if (tag.Equals(gameObject.Tag)) {
                    return gameObject;
                }
            }

            return null;
        }

#if WINDOWS_PHONE
        /// <summary>
        /// Stores <see cref="GameObjects"/> in the phone state.
        /// </summary>
        protected void WriteGameObjectsToPhoneState()
        {
            PhoneApplicationService.Current.State.Clear();

            int index = 0;
            foreach (GameObject gameObject in GameObjects) {
                if (gameObject.WriteToPhoneState) {
                    PhoneApplicationService.Current.State.Add("_obj" + index.ToString(), gameObject);
                    index++;
                }
            }

            if (Camera != null) {
                PhoneApplicationService.Current.State.Add("_camera", Camera);
            }

            if (Skybox != null) {
                PhoneApplicationService.Current.State.Add("_skybox", Skybox);
            }
        }

        /// <summary>
        /// Restores <see cref="GameObjects"/> from the phone state.
        /// </summary>
        protected void ReadGameObjectsFromPhoneState()
        {
            GameObject gameObject;

            foreach (KeyValuePair<string, object> item in PhoneApplicationService.Current.State) {
                if (item.Value is GameObject) {
                    gameObject = (GameObject) item.Value;
                    gameObject.GameHost = this;

                    if (gameObject is MatrixCameraObject) {
                        Camera = (MatrixCameraObject) gameObject;
                    } else if (gameObject is MatrixSkyboxObject) {
                        Skybox = (MatrixSkyboxObject) gameObject;
                    } else {
                        GameObjects.Add(gameObject);
                    }
                }
            }
        }
#endif

        /// <summary>
        /// Returns the name of the specified content object.
        /// </summary>
        /// <typeparam name="T">The type of the content object.</typeparam>
        /// <param name="objectDictionary">
        /// The dictionary where to search for <paramref name="contentObject"/>.
        /// </param>
        /// <param name="contentObject">
        /// The content object of which to return the name.
        /// </param>
        /// <returns>
        /// The name of <paramref name="contentObject"/>, if found; otherwise, <see langword="null"/>.
        /// </returns>
        public string GetContentObjectName<T>(Dictionary<string, T> objectDictionary, T contentObject)
        {
            foreach (KeyValuePair<string, T> item in objectDictionary) {
                if (EqualityComparer<T>.Default.Equals(item.Value, contentObject)) {
                    return item.Key;
                }
            }

            return null;
        }

        /// <summary>
        /// Returns the sprites at the specified position.
        /// </summary>
        /// <param name="position">The position of the sprites to retrieve.</param>
        /// <returns>The sprites at <paramref name="position"/>.</returns>
        public SpriteObject[] GetSpritesAtPoint(Vector2 position)
        {
            return GetSpritesAtPoint(position, null);
        }

        /// <summary>
        /// Returns the sprites at the specified position, applying the specified filter.
        /// </summary>
        /// <param name="position">The position of the sprites to retrieve.</param>
        /// <param name="filter">
        /// The delegate that determines what sprites should be filtered out, or
        /// <see langword="null"/> to apply the default filter.
        /// </param>
        /// <returns>The sprites at <paramref name="position"/>.</returns>
        public SpriteObject[] GetSpritesAtPoint(Vector2 position, Filter filter)
        {
            SpriteObject[] hits = new SpriteObject[GameObjects.Count];
            int hitCount = 0;

            foreach (GameObject gameObject in GameObjects) {
                if (gameObject.IsDisposed || !(gameObject is SpriteObject)) { continue; }
                if (filter != null && filter(gameObject)) { continue; }

                if (gameObject.IsPointInObject(position)) {
                    hits[hitCount] = gameObject as SpriteObject;
                    hitCount += 1;
                }
            }

            Array.Resize(ref hits, hitCount);
            return hits;
        }

        /// <summary>
        /// Returns the sprite with lowest layer depth at the specified position.
        /// </summary>
        /// <param name="position">The position of the sprite to retrieve.</param>
        /// <returns>
        /// The sprite with lowest layer depth at <paramref name="position"/>,
        /// if any; otherwise, <see langword="null"/>.
        /// </returns>
        public SpriteObject GetSpriteAtPoint(Vector2 position)
        {
            return GetSpriteAtPoint(position, null);
        }

        /// <summary>
        /// Returns the sprite with the lowest layer depth at the specified position,
        /// applying the specified filter.
        /// </summary>
        /// <param name="position">The position of the sprite to retrieve.</param>
        /// <param name="filter">
        /// The delegate that determines what sprites should be filtered out, or
        /// <see langword="null"/> to apply the default filter.
        /// </param>
        /// <returns>
        /// The sprite with the lowest layer depth at <paramref name="position"/>,
        /// if any; otherwise, <see langword="null"/>.
        /// </returns>
        public SpriteObject GetSpriteAtPoint(Vector2 position, Filter filter)
        {
            SpriteObject spriteObject;
            SpriteObject retObject = null;
            float lowestLayerDepth = float.MaxValue;

            foreach (GameObject gameObject in GameObjects) {
                if (gameObject.IsDisposed || !gameObject.IsVisible || !(gameObject is SpriteObject)) { continue; }
                if (gameObject is TouchIndicator || (filter != null && filter(gameObject))) { continue; }

                spriteObject = (SpriteObject) gameObject;

                if (spriteObject.LayerDepth <= lowestLayerDepth) {
                    if (spriteObject.IsPointInObject(position)) {
                        retObject = spriteObject;
                        lowestLayerDepth = spriteObject.LayerDepth;
                    }
                }
            }

            return retObject;
        }

        /// <summary>
        /// Stores the state of the rendering engine.
        /// </summary>
        /// <remarks>
        /// Call <see cref="StoreStateBeforeSprites"/> before <b>SpriteBatch.Begin</b>
        /// when rendering sprites and matrices together.
        /// </remarks>
        protected void StoreStateBeforeSprites()
        {
            preSpriteBlendState = GraphicsDevice.BlendState;
            preSpriteDepthStencilState = GraphicsDevice.DepthStencilState;
            preSpriteRasterizerState = GraphicsDevice.RasterizerState;
            preSpriteSamplerState = GraphicsDevice.SamplerStates[0];
        }

        /// <summary>
        /// Restores the state of the rendering engine.
        /// </summary>
        /// <remarks>
        /// Call <see cref="RestoreStateAfterSprites"/> after <b>SpriteBatch.End</b>
        /// when rendering sprites and matrices together.
        /// </remarks>
        protected void RestoreStateAfterSprites()
        {
            GraphicsDevice.BlendState = preSpriteBlendState;
            GraphicsDevice.DepthStencilState = preSpriteDepthStencilState;
            GraphicsDevice.RasterizerState = preSpriteRasterizerState;
            GraphicsDevice.SamplerStates[0] = preSpriteSamplerState;
        }

        /// <summary>
        /// Draws the sprites in <see cref="GameObjects"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawSprites</b>.</param>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the sprites to be drawn.</param>
        public void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch)
        {
            DrawSprites(gameTime, spriteBatch, null);
        }

        /// <summary>
        /// Draws the sprites in <see cref="GameObjects"/>, restricting to the
        /// specified texture.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawSprites</b>.</param>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the sprites to be drawn.</param>
        /// <param name="texture">The texture that determines what sprites are to be drawn.</param>
        /// <remarks>
        /// If <paramref name="texture"/> is <see langword="null"/>, then no filtering
        /// is applied and all the sprites are drawn.
        /// </remarks>
        public virtual void DrawSprites(GameTime gameTime, SpriteBatch spriteBatch, Texture2D texture)
        {
            GameObject gameObject;
            int objectCount = objectArray.Length;

            for (int i = 0; i < objectCount; i++) {
                gameObject = objectArray[i];

                if (gameObject == null || gameObject.IsDisposed) { continue; }
                if (!gameObject.IsVisible || !(gameObject is SpriteObject) || gameObject is TextObject) { continue; }

                if (texture == null || ((SpriteObject) gameObject).Texture == texture) {
                    ((SpriteObject) gameObject).Draw(gameTime, spriteBatch);
                } 
            }
        }

        /// <summary>
        /// Draws the text objects in <see cref="GameObjects"/>.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawText</b>.</param>
        /// <param name="spriteBatch">The <b>SpriteBatch</b> that groups the text objects to be drawn.</param>
        public virtual void DrawText(GameTime gameTime, SpriteBatch spriteBatch)
        {
            GameObject gameObject;
            int objectCount = objectArray.Length;

            for (int i = 0; i < objectCount; i++) {
                gameObject = objectArray[i];

                if (gameObject == null || gameObject.IsDisposed) { continue; }
                if (!gameObject.IsVisible || !(gameObject is TextObject)) { continue; }

                ((TextObject) gameObject).Draw(gameTime, spriteBatch);
            }
        }

        /// <summary>
        /// Draws the 3D objects in <see cref="GameObjects"/> with the specified
        /// effect.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawObjects</b>.</param>
        /// <param name="effect">The <b>Effect</b> to be used when drawing the 3D objects.</param>
        public virtual void DrawObjects(GameTime gameTime, Effect effect)
        {
            DrawMatrixObjects(gameTime, effect, false, null);
        }

        /// <summary>
        /// Draws the 3D objects in <see cref="GameObjects"/> with the specified
        /// effect, restricting to the specified texture.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawObjects</b>.</param>
        /// <param name="effect">The <b>Effect</b> to be used when drawing the 3D objects.</param>
        /// <param name="texture">The texture that determines what 3D objects are to be drawn.</param>
        /// <remarks>
        /// If <paramref name="texture"/> is <see langword="null"/>, then no filtering
        /// is applied and all the 3D objects are drawn.
        /// </remarks>
        public virtual void DrawObjects(GameTime gameTime, Effect effect, Texture2D texture)
        {
            DrawMatrixObjects(gameTime, effect, false, texture);
        }

        /// <summary>
        /// Draws the particles in <see cref="GameObjects"/> with the specified
        /// effect.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawParticles</b>.</param>
        /// <param name="effect">The <b>Effect</b> to be used when drawing the particles.</param>
        public virtual void DrawParticles(GameTime gameTime, Effect effect)
        {
            DrawMatrixObjects(gameTime, effect, true, null);
        }

        /// <summary>
        /// Draws the particles in <see cref="GameObjects"/> with the specified
        /// effect, restricting to the specified texture.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawParticles</b>.</param>
        /// <param name="effect">The <b>Effect</b> to be used when drawing the particles.</param>
        /// <param name="texture">The texture that determines what particles are to be drawn.</param>
        /// <remarks>
        /// If <paramref name="texture"/> is <see langword="null"/>, then no filtering
        /// is applied and all the particles are drawn.
        /// </remarks>
        public virtual void DrawParticles(GameTime gameTime, Effect effect, Texture2D texture)
        {
            DrawMatrixObjects(gameTime, effect, true, texture);
        }

        /// <summary>
        /// Draws the matrix objects in <see cref="GameObjects"/> with the specified
        /// effect, restricting to the specified texture.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to <b>DrawMatrixObjects</b>.</param>
        /// <param name="effect">The <b>Effect</b> to be used when drawing the matrix objects.</param>
        /// <param name="renderParticles">A boolean value indicating whether or not to render particles.</param>
        /// <param name="texture">The texture that determines what matrix objects are to be drawn.</param>
        /// <remarks>
        /// If <paramref name="texture"/> is <see langword="null"/>, then no filtering
        /// is applied and all the matrix objects are drawn.
        /// </remarks>
        private void DrawMatrixObjects(GameTime gameTime, Effect effect, bool renderParticles, Texture2D texture)
        {
            if (Camera != null && Camera.IsVisible) {
                Camera.Draw(gameTime, effect);
            }

            if (Skybox != null && Camera.IsVisible && Skybox.Rendered == false) { 
                Skybox.Draw(gameTime, effect);
            }

            GameObject gameObject;
            int objectCount = objectArray.Length;

            for (int i = 0; i < objectCount; i++) {
                gameObject = objectArray[i];

                if (gameObject == null || gameObject.IsDisposed) { continue; }
                if (!gameObject.IsVisible || !(gameObject is MatrixObject)) { continue; }

                if ((renderParticles && gameObject is MatrixParticleObject) || (!renderParticles && !(gameObject is MatrixParticleObject))) {
                    if (texture == null || ((MatrixObject) gameObject).Texture == texture) {
                        ((MatrixObject) gameObject).Draw(gameTime, effect);
                    }
                }
            }
        }
        #endregion
    }
}
