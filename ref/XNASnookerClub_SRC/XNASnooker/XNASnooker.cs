using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.IO;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Snooker.Client.XNA.Controls;
using Snooker.Client.Core;
using Snooker.Client.Core.Model;
using SnookerCore;
using Drawing = System.Drawing;

namespace Snooker.Client.XNA
{
    public enum PoolState
    {
        AwaitingShot,
        MovingBalls,
        PreparingCue,
        ShootingCue
    }

    public enum BallValues
    {
        White = 0,
        Red = 1,
        Yellow = 2,
        Green = 3,
        Brown = 4,
        Blue = 5,
        Pink = 6,
        Black = 7
    }

    public enum PlayerState
    {
        None,
        SelectingNumberOfPlayers,
        SelectingHost,
        Connecting,
        ReceivingInvitation,
        Aiming,
        Calling
    }

    public enum GameState
    {
        None,
        SignIn,
        Setup,
        ShowOpponents,
        Play,
        TestShot,
        GameOver
    }

    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class XNASnooker : Game, IBallObserver, IPocketObserver, IVisualKeyboardObserver
    {
        bool showLogList = Convert.ToBoolean(ConfigurationManager.AppSettings["showLogList"]);
        int maxComputerAttempts = 1;
        static ContractPerson contractPerson = new ContractPerson();
        private SpriteFont gameFont;
        GameState lastGameState = GameState.None;
        GameState currentGameState = GameState.None;
        static int playingTeamID = 1;
        static int awaitingTeamID = 2;

        bool calculatingPositions = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        CursorSprite cursorSprite;
        RotatorSprite ballPointerSprite;
        CursorSprite targetSprite;
        KeySprite keySprite;
        GhostBallSprite ghostBallSprite;
        KeyboardSprite keyboardSprite;
        StrengthSprite strengthSprite;
        static Texture2D alphaCueTexture1 = null;
        static Texture2D alphaCueTexture2 = null;
        static Texture2D cueTexture1 = null;
        static Texture2D cueTexture2 = null;
        static CueSprite cueSprite;
        static PictureSprite picture1Sprite;
        static PictureSprite picture2Sprite;
        static PictureSprite signInPictureSprite;
        static PictureSprite clickHereSprite;
        static Texture2D computerTexture;

        static PictureSprite team1Player1PictureSprite;
        static PictureSprite team1Player2PictureSprite;
        static PictureSprite team2Player1PictureSprite;
        static PictureSprite team2Player2PictureSprite;

        Vector2 cueSpriteOrigin = new Vector2(-24, 14);
        Vector2 hitPosition = new Vector2(0, 0);
        int cueDistance = 0;
        int maxCueDistance = 200;

        List<Ball> ballSprites = new List<Ball>();
        List<TableBorder> tableBorders = new List<TableBorder>();
        List<DiagonalBorder> diagonalBorders = new List<DiagonalBorder>();
        List<Pocket> pockets = new List<Pocket>();
        Rectangle poolRectangle = new Rectangle(232, 22, 603, 342);
        Rectangle picture1Rectangle = new Rectangle(56, 31, 117, 78);
        Rectangle picture2Rectangle = new Rectangle(56, 152, 117, 78);
        Rectangle name1Rectangle = new Rectangle(42, 95, 100, 14);
        Rectangle name2Rectangle = new Rectangle(42, 217, 100, 14);
        Rectangle score1Rectangle = new Rectangle(155, 95, 37, 17);
        Rectangle score2Rectangle = new Rectangle(155, 217, 37, 17);
        Point ballOn1Position = new Point(186, 91);
        Point ballOn2Position = new Point(186, 213);
        int ballOnIndex = 1;
        Ball ballOn1;
        Ball ballOn2;
        Rectangle signInNameRectangle = new Rectangle(480, 250, 100, 14);
        static Rectangle team1Player1PictureRectangle = new Rectangle(360, 150, 117, 78);
        static Rectangle team2Player1PictureRectangle = new Rectangle(605, 150, 117, 78);
        static Rectangle clickHereRectangle = new Rectangle(328, 56, 179, 172);

        Rectangle keyboardRectangle = new Rectangle(370, 152, 86, 75);
        private Texture2D backgroundTexture;
        private Texture2D backgroundTextureBlue;
        private Texture2D backgroundTextureGreen;
        private Texture2D backgroundTextureRed;
        private Texture2D xnaSnookerTexture;
        private Texture2D xnaSnookerAlphaTexture;
        Rectangle gameOverTextRectangle = new Rectangle(370, 120, 86, 75);
        float friction = 0.008F;
        PoolState currentPoolState = PoolState.AwaitingShot;
        PoolState lastPoolState = PoolState.AwaitingShot;
        static ContractTeam contractTeam;
        //static Team currentTeam;
        //static Team otherTeam;
        static List<Team> teams = new List<Team>();
        AudioEngine audioEngine;
        WaveBank waveBank;
        SoundBank soundBank;
        SoundEffect soundEffectIntro;
        SoundEffectInstance soundEffectInstance;
        List<Ball> pottedBalls = new List<Ball>();
        List<Ball> fallenBalls = new List<Ball>();
        List<Ball> strokenBalls = new List<Ball>();
        PlayerState playerState = PlayerState.None;
        PlayerState lastPlayerState = PlayerState.None;
        int fallingBallFadeDecrement = 24;
        double fallingBallFadeDelay = .035;
        double maxFallingBallFadeDelay = .035;

        double keyboardDelay = .05;
        double maxKeyboardDelay = .05;

        double ballPointerFadeDelay = .035;
        double maxBallPointerFadeDelay = .035;

        double changeBallPointerDelay = 1.000;
        double maxChangeBallPointerDelay = 1.000;

        double ballPointerRotateDelay = .035;
        double maxBallPointerRotateDelay = .035;

        double roofFanRotateDelay = .005;
        double maxroofFanRotateDelay = .005;

        double movingBallDelay = .010;
        double maxMovingBallDelay = .010;

        double showOpponentsDelay = 2.0;
        double maxShowOpponentsDelay = 2.0;
        double showOpponentsFadeDelay = 2.0;
        double maxShowOpponentsFadeDelay = 2.0;

        double rotatingCueDelay = .0005;
        double maxRotatingCueDelay = .0005;

        int showOpponentsAlpha = 255;

        double gameOverFadeDelay = 3.0;
        double maxGameOverFadeDelay = 3.0;
        int gameOverAlpha = 255;

        Vector2 targetVector = new Vector2(0, 0);

        Rectangle thermometerRectangle = new Rectangle(32, 278, 152, 10);
        Rectangle strengthRectangle = new Rectangle(42, 278, 142, 4);
        Rectangle targetRectangle = new Rectangle(73, 298, 71, 71);

        bool fallenBallsProcessed = false;
        List<Vector2> lights = new List<Vector2>();
        VisualKeyboard visualKeyboard;
        static Player signInPlayer;
        static Player computerPlayer;

        //XNAControls
        static XNARadioButton rbtSinglePlayer;
        static XNARadioButton rbtMultiPlayer;
        static XNARadioButton rbtLevelEasy;
        static XNARadioButton rbtLevelNormal;
        static XNARadioButton rbtLevelHard;
        XNAButton btnSignIn;

        Rectangle playerOptionsRectangle = new Rectangle(557, 100, 228, 192);

        static int currentSnapshotNumber = 0;
        static Shot currentIncomingShot = null;

        static List<Shot> incomingShotList = new List<Shot>();
        static Shot outgoingShot = new Shot();
        static List<string> logList = new List<string>();

        public XNASnooker()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 874;
            graphics.PreferredBackBufferHeight = 397;
            visualKeyboard = new VisualKeyboard(this, soundBank);
        }

        private static void ResetOutgoingShot()
        {
            outgoingShot.TeamId = playingTeamID;
            outgoingShot.SnapshotList = new Snapshot[] { };
            outgoingShot.CurrentTeamScore = teams[playingTeamID - 1].Points;
            outgoingShot.OtherTeamScore = teams[awaitingTeamID - 1].Points;
            outgoingShot.GameOver = false;
            outgoingShot.HasFinishedTurn = false;

            currentSnapshotNumber = 0;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            audioEngine = new AudioEngine(@"Content\Sounds\XNASnooker.xgs");
            // Assume the default names for the wave and sound banks.
            // To change these names, change properties in XACT.
            waveBank = new WaveBank(audioEngine, @"Content\Sounds\Wave Bank.xwb");
            soundBank = new SoundBank(audioEngine, @"Content\Sounds\Sound Bank.xsb");

            base.Initialize();
            this.Exiting += new EventHandler(XNASnooker_Exiting);

            UpdateGameState(GameState.SignIn);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            System.Windows.Forms.Clipboard.Clear();

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            backgroundTextureBlue = Content.Load<Texture2D>(@"Images\score_wallpaper_blue");
            backgroundTextureGreen = Content.Load<Texture2D>(@"Images\score_wallpaper_green");
            backgroundTextureRed = Content.Load<Texture2D>(@"Images\score_wallpaper_red");

            backgroundTexture = backgroundTextureBlue;

            xnaSnookerTexture = Content.Load<Texture2D>(@"Images\xnaSnooker");
            xnaSnookerAlphaTexture = Content.Load<Texture2D>(@"Images\xnaSnookerAlpha");

            cursorSprite = new CursorSprite(Content.Load<Texture2D>(@"Images\cursor"), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), new Vector2(16f, 16f));
            targetSprite = new CursorSprite(Content.Load<Texture2D>(@"Images\targetCursor"), new Vector2(targetVector.X, targetVector.Y), new Vector2(12f, 12f));
            targetSprite.Position = new Vector2((int)(targetRectangle.X + targetRectangle.Width / 2 + targetVector.X - targetSprite.Size.X / 2), (int)(targetRectangle.Y + targetRectangle.Height / 2 + targetVector.Y - targetSprite.Size.Y / 2));

            keyboardSprite = new KeyboardSprite(Content.Load<Texture2D>(@"Images\Keyboard"), new Vector2(keyboardRectangle.X, keyboardRectangle.Y), new Vector2(365, 112));
            keySprite = new KeySprite(Content.Load<Texture2D>(@"Images\Key"), new Vector2(-100, -100), new Vector2(27f, 27f));

            strengthSprite = new StrengthSprite(Content.Load<Texture2D>(@"Images\strengthBar"), new Vector2(strengthRectangle.X, strengthRectangle.Y + 4f), new Vector2(1f, 4f), 1);

            alphaCueTexture1 = Content.Load<Texture2D>(@"Images\AlphaCue");
            cueTexture1 = Content.Load<Texture2D>(@"Images\Cue");
            alphaCueTexture2 = Content.Load<Texture2D>(@"Images\AlphaCue");
            cueTexture2 = Content.Load<Texture2D>(@"Images\CueBlack");

            cueSprite = new CueSprite(cueTexture1, alphaCueTexture1, Content.Load<Texture2D>(@"Images\ShadowCue"), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), new Vector2(910, 30));


            picture1Sprite = new PictureSprite(null, new Vector2(picture1Rectangle.X, picture1Rectangle.Y), new Vector2(picture1Rectangle.Width, picture1Rectangle.Height));
            picture2Sprite = new PictureSprite(null, new Vector2(picture2Rectangle.X, picture2Rectangle.Y), new Vector2(picture2Rectangle.Width, picture2Rectangle.Height));
            signInPictureSprite = new PictureSprite(Content.Load<Texture2D>(@"Images\anonymous"), new Vector2(signInNameRectangle.X, signInNameRectangle.Y), new Vector2(signInNameRectangle.Width, signInNameRectangle.Height));
            clickHereSprite = new PictureSprite(Content.Load<Texture2D>(@"Images\ClickHere"), new Vector2(clickHereRectangle.X, clickHereRectangle.Y), new Vector2(clickHereRectangle.Width, clickHereRectangle.Height));
            computerTexture = Content.Load<Texture2D>(@"Images\computer3");

            signInPlayer = new Player("");
            signInPlayer.Texture = signInPictureSprite.Texture;

            computerPlayer = new Player("Computer");
            computerPlayer.Texture = computerTexture;

            lights.Add(new Vector2(0, 0));
            lights.Add(new Vector2(0, 342));
            lights.Add(new Vector2(603, 0));
            lights.Add(new Vector2(603, 342));

            LoadBalls();

            ghostBallSprite = new GhostBallSprite(Content.Load<Texture2D>(@"Images\GhostBall"), new Vector2(0, 0), new Vector2(51, 17));
            ballPointerSprite = new RotatorSprite(Content.Load<Texture2D>(@"Images\BallPointer"), new Vector2(targetVector.X, targetVector.Y), new Vector2(171f, 152f), new Vector2(85, 83), 0.5f);

            pockets.Add(new Pocket(this, 1, 5, 5, 29, 29));
            pockets.Add(new Pocket(this, 2, 288, 0, 301, 25));
            pockets.Add(new Pocket(this, 3, 571, 5, 573, 29));
            pockets.Add(new Pocket(this, 4, 5, 309, 29, 309));
            pockets.Add(new Pocket(this, 5, 288, 314, 301, 313));
            pockets.Add(new Pocket(this, 6, 571, 309, 572, 310));

            diagonalBorders.Add(new DiagonalBorder(547, 309, 35, Side.Southwest));
            diagonalBorders.Add(new DiagonalBorder(573, 286, 35, Side.Northeast));
            diagonalBorders.Add(new DiagonalBorder(1, 27, 35, Side.Southwest));
            diagonalBorders.Add(new DiagonalBorder(24, 1, 35, Side.Northeast));
            diagonalBorders.Add(new DiagonalBorder(546, 33, 35, Side.Northwest));
            diagonalBorders.Add(new DiagonalBorder(567, 59, 35, Side.Southeast));
            diagonalBorders.Add(new DiagonalBorder(1, 319, 35, Side.Northwest));
            diagonalBorders.Add(new DiagonalBorder(18, 344, 35, Side.Southeast));

            tableBorders.Add(new TableBorder(this, -60, 54, 90, 235, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 577, 54, 90, 235, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 51, -60, 230, 90, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 51, 316, 230, 90, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 319, -60, 235, 90, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 319, 316, 235, 90, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 281, -60, 38, 60, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 281, 344, 38, 60, ForcedDirection.None));

            tableBorders.Add(new TableBorder(this, -20, 0, 20, 53, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 606, 0, 20, 53, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 0, -20, 53, 20, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 0, 344, 53, 20, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, -20, 289, 20, 53, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 554, -20, 53, 20, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 606, 289, 20, 53, ForcedDirection.None));
            tableBorders.Add(new TableBorder(this, 554, 344, 53, 20, ForcedDirection.None));

            foreach (TableBorder tb in tableBorders)
            {
                tb.Texture = strengthSprite.Texture;
            }

            gameFont = Content.Load<SpriteFont>(@"Fonts\Calibri");

            //XNAControls
            rbtSinglePlayer = new XNARadioButton(gameFont, true, "Single Player");
            rbtMultiPlayer = new XNARadioButton(gameFont, false, "Multi Player");
            rbtLevelEasy = new XNARadioButton(gameFont, false, "Easy");
            rbtLevelNormal = new XNARadioButton(gameFont, true, "Normal");
            rbtLevelHard = new XNARadioButton(gameFont, false, "Hard");
            btnSignIn = new XNAButton(gameFont, "Sign In");

            teams.Add(new Team(1));
            teams.Add(new Team(2));
        }

        private void LoadBalls()
        {
            Texture2D shadowTexture = Content.Load<Texture2D>(@"Images\ShadowBall");
            Texture2D alphaRedTexture = Content.Load<Texture2D>(@"Images\AlphaRedBall");
            Texture2D alphaYellowTexture = Content.Load<Texture2D>(@"Images\AlphaYellowBall");
            Texture2D alphaGreenTexture = Content.Load<Texture2D>(@"Images\AlphaGreenBall");
            Texture2D alphaBrownTexture = Content.Load<Texture2D>(@"Images\AlphaBrownBall");
            Texture2D alphaBlueTexture = Content.Load<Texture2D>(@"Images\AlphaBlueBall");
            Texture2D alphaPinkTexture = Content.Load<Texture2D>(@"Images\AlphaPinkBall");
            Texture2D alphaBlackTexture = Content.Load<Texture2D>(@"Images\AlphaBlackBall");
            Texture2D alphaWhiteTexture = Content.Load<Texture2D>(@"Images\AlphaWhiteBall");

            ballOn1 = new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaBlackTexture, shadowTexture, lights, new Vector2(ballOn1Position.X, ballOn1Position.Y), new Vector2(16f, 16f), "ballOn1", this, (int)BallValues.Red);
            ballOn2 = new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaBlackTexture, shadowTexture, lights, new Vector2(ballOn2Position.X, ballOn2Position.Y), new Vector2(16f, 16f), "ballOn2", this, (int)BallValues.Red);

            ballSprites.Clear();
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\WhiteBall"), alphaWhiteTexture, shadowTexture, lights, new Vector2(469, 140), new Vector2(16f, 16f), "white", this, (int)BallValues.White));

            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(91, 134), new Vector2(16f, 16f), "red01", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(91, 152), new Vector2(16f, 16f), "red02", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(91, 171), new Vector2(16f, 16f), "red03", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(91, 190), new Vector2(16f, 16f), "red04", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(91, 208), new Vector2(16f, 16f), "red05", this, (int)BallValues.Red));

            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(108, 144), new Vector2(16f, 16f), "red06", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(108, 162), new Vector2(16f, 16f), "red07", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(108, 180), new Vector2(16f, 16f), "red08", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(108, 198), new Vector2(16f, 16f), "red09", this, (int)BallValues.Red));

            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(125, 152), new Vector2(16f, 16f), "red10", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(125, 171), new Vector2(16f, 16f), "red11", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(125, 190), new Vector2(16f, 16f), "red12", this, (int)BallValues.Red));

            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(142, 162), new Vector2(16f, 16f), "red13", this, (int)BallValues.Red));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(142, 180), new Vector2(16f, 16f), "red14", this, (int)BallValues.Red));

            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\RedBall"), alphaRedTexture, shadowTexture, lights, new Vector2(159, 171), new Vector2(16f, 16f), "red15", this, (int)BallValues.Red));

            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\YellowBall"), alphaYellowTexture, shadowTexture, lights, new Vector2(469, 115), new Vector2(16f, 16f), "yellow", this, (int)BallValues.Yellow));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\GreenBall"), alphaGreenTexture, shadowTexture, lights, new Vector2(469, 228), new Vector2(16f, 16f), "green", this, (int)BallValues.Green));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\BrownBall"), alphaBrownTexture, shadowTexture, lights, new Vector2(469, 171), new Vector2(16f, 16f), "brown", this, (int)BallValues.Brown));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\BlueBall"), alphaBlueTexture, shadowTexture, lights, new Vector2(298, 171), new Vector2(16f, 16f), "blue", this, (int)BallValues.Blue));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\PinkBall"), alphaPinkTexture, shadowTexture, lights, new Vector2(178, 171), new Vector2(16f, 16f), "pink", this, (int)BallValues.Pink));
            ballSprites.Add(new Ball(Content.Load<Texture2D>(@"Images\BlackBall"), alphaBlackTexture, shadowTexture, lights, new Vector2(50, 171), new Vector2(16f, 16f), "black", this, (int)BallValues.Black));

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            foreach (Sprite sprite in ballSprites)
            {
                sprite.Texture.Dispose();
            }
            backgroundTexture.Dispose();
            backgroundTextureBlue.Dispose();
            backgroundTextureGreen.Dispose();
            backgroundTextureRed.Dispose();
            xnaSnookerTexture.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            //if (this.IsActive)
            //{
            ballPointerRotateDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            ballPointerFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            changeBallPointerDelay -= gameTime.ElapsedGameTime.TotalSeconds;
            roofFanRotateDelay -= gameTime.ElapsedGameTime.TotalSeconds;

            cursorSprite.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

            switch (currentGameState)
            {
                case GameState.SignIn:
                    visualKeyboard.MouseOver(new Vector2(Mouse.GetState().X - keyboardRectangle.X, Mouse.GetState().Y - keyboardRectangle.Y));

                    if (this.IsActive)
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            visualKeyboard.KeyPressed(new Vector2(Mouse.GetState().X - keyboardRectangle.X, Mouse.GetState().Y - keyboardRectangle.Y));
                        }
                        else if (Mouse.GetState().LeftButton == ButtonState.Released)
                        {
                            keyboardDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                            if (keyboardDelay <= 0)
                            {
                                keyboardDelay = maxKeyboardDelay;
                                visualKeyboard.KeyReleased(new Vector2(Mouse.GetState().X - keyboardRectangle.X, Mouse.GetState().Y - keyboardRectangle.Y));
                            }
                        }
                    }
                    if (Gamer.SignedInGamers.Count < 1)
                    {
                        //log.Add("Opened User SignIn Interface");
                    }
                    else
                    {
                        currentGameState = GameState.Play;
                    }
                    break;
                case GameState.Setup:
                    Vector2 clickPosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    if (this.IsActive)
                    {
                        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
                        {
                            if (rbtSinglePlayer.TestClick(clickPosition))
                            {
                                if (!rbtSinglePlayer.Checked)
                                {
                                    soundBank.PlayCue(GameSound.Click02.ToString());
                                    rbtLevelEasy.Checked = false;
                                    rbtLevelNormal.Checked = true;
                                    rbtLevelHard.Checked = false;
                                    backgroundTexture = backgroundTextureBlue;
                                    maxComputerAttempts = 10;
                                }

                                rbtSinglePlayer.Checked = true;
                                rbtMultiPlayer.Checked = false;
                                signInPlayer.Texture = signInPictureSprite.Texture;
                            }
                            else if (rbtMultiPlayer.TestClick(clickPosition))
                            {
                                if (!rbtMultiPlayer.Checked)
                                {
                                    soundBank.PlayCue(GameSound.Click02.ToString());
                                    rbtLevelEasy.Checked = false;
                                    rbtLevelNormal.Checked = false;
                                    rbtLevelHard.Checked = false;
                                }

                                rbtSinglePlayer.Checked = false;
                                rbtMultiPlayer.Checked = true;
                            }
                            else if (btnSignIn.TestClick(clickPosition))
                            {
                                soundBank.PlayCue(GameSound.Click02.ToString());
                                btnSignInClicked();
                            }
                            else if (rbtLevelEasy.TestClick(clickPosition) && rbtSinglePlayer.Checked)
                            {
                                if (!rbtLevelEasy.Checked)
                                    soundBank.PlayCue(GameSound.Click02.ToString());

                                rbtLevelEasy.Checked = true;
                                rbtLevelNormal.Checked = false;
                                rbtLevelHard.Checked = false;
                                backgroundTexture = backgroundTextureGreen;
                                maxComputerAttempts = 1;
                            }
                            else if (rbtLevelNormal.TestClick(clickPosition) && rbtSinglePlayer.Checked)
                            {
                                if (!rbtLevelNormal.Checked)
                                    soundBank.PlayCue(GameSound.Click02.ToString());

                                rbtLevelNormal.Checked = true;
                                rbtLevelEasy.Checked = false;
                                rbtLevelHard.Checked = false;
                                backgroundTexture = backgroundTextureBlue;
                                maxComputerAttempts = 10;
                            }
                            else if (rbtLevelHard.TestClick(clickPosition) && rbtSinglePlayer.Checked)
                            {
                                if (!rbtLevelHard.Checked)
                                    soundBank.PlayCue(GameSound.Click02.ToString());

                                rbtLevelHard.Checked = true;
                                rbtLevelEasy.Checked = false;
                                rbtLevelNormal.Checked = false;
                                backgroundTexture = backgroundTextureRed;
                                maxComputerAttempts = 20;
                            }
                            else
                            {
                                if (
                                Mouse.GetState().X >= team1Player1PictureRectangle.X &&
                                Mouse.GetState().X <= (team1Player1PictureRectangle.X + team1Player1PictureRectangle.Width) &&
                                Mouse.GetState().Y >= team1Player1PictureRectangle.Y &&
                                Mouse.GetState().Y <= (team1Player1PictureRectangle.Y + team1Player1PictureRectangle.Height))
                                {
                                    System.Windows.Forms.IDataObject iDataObject = System.Windows.Forms.Clipboard.GetDataObject();
                                    Drawing.Bitmap sourceBitmap = null;
                                    if (iDataObject.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
                                    {
                                        string[] fileNames = iDataObject.GetData(System.Windows.Forms.DataFormats.FileDrop, true) as string[];
                                        if (fileNames.Length > 0)
                                        {
                                            string photoPath = fileNames[0];
                                            try
                                            {
                                                sourceBitmap = new Drawing.Bitmap(photoPath);
                                            }
                                            catch { }
                                        }
                                    }
                                    else if (iDataObject.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap))
                                    {
                                        sourceBitmap = (Drawing.Bitmap)iDataObject.GetData(System.Windows.Forms.DataFormats.Bitmap);
                                    }

                                    //if the bitmap doesn't get loaded, then or the file is corrupted or is of a wrong type,
                                    //so we'll just ignore that image.
                                    if (sourceBitmap != null)
                                    {
                                        float sourcePictureRatio = (float)sourceBitmap.Width / (float)sourceBitmap.Height;
                                        float targetPictureRatio = (float)team1Player1PictureRectangle.Width / (float)team1Player1PictureRectangle.Height;

                                        Drawing.Image targetImage = new Drawing.Bitmap(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height);
                                        Drawing.Image resizedImage = null;

                                        //resizing to fit into the target rectangle
                                        using (Drawing.Graphics g = Drawing.Graphics.FromImage(targetImage))
                                        {
                                            g.Clear(Drawing.Color.Black);
                                            if (sourcePictureRatio < targetPictureRatio)
                                            {
                                                float scale = (float)team1Player1PictureRectangle.Height / (float)sourceBitmap.Height;
                                                //resizedImage = new Drawing.Bitmap(sourceBitmap, new Drawing.Size((int)(sourceBitmap.Width * scale), (int)(sourceBitmap.Height * scale)));
                                                resizedImage = new Drawing.Bitmap(sourceBitmap, new Drawing.Size((int)(sourceBitmap.Width * scale), (int)(sourceBitmap.Height * scale)));
                                                g.DrawImage(resizedImage, new Drawing.Point((targetImage.Size.Width - resizedImage.Width) / 2, 0));
                                            }
                                            else
                                            {
                                                float scale = (float)team1Player1PictureRectangle.Width / (float)sourceBitmap.Width;
                                                resizedImage = new Drawing.Bitmap(sourceBitmap, new Drawing.Size((int)(sourceBitmap.Width * scale), (int)(sourceBitmap.Height * scale)));
                                                g.DrawImage(resizedImage, new Drawing.Point(0, (targetImage.Size.Height - resizedImage.Height) / 2));
                                            }
                                        }

                                        targetImage.Save("tempImage");
                                        signInPictureSprite.Texture = Texture2D.FromFile(GraphicsDevice, "tempImage");
                                        Texture2D texture = signInPictureSprite.Texture;
                                        signInPlayer.ImageByteArray = new byte[4 * texture.Width * texture.Height];
                                        signInPlayer.Texture = signInPictureSprite.Texture;
                                        texture.GetData<byte>(signInPlayer.ImageByteArray);
                                        clickHereSprite.Texture = null;
                                    }
                                    System.Windows.Forms.Clipboard.Clear();
                                }
                            }
                        }
                    }
                    break;
                case GameState.ShowOpponents:
                    if (rbtSinglePlayer.Checked || (rbtMultiPlayer.Checked && teams != null))
                    {
                        if (rbtSinglePlayer.Checked || (rbtMultiPlayer.Checked && teams.Count > 1))
                        {
                            if (showOpponentsDelay > 0)
                            {
                                showOpponentsDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                            }
                            else
                            {
                                showOpponentsFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                                if (showOpponentsFadeDelay > 0)
                                {
                                    if (team1Player1PictureSprite.Alpha >= 0)
                                        team1Player1PictureSprite.Alpha -= 10;
                                    if (team2Player1PictureSprite.Alpha >= 0)
                                        team2Player1PictureSprite.Alpha -= 10;
                                }
                                else
                                {
                                    if (teams[1].Players.Count > 0)
                                        UpdateGameState(GameState.Play);
                                }
                            }
                        }
                    }
                    break;
                case GameState.Play:
                    {
                        if (ballPointerRotateDelay <= 0)
                        {
                            ballPointerSprite.Angle += 0.1f;
                            ballPointerRotateDelay = maxBallPointerFadeDelay;
                        }

                        if (changeBallPointerDelay <= 0)
                        {
                            if (teams[playingTeamID - 1].BallOn != null)
                            {
                                string oldId = teams[playingTeamID - 1].BallOn.Id;
                                teams[playingTeamID - 1].BallOn = GetEquivalentBallOn();
                                string newId = teams[playingTeamID - 1].BallOn.Id;
                                if (oldId != newId)
                                {
                                    ballPointerSprite.Angle = 0f;
                                }
                            }
                            changeBallPointerDelay = maxChangeBallPointerDelay;
                        }

                        if (ballPointerFadeDelay <= 0)
                        {
                            ballPointerSprite.Fade();
                            ballPointerFadeDelay = maxBallPointerFadeDelay;
                        }

                        foreach (Ball b in ballSprites)
                        {
                            b.DrawPosition = b.Position;
                        }

                        ballPointerSprite.Position = new Vector2(0, 0);
                        if (teams[playingTeamID - 1].BallOn != null)
                        {
                            if (!teams[playingTeamID - 1].BallOn.IsBallInPocket)
                                ballPointerSprite.Position = new Vector2(teams[playingTeamID - 1].BallOn.Position.X, teams[playingTeamID - 1].BallOn.Position.Y);
                            else
                                ballPointerSprite.Position = new Vector2(-100, -100);
                        }

                        cursorSprite.Position = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);

                        //Decrement the delay by the number of seconds that have elapsed since            
                        //the last time that the Update method was called           
                        fallingBallFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                        movingBallDelay -= gameTime.ElapsedGameTime.TotalSeconds;

                        // Allows the game to exit
                        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                            this.Exit();

                        bool someFalling = false;
                        if (poolState == PoolState.AwaitingShot)
                        {

                            someFalling = ProcessSomeFalling();

                            if (!someFalling)
                            {
                                if (lastPoolState == PoolState.MovingBalls)
                                {
                                    if (!fallenBallsProcessed)
                                    {
                                        ProcessFallenBalls();
                                        foreach (Ball b in ballSprites)
                                        {
                                            b.DrawPosition = b.Position;
                                        }
                                        CreateSnapshot(GetBallPositionList());

                                        if (rbtMultiPlayer.Checked)
                                        {
                                            logList.Add(string.Format("Player {0} sent {1} snapshots", contractPerson.Name, outgoingShot.SnapshotList.Count()));
                                            outgoingShot.CurrentTeamScore = teams[0].Points;
                                            outgoingShot.OtherTeamScore = teams[1].Points;
                                            SnookerServiceAgent.GetInstance().Play(contractTeam, contractPerson, outgoingShot);

                                            ResetOutgoingShot();
                                        }
                                    }
                                }
                            }

                            if (teams[playingTeamID - 1].Id == 2 && rbtSinglePlayer.Checked)
                            {

                            }
                            else
                            {
                                if (Mouse.GetState().LeftButton == ButtonState.Pressed && contractTeam != null)
                                {
                                    if (contractTeam.Id == playingTeamID)
                                    {
                                        if (Mouse.GetState().X >= poolRectangle.X &&
                                            Mouse.GetState().X <= poolRectangle.X + poolRectangle.Width &
                                            Mouse.GetState().Y >= poolRectangle.Y &&
                                            Mouse.GetState().Y <= poolRectangle.Y + poolRectangle.Height)
                                        {

                                            if (poolState == PoolState.AwaitingShot)
                                            {
                                                cueDistance = 0;
                                                hitPosition = new Vector2(Mouse.GetState().X - poolRectangle.X - 3, Mouse.GetState().Y - poolRectangle.Y - 3);
                                                poolState = PoolState.PreparingCue;
                                            }
                                        }
                                        else
                                        {
                                            TrySetTargetOrStrength(Mouse.GetState().X, Mouse.GetState().Y);
                                        }
                                    }
                                }
                            }

                            if (contractTeam != null)
                            {
                                if (playingTeamID == contractTeam.Id)// & incomingSnapshotQueue.Count == 0)
                                {
                                    if (teams[playingTeamID - 1].Id == 2 && rbtSinglePlayer.Checked)
                                    {
                                        //UpdateCuePosition(cueDistance, (int)teams[playingTeamID - 1].TestPosition.X, (int)teams[playingTeamID - 1].TestPosition.Y);
                                    }
                                    else
                                    {
                                        UpdateCuePosition(cueDistance, Mouse.GetState().X, Mouse.GetState().Y);
                                    }
                                }
                                else
                                {
                                    if (contractTeam.Id != playingTeamID)
                                    {
                                        if (currentIncomingShot == null)
                                        {
                                            if (incomingShotList.Count > 0)
                                            {
                                                currentIncomingShot = incomingShotList[0];
                                                incomingShotList.RemoveAt(0);
                                            }
                                        }

                                        if (currentIncomingShot != null)
                                        {
                                            if (currentSnapshotNumber < currentIncomingShot.SnapshotList.Length)
                                            {
                                                if (movingBallDelay <= 0)
                                                {
                                                    movingBallDelay = maxMovingBallDelay;

                                                    foreach (BallPosition bp in currentIncomingShot.SnapshotList[currentSnapshotNumber].ballPositionList)
                                                    {
                                                        ballSprites[bp.ballIndex].Position = new Vector2(bp.x, bp.y);
                                                        ballSprites[bp.ballIndex].IsBallInPocket = bp.isBallInPocket;
                                                    }
                                                    GameSound sound = currentIncomingShot.SnapshotList[currentSnapshotNumber].sound;
                                                    if (sound != GameSound.None)
                                                    {
                                                        soundBank.PlayCue(sound.ToString());
                                                    }
                                                    currentSnapshotNumber++;
                                                }
                                            }
                                            else
                                            {
                                                teams[0].Points = currentIncomingShot.CurrentTeamScore;
                                                teams[1].Points = currentIncomingShot.OtherTeamScore;

                                                if (currentIncomingShot.GameOver)
                                                {
                                                    UpdateGameState(GameState.GameOver);
                                                }
                                                else if (currentIncomingShot.HasFinishedTurn)
                                                {
                                                    playingTeamID = (playingTeamID == 1) ? 2 : 1;
                                                    awaitingTeamID = (playingTeamID == 1) ? 2 : 1;
                                                    logList.Add(string.Format("Team {0} is ready to play", playingTeamID));
                                                    logList.Add(string.Format("Team {0} is waiting", awaitingTeamID));

                                                    teams[playingTeamID - 1].BallOn = GetRandomRedBall();

                                                    if (teams[playingTeamID - 1].BallOn == null)
                                                        teams[playingTeamID - 1].BallOn = GetMinColouredball();
                                                }
                                                currentSnapshotNumber = 0;
                                                currentIncomingShot = null;
                                            }
                                        }
                                    }
                                }

                                if (teams[playingTeamID - 1].Players[0].Name == "Computer")
                                {
                                    UpdateGameState(GameState.TestShot);
                                }
                            }
                        }
                        else if (poolState == PoolState.MovingBalls)
                        {
                            if (!calculatingPositions)
                            {
                                if (movingBallDelay <= 0)
                                {
                                    movingBallDelay = maxMovingBallDelay;
                                    List<SnookerCore.BallPosition> newBPList = MoveBalls();

                                    CreateSnapshot(newBPList);
                                }
                            }
                        }
                        else if (poolState == PoolState.PreparingCue)
                        {
                            if (cueDistance >= maxCueDistance)
                            {
                                poolState = PoolState.ShootingCue;
                            }
                        }
                        else if (poolState == PoolState.ShootingCue)
                        {
                            if (cueDistance <= 0)
                            {
                                cueDistance = 0;
                                HitBall((int)hitPosition.X, (int)hitPosition.Y);
                            }
                        }

                        foreach (Ball ball in ballSprites)
                        {
                            if (ball.IsBallInPocket)
                            {
                                if (ball.AlphaValue >= 0)
                                {
                                    ball.AlphaValue -= fallingBallFadeDecrement;
                                    if (ball.AlphaValue < 0)
                                        ball.AlphaValue = 0;
                                }
                            }
                        }

                        targetSprite.Position = new Vector2((int)(targetRectangle.X + targetRectangle.Width / 2 + targetVector.X - targetSprite.Size.X / 2), (int)(targetRectangle.Y + targetRectangle.Height / 2 + targetVector.Y - targetSprite.Size.Y / 2));

                        base.Update(gameTime);
                    }
                    break;
                case GameState.TestShot:
                    if (teams[playingTeamID - 1].Id == 2 && rbtSinglePlayer.Checked)
                    {
                        //UpdateCuePosition(cueDistance, (int)teams[playingTeamID - 1].TestPosition.X, (int)teams[playingTeamID - 1].TestPosition.Y);
                    }
                    break;
                case GameState.GameOver:
                    gameOverFadeDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (gameOverFadeDelay > 0)
                    {
                        if (gameOverAlpha > 0)
                            gameOverAlpha--;
                    }
                    else
                    {
                        gameOverFadeDelay = maxGameOverFadeDelay;
                        gameOverAlpha = 255;
                        ResetGame();
                        UpdatePlayerState(PlayerState.Aiming);
                        UpdateGameState(GameState.SignIn);
                    }
                    break;
            }

            if (currentGameState == GameState.TestShot)
            {
                bool someFalling = false;
                someFalling = ProcessSomeFalling();
                if (teams[playingTeamID - 1].Players[0].Name == "Computer" && !teams[playingTeamID - 1].IsRotatingCue && !someFalling)
                {
                    cueSprite.Texture = (teams[playingTeamID - 1].Id == 1 ? cueTexture1 : cueTexture2);

                    GenerateComputerShot();
                }
                else if (teams[playingTeamID - 1].Players[0].Name == "Computer" && teams[playingTeamID - 1].IsRotatingCue)
                {
                    UpdateCuePosition(0);
                    rotatingCueDelay -= gameTime.ElapsedGameTime.TotalSeconds;
                    if (rotatingCueDelay < 0)
                    {
                        int xIncrement = (cueSprite.Target.X < cueSprite.NewTarget.X ? 8 : -8);
                        int yIncrement = (cueSprite.Target.Y < cueSprite.NewTarget.Y ? 8 : -8);

                        rotatingCueDelay = maxRotatingCueDelay;
                        cueSprite.Target += new Vector2(xIncrement, yIncrement);

                        if (Math.Abs(cueSprite.Target.X - cueSprite.NewTarget.X) < 10 &&
                            Math.Abs(cueSprite.Target.Y - cueSprite.NewTarget.Y) < 10)
                        {
                            cueSprite.Target = new Vector2(cueSprite.NewTarget.X, cueSprite.NewTarget.Y);
                            teams[playingTeamID - 1].IsRotatingCue = false;

                            if (teams[playingTeamID - 1].BestShotSelected)
                            {
                                UpdateGameState(GameState.Play);
                            }
                        }
                    }
                }
            }
        }

        private bool ProcessSomeFalling()
        {
            bool someFalling = false;
            foreach (Ball ball in ballSprites)
            {
                if (ball.IsBallInPocket)
                {
                    if (ball.AlphaValue != 0)
                    {
                        if (fallingBallFadeDelay <= 0)
                        {
                            fallingBallFadeDelay = maxFallingBallFadeDelay;
                            if (ball.AlphaValue >= 0)
                            {
                                ball.AlphaValue -= fallingBallFadeDecrement;
                                if (ball.AlphaValue < 0)
                                    ball.AlphaValue = 0;
                            }
                        }
                        someFalling = true;
                    }
                }
            }
            return someFalling;
        }

        private static void CreateSnapshot(List<SnookerCore.BallPosition> newBPList)
        {
            if (rbtMultiPlayer.Checked)
            {
                List<SnookerCore.Snapshot> currentSnapshotList = outgoingShot.SnapshotList.ToList();
                SnookerCore.Snapshot newSnapshot = new Snapshot();
                newSnapshot.ballPositionList = newBPList.ToArray();
                newSnapshot.snapshotNumber = currentSnapshotNumber;
                newSnapshot.sound = GameSound.None;
                currentSnapshotList.Add(newSnapshot);
                outgoingShot.SnapshotList = currentSnapshotList.ToArray();
                currentSnapshotNumber++;
            }
        }

        private void PlayCue(GameSound sound)
        {
            if (rbtMultiPlayer.Checked)
                outgoingShot.SnapshotList[currentSnapshotNumber - 1].sound = sound;
            soundBank.PlayCue(sound.ToString());
        }

        private void btnSignInClicked()
        {
            if (rbtSinglePlayer.Checked)
            {
                SetupSinglePlayerGame();
                DrawStrengthBar();
                UpdateGameState(GameState.ShowOpponents);

                logList.Add(string.Format("{0} has signed in.", signInPlayer.Name));
            }
            else if (rbtMultiPlayer.Checked)
            {
                team1Player1PictureSprite = new PictureSprite(signInPlayer.Texture, new Vector2(team1Player1PictureRectangle.X, team1Player1PictureRectangle.Y), new Vector2(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height));
                team2Player1PictureSprite = new PictureSprite(computerTexture, new Vector2(team1Player1PictureRectangle.X, team1Player1PictureRectangle.Y), new Vector2(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height));

                SnookerServiceAgent.GetInstance().ProxyEvent += new SnookerServiceAgent.ProxyEventHandler(SnookerServiceAgent_ProxyEvent);
                SnookerServiceAgent.GetInstance().ProxyCallBackEvent += new SnookerServiceAgent.ProxyCallBackEventHandler(SnookerServiceAgent_ProxyCallBackEvent);

                ContractPerson p = new ContractPerson();
                p.Name = signInPlayer.Name;
                p.ImageByteArray = signInPlayer.ImageByteArray;
                SnookerServiceAgent.GetInstance().Connect(p);
                UpdateGameState(GameState.ShowOpponents);

                logList.Add(string.Format("{0} has signed in. Awaiting confirmation from WCF service", signInPlayer.Name));
            }
        }

        private void DrawStrengthBar()
        {
            int x = (int)((teams[playingTeamID - 1].Strength / 100.0) * strengthRectangle.Width + strengthRectangle.X);
            strengthSprite.Size = new Vector2(x - strengthRectangle.X, 4);
        }

        private void SetupSinglePlayerGame()
        {
            contractTeam = new ContractTeam();
            contractTeam.Id = 1;

            teams = new List<Team>();
            Team team1 = new Team(1);
            team1Player1PictureSprite = new PictureSprite(signInPlayer.Texture, new Vector2(team1Player1PictureRectangle.X, team1Player1PictureRectangle.Y), new Vector2(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height));
            team1.Players.Add(signInPlayer);
            team1.BallOn = ballSprites[1];
            team1.NextPlayer();
            Team team2 = new Team(2);
            team2Player1PictureSprite = new PictureSprite(computerTexture, new Vector2(team1Player1PictureRectangle.X, team1Player1PictureRectangle.Y), new Vector2(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height));
            team2.Players.Add(computerPlayer);
            team2.BallOn = ballSprites[1];
            team2.NextPlayer();

            teams.Add(team1);
            teams.Add(team2);


            cueSprite.Texture = (teams[playingTeamID - 1].Id == 1 ? cueTexture1 : cueTexture2);

            LoadPlayerTextures(team1, team2);
        }

        private void SetupMultiPlayerGame(ContractTeam[] receivedTeams)
        {
            foreach (ContractTeam t in receivedTeams)
            {
                Team team = new Team(t.Id);
                team.BallOn = ballSprites[1];
                foreach (ContractPerson p in t.Players)
                {
                    logList.Add(string.Format("Confirmed: team {0}, player: {1}", team.Id, p.Name));

                    if (t.Id == 1)
                    {
                        if (p.ImageByteArray != null)
                            team1Player1PictureSprite.Texture.SetData<byte>(p.ImageByteArray);
                    }
                    else
                    {
                        if (p.ImageByteArray != null)
                            team2Player1PictureSprite.Texture.SetData<byte>(p.ImageByteArray);
                    }

                    Player player = new Player(p.Name);
                    team.Players.Add(player);

                    if (p.Name == signInPlayer.Name)
                    {
                        contractTeam = t;
                        contractPerson = p;

                        teams[playingTeamID - 1].Players.Add(player);
                        cueSprite.Texture = (teams[playingTeamID - 1].Id == 1 ? cueTexture1 : cueTexture2);
                    }
                    else
                    {
                        teams[awaitingTeamID - 1].Players.Add(player);
                    }
                }

                teams[team.Id - 1] = team;
            }

            picture1Sprite.Texture = team1Player1PictureSprite.Texture;
            picture2Sprite.Texture = team2Player1PictureSprite.Texture;

            ResetOutgoingShot();
        }

        private static void LoadPlayerTextures(Team team1, Team team2)
        {
            picture1Sprite.Texture = team1.CurrentPlayer.Texture;
            picture2Sprite.Texture = team2.CurrentPlayer.Texture;
        }

        private void UpdateGameState(GameState gameState)
        {
            switch (gameState)
            {
                case GameState.SignIn:
                    break;
                case GameState.GameOver:
                    break;
            }
            lastGameState = currentGameState;
            currentGameState = gameState;
        }

        private void UpdateCuePosition(int cueDistance, int targetX, int targetY)
        {
            if (targetX >= poolRectangle.X &&
                targetX <= poolRectangle.X + poolRectangle.Width &
                targetY >= poolRectangle.Y &&
                targetY <= poolRectangle.Y + poolRectangle.Height)
            {
                cueSprite.Target = new Vector2(targetX, targetY);
                UpdateCuePosition(cueDistance);
            }
        }

        private void UpdateCuePosition(int cueDistance)
        {
            int targetX = (int)cueSprite.Target.X;
            int targetY = (int)cueSprite.Target.Y;

            float dx = targetX - poolRectangle.X - ballSprites[0].DrawPosition.X;
            float dy = targetY - poolRectangle.Y - ballSprites[0].DrawPosition.Y;
            float h = (float)Math.Sqrt(dx * dx + dy * dy);

            float rotationAngle = 0;
            float shadowRotationAngle = 0;

            if (h == 0)
            {
                rotationAngle = 0;
            }
            else if (dx == 0 && dy == 0)
            {
                rotationAngle = 0;
            }
            else if (dx > 0 && dy > 0)
            {
                rotationAngle = (float)(MathHelper.Pi) + (float)Math.Acos(dx / h);
            }
            else if (dx > 0 && dy < 0)
            {
                rotationAngle = (float)(MathHelper.Pi) - (float)Math.Acos(dx / h);
            }
            else if (dx < 0 && dy > 0)
            {
                rotationAngle = (float)(MathHelper.Pi) + (float)Math.Acos(dx / h);
            }
            else if (dx < 0 && dy < 0)
            {
                rotationAngle = (float)(MathHelper.Pi) - (float)Math.Acos(dx / h);
            }

            rotationAngle = rotationAngle % (MathHelper.Pi * 2f);

            if (h == 0)
            {
                shadowRotationAngle = 0;
            }
            else if (dx == 0 && dy == 0)
            {
                shadowRotationAngle = 0;
            }
            else if (dx > 0 && dy > 0)
            {
                shadowRotationAngle = rotationAngle + 0.05f * (rotationAngle - MathHelper.Pi * (3f / 2f));
            }
            else if (dx > 0 && dy < 0)
            {
                shadowRotationAngle = rotationAngle + 0.05f * (MathHelper.Pi * (1f / 2f) - rotationAngle);
            }
            else if (dx < 0 && dy > 0)
            {
                shadowRotationAngle = rotationAngle - 0.05f * (MathHelper.Pi * (3f / 2f) - rotationAngle);
            }
            else if (dx < 0 && dy < 0)
            {
                shadowRotationAngle = rotationAngle - 0.05f * (rotationAngle - MathHelper.Pi * (1f / 2f));
            }

            cueSprite.Origin = new Vector2(cueSpriteOrigin.X - cueDistance, cueSpriteOrigin.Y);
            cueSprite.RotationAngle = rotationAngle;
            cueSprite.ShadowRotationAngle = shadowRotationAngle;

            cueSprite.Position = new Vector2(ballSprites[0].DrawPosition.X + poolRectangle.X + (int)Ball.Radius / 2, ballSprites[0].DrawPosition.Y + poolRectangle.Y + (int)Ball.Radius / 2);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin(SpriteBlendMode.AlphaBlend, SpriteSortMode.Immediate, SaveStateMode.None);

            spriteBatch.Draw(backgroundTexture, new Rectangle(0, 0,
            backgroundTexture.Width,
            backgroundTexture.Height),
            Color.LightGray);

            Vector2 textPosition = new Vector2(name1Rectangle.X, name1Rectangle.Y);

            if (teams != null)
            {
                if (teams.Count > 1)
                {
                    if (teams[0].CurrentPlayer != null)
                    {
                        textPosition = new Vector2(name1Rectangle.X, name1Rectangle.Y);
                        textPosition = DrawString(textPosition, teams[0].CurrentPlayer.Name, 0.80f);
                        textPosition = new Vector2(score1Rectangle.X, score1Rectangle.Y);
                        textPosition = DrawString(textPosition, teams[0].Points.ToString(), 0.80f);
                    }

                    if (teams[1].CurrentPlayer != null)
                    {
                        textPosition = new Vector2(name2Rectangle.X, name2Rectangle.Y);
                        textPosition = DrawString(textPosition, teams[1].CurrentPlayer.Name, 0.80f);
                        textPosition = new Vector2(score2Rectangle.X, score2Rectangle.Y);
                        textPosition = DrawString(textPosition, teams[1].Points.ToString(), 0.80f);
                    }
                }
            }

            if (currentGameState == GameState.SignIn)
            {
                textPosition = new Vector2(signInNameRectangle.X, signInNameRectangle.Y);
                textPosition = DrawString(textPosition, signInPlayer.Name, 0.80f);
            }

            if (picture1Sprite.Texture != null)
            {
                picture1Sprite.Position = new Vector2(picture1Rectangle.X, picture1Rectangle.Y);
                picture1Sprite.Size = new Vector2(picture1Rectangle.Width, picture1Rectangle.Height);
                picture1Sprite.Draw(spriteBatch, (playingTeamID == 1) ? 255 : 64);
            }
            if (picture2Sprite.Texture != null)
            {
                picture2Sprite.Position = new Vector2(picture2Rectangle.X, picture2Rectangle.Y);
                picture2Sprite.Size = new Vector2(picture2Rectangle.Width, picture2Rectangle.Height);
                picture2Sprite.Draw(spriteBatch, (playingTeamID == 2) ? 255 : 64);
            }

            if (currentGameState == GameState.Setup)
            {
                if (signInPictureSprite.Texture != null)
                {
                    signInPictureSprite.Position = new Vector2(team1Player1PictureRectangle.X, team1Player1PictureRectangle.Y);
                    signInPictureSprite.Size = new Vector2(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height);
                    signInPictureSprite.Draw(spriteBatch);
                }

                if (clickHereSprite.Texture != null)
                    clickHereSprite.Draw(spriteBatch, 255, 0.75f);

                //XNAControls
                Vector2 playersPosition = new Vector2(playerOptionsRectangle.X, playerOptionsRectangle.Y);
                playersPosition = rbtSinglePlayer.DrawString(spriteBatch, playersPosition, 0.8f);
                Vector2 levelPosition = new Vector2(playersPosition.X + 30, playersPosition.Y);
                levelPosition = rbtLevelEasy.DrawString(spriteBatch, levelPosition, 0.8f);
                levelPosition = rbtLevelNormal.DrawString(spriteBatch, levelPosition, 0.8f);
                levelPosition = rbtLevelHard.DrawString(spriteBatch, levelPosition, 0.8f);
                playersPosition = new Vector2(playerOptionsRectangle.X, levelPosition.Y);
                playersPosition = rbtMultiPlayer.DrawString(spriteBatch, playersPosition, 0.8f);

                btnSignIn.DrawButton(spriteBatch, new Vector2(600, 200), 100, 40, 0.8f);
            }
            else if (currentGameState == GameState.ShowOpponents)
            {
                if (teams != null)
                {
                    if (teams.Count > 0)
                    {
                        if (teams[0].Players.Count > 0)
                        {
                            team1Player1PictureSprite.Position = new Vector2(team1Player1PictureRectangle.X, team1Player1PictureRectangle.Y);
                            team1Player1PictureSprite.Size = new Vector2(team1Player1PictureRectangle.Width, team1Player1PictureRectangle.Height);
                            team1Player1PictureSprite.Draw(spriteBatch);
                        }
                        if (teams[1].Players.Count > 0)
                        {
                            team2Player1PictureSprite.Position = new Vector2(team2Player1PictureRectangle.X, team2Player1PictureRectangle.Y);
                            team2Player1PictureSprite.Size = new Vector2(team2Player1PictureRectangle.Width, team2Player1PictureRectangle.Height);
                            team2Player1PictureSprite.Draw(spriteBatch);
                        }
                    }
                }
            }
            else
            {
                if (currentGameState == GameState.Play || currentGameState == GameState.TestShot)
                {
                    spriteBatch.Draw(xnaSnookerAlphaTexture, new Rectangle(poolRectangle.X + (poolRectangle.Width - xnaSnookerTexture.Width) / 2, poolRectangle.Y + (poolRectangle.Height - xnaSnookerTexture.Height) / 2 + 20,
                    xnaSnookerTexture.Width,
                    xnaSnookerTexture.Height),
                    new Color(255, 255, 255, 8));

                    spriteBatch.Draw(xnaSnookerTexture, new Rectangle(poolRectangle.X + (poolRectangle.Width - xnaSnookerTexture.Width) / 2, poolRectangle.Y + (poolRectangle.Height - xnaSnookerTexture.Height) / 2 + 20,
                    xnaSnookerTexture.Width,
                    xnaSnookerTexture.Height),
                    new Color(255, 255, 255, 64));
                }
            }

            if (currentGameState == GameState.Play || currentGameState == GameState.TestShot)
            {
                if (true)//currentGameState != GameState.TestShot)
                {
                    foreach (Ball ball in ballSprites)
                    {
                        ball.DrawShadow(spriteBatch, new Vector2(poolRectangle.X - (float)(Ball.Radius / 2), poolRectangle.Y - (float)(Ball.Radius / 2)));
                    }

                    foreach (Ball ball in ballSprites)
                    {
                        ball.Draw(spriteBatch, new Vector2(poolRectangle.X - (float)(Ball.Radius / 2), poolRectangle.Y - (float)(Ball.Radius / 2)));
                    }
                }

                targetSprite.Draw(spriteBatch, new Vector2(0, 0));
                strengthSprite.Draw(spriteBatch, new Vector2(0, 0));
            }

            switch (currentGameState)
            {
                case GameState.SignIn:
                    {
                        textPosition = new Vector2(poolRectangle.X + 48, poolRectangle.Y + 24);

                        keySprite.Draw(spriteBatch, new Vector2(0, 0));
                        keyboardSprite.Draw(spriteBatch, new Vector2(0, 0));
                    }
                    break;
                case GameState.Play:
                    {
                        if (contractTeam != null)
                        {
                            if (poolState == PoolState.AwaitingShot)
                            {
                                if (playingTeamID == contractTeam.Id)
                                {
                                    cueSprite.DrawShadow(spriteBatch, new Vector2(0, 0));
                                    cueSprite.Draw(spriteBatch, new Vector2(0, 0));
                                }
                            }
                            else if ((poolState == PoolState.PreparingCue || poolState == PoolState.ShootingCue) && !teams[playingTeamID - 1].IsRotatingCue)
                            {
                                if (poolState == PoolState.PreparingCue)
                                {
                                    if (cueDistance < maxCueDistance)
                                    {
                                        cueDistance += 4;
                                        cueSprite.Origin = new Vector2(cueSpriteOrigin.X - cueDistance, cueSpriteOrigin.Y);
                                    }
                                    else
                                    {
                                        poolState = PoolState.ShootingCue;
                                    }
                                }
                                else if (poolState == PoolState.ShootingCue)
                                {
                                    if (cueDistance > 0)
                                    {
                                        cueDistance -= (int)(60 * (teams[playingTeamID - 1].Strength) / 100.0);
                                        cueSprite.Origin = new Vector2(cueSpriteOrigin.X - cueDistance, cueSpriteOrigin.Y);
                                    }
                                }
                                cueSprite.DrawShadow(spriteBatch, new Vector2(0, 0));
                                cueSprite.Draw(spriteBatch, new Vector2(0, 0));
                            }
                        }
                    }
                    break;
                case GameState.TestShot:
                    cueSprite.DrawShadow(spriteBatch, new Vector2(0, 0));
                    cueSprite.Draw(spriteBatch, new Vector2(0, 0));
                    break;
                case GameState.GameOver:
                    string gameOverText;
                    if (teams[playingTeamID - 1].Points == teams[awaitingTeamID - 1].Points)
                    {
                        gameOverText = "Draw!";
                    }
                    else if (teams[playingTeamID - 1].Points > teams[awaitingTeamID - 1].Points)
                    {
                        gameOverText = string.Format("{0} Wins!", teams[playingTeamID - 1].Players[0].Name);
                    }
                    else
                    {
                        gameOverText = string.Format("{0} Wins!", teams[awaitingTeamID - 1].Players[0].Name);
                    }

                    textPosition = new Vector2(gameOverTextRectangle.X, gameOverTextRectangle.Y);
                    textPosition = DrawString(textPosition, gameOverText, 2.00f, new Color(gameOverAlpha, gameOverAlpha, gameOverAlpha, gameOverAlpha));

                    break;
            }

            if (teams != null)
            {
                if (teams.Count > 1)
                {
                    if (teams[playingTeamID - 1].BallOn != null)
                    {
                        if (!teams[playingTeamID - 1].BallOn.IsBallInPocket)
                        {
                            if (currentGameState == GameState.Play || currentGameState == GameState.TestShot)
                            {
                                ballPointerSprite.Draw(spriteBatch, new Vector2(poolRectangle.X + 4, poolRectangle.Y + 4));
                            }
                        }
                    }
                }
            }

            if (!(cursorSprite.Position.X > poolRectangle.X && cursorSprite.Position.X < poolRectangle.X + poolRectangle.Width &&
                cursorSprite.Position.Y > poolRectangle.Y && cursorSprite.Position.Y < poolRectangle.Y + poolRectangle.Height))
            {
                if (showLogList)
                    DrawLog();
            }

            cursorSprite.Draw(spriteBatch, new Vector2(0, 0));


            GraphicsDevice.RenderState.AlphaBlendEnable = false;

            spriteBatch.End();
            base.Draw(gameTime);
            //}
        }

        private Vector2 DrawString(Vector2 textPosition, string text)
        {
            return DrawString(textPosition, text, 1f, new Color(255, 255, 255, 255));
        }

        private Vector2 DrawString(Vector2 textPosition, string text, float scale)
        {
            return DrawString(textPosition, text, scale, new Color(255, 255, 255, 255));
        }

        private Vector2 DrawString(Vector2 textPosition, string text, float scale, Color color)
        {
            text = text.Replace("\0", "");
            Vector2 measureString = gameFont.MeasureString(text) * scale;
            textPosition += new Vector2(0, measureString.Y);
            spriteBatch.DrawString(gameFont, text, textPosition, color, 0, new Vector2(0, 0), new Vector2(scale, scale), SpriteEffects.None, 0f);
            return textPosition;
        }

        private void DrawLog()
        {
            int offSet = 0;
            if (logList.Count > 0)
            {
                if (logList.Count > 14)
                {
                    offSet = logList.Count - 14;
                }

                Vector2 position = new Vector2(poolRectangle.X + 40, poolRectangle.Y + 20);
                for (int i = offSet; i < logList.Count; i++)
                {
                    position = DrawString(position, logList[i], 0.75f);
                }
            }
        }

        #region IBallObserver Members

        public void Hit(GameSound sound)
        {
            if (currentGameState != GameState.TestShot)
                PlayCue(sound);
        }

        #endregion

        private List<SnookerCore.BallPosition> MoveBalls()
        {
            List<SnookerCore.BallPosition> ballPositionList = new List<SnookerCore.BallPosition>();
            calculatingPositions = true;
            DateTime before = DateTime.Now;
            foreach (Ball ball in ballSprites)
            {
                if (Math.Abs(ball.X) < 5 && Math.Abs(ball.Y) < 5 && Math.Abs(ball.TranslateVelocity.X) < 10 && Math.Abs(ball.TranslateVelocity.Y) < 10)
                {
                    ball.X =
                    ball.Y = 0;

                    ball.TranslateVelocity = new Vector2(0, 0);
                }
            }

            bool conflicted = true;

            while (conflicted)
            {
                conflicted = false;

                bool someCollision = true;
                while (someCollision)
                {
                    foreach (Ball ball in ballSprites)
                    {
                        foreach (Pocket pocket in pockets)
                        {
                            bool inPocket = pocket.IsBallInPocket(ball);
                        }
                    }

                    someCollision = false;
                    foreach (Ball ballA in ballSprites)
                    {
                        if (ballA.IsBallInPocket)
                        {
                            ballA.TranslateVelocity = new Vector2(0, 0);
                        }

                        foreach (DiagonalBorder diagonalBorder in diagonalBorders)
                        {
                            if (diagonalBorder.Colliding(ballA) && !ballA.IsBallInPocket)
                            {
                                diagonalBorder.ResolveCollision(ballA);
                            }
                        }

                        RectangleCollision borderCollision = RectangleCollision.None;
                        foreach (TableBorder tableBorder in tableBorders)
                        {
                            borderCollision = tableBorder.Colliding(ballA);

                            if (borderCollision != RectangleCollision.None && !ballA.IsBallInPocket)
                            {
                                someCollision = true;
                                tableBorder.ResolveCollision(ballA, borderCollision);
                            }
                        }

                        foreach (Ball ballB in ballSprites)
                        {
                            if (ballA.Id.CompareTo(ballB.Id) != 0)
                            {
                                if (ballA.Colliding(ballB) && !ballA.IsBallInPocket && !ballB.IsBallInPocket)
                                {
                                    if (ballA.Points == 0)
                                    {
                                        strokenBalls.Add(ballB);
                                    }
                                    else if (ballB.Points == 0)
                                    {
                                        strokenBalls.Add(ballA);
                                    }

                                    while (ballA.Colliding(ballB))
                                    {
                                        someCollision = true;
                                        ballA.ResolveCollision(ballB);
                                    }
                                }
                            }
                        }

                        if (ballA.IsBallInPocket)
                        {
                            ballA.TranslateVelocity = new Vector2(0, 0);
                            ballA.VSpinVelocity = new Vector2(0, 0);
                        }

                        if (ballA.TranslateVelocity.X != 0.0d ||
                            ballA.TranslateVelocity.Y != 0.0d)
                        {
                            float signalXVelocity = ballA.TranslateVelocity.X >= 0.0f ? 1.0f : -1.0f;
                            float signalYVelocity = ballA.TranslateVelocity.Y >= 0.0f ? 1.0f : -1.0f;
                            float absXVelocity = Math.Abs(ballA.TranslateVelocity.X);
                            float absYVelocity = Math.Abs(ballA.TranslateVelocity.Y);

                            Vector2 absVelocity = new Vector2(absXVelocity, absYVelocity);

                            Vector2 normalizedDiff = new Vector2(absVelocity.X, absVelocity.Y);
                            normalizedDiff.Normalize();

                            absVelocity.X = absVelocity.X * (1.0f - friction) - normalizedDiff.X * friction;
                            absVelocity.Y = absVelocity.Y * (1.0f - friction) - normalizedDiff.Y * friction;

                            if (absVelocity.X < 0f)
                                absVelocity.X = 0f;

                            if (absVelocity.Y < 0f)
                                absVelocity.Y = 0f;

                            float vx = absVelocity.X * signalXVelocity;
                            float vy = absVelocity.Y * signalYVelocity;

                            if (float.IsNaN(vx))
                                vx = 0;

                            if (float.IsNaN(vy))
                                vy = 0;

                            ballA.TranslateVelocity = new Vector2(vx, vy);
                        }

                        if (ballA.VSpinVelocity.X != 0.0d || ballA.VSpinVelocity.Y != 0.0d)
                        {

                            float signalXVelocity = ballA.VSpinVelocity.X >= 0.0f ? 1.0f : -1.0f;
                            float signalYVelocity = ballA.VSpinVelocity.Y >= 0.0f ? 1.0f : -1.0f;
                            float absXVelocity = Math.Abs(ballA.VSpinVelocity.X);
                            float absYVelocity = Math.Abs(ballA.VSpinVelocity.Y);

                            Vector2 absVelocity = new Vector2(absXVelocity, absYVelocity);

                            Vector2 normalizedDiff = new Vector2(absVelocity.X, absVelocity.Y);
                            normalizedDiff.Normalize();

                            absVelocity.X = absVelocity.X - normalizedDiff.X * friction / 1.2f;
                            absVelocity.Y = absVelocity.Y - normalizedDiff.Y * friction / 1.2f;

                            if (absVelocity.X < 0f)
                                absVelocity.X = 0f;

                            if (absVelocity.Y < 0f)
                                absVelocity.Y = 0f;

                            ballA.VSpinVelocity = new Vector2(absVelocity.X * signalXVelocity, absVelocity.Y * signalYVelocity);
                        }
                    }

                    foreach (Ball ball in ballSprites)
                    {
                        ball.Position += new Vector2(ball.TranslateVelocity.X + ball.VSpinVelocity.X, 0f);
                        ball.Position += new Vector2(0f, ball.TranslateVelocity.Y + ball.VSpinVelocity.Y);
                    }
                }

                MoveBall(false);
                conflicted = false;
            }

            double totalVelocity = 0;
            foreach (Ball ball in ballSprites)
            {
                totalVelocity += ball.TranslateVelocity.X;
                totalVelocity += ball.TranslateVelocity.Y;
            }

            calculatingPositions = false;

            if (poolState == PoolState.MovingBalls && totalVelocity == 0)
            {
                if (poolState == PoolState.MovingBalls)
                {
                    MoveBall(true);
                    poolState = PoolState.AwaitingShot;
                }
            }

            ballPositionList = GetBallPositionList();

            return ballPositionList;
        }

        private List<SnookerCore.BallPosition> GetBallPositionList()
        {
            List<SnookerCore.BallPosition> ballPositionList = new List<BallPosition>();
            int index = 0;
            foreach (Ball ball in ballSprites)
            {
                SnookerCore.BallPosition ballPosition = new SnookerCore.BallPosition();
                ballPosition.ballIndex = index;
                ballPosition.snapshotNumber = currentSnapshotNumber;
                ballPosition.x = (int)ball.Position.X;
                ballPosition.y = (int)ball.Position.Y;
                ballPosition.isBallInPocket = ball.IsBallInPocket;
                ballPositionList.Add(ballPosition);
                index++;
            }
            return ballPositionList;
        }

        private void MoveBall(bool forcePaint)
        {
            int index = 0;
            foreach (Ball ball in ballSprites)
            {
                int lastX = (int)ball.LastX;
                int X = (int)ball.X;
                int lastY = (int)ball.LastY;
                int Y = (int)ball.Y;

                if (!ball.IsBallInPocket)
                {
                    ball.LastX = ball.X;
                    ball.LastY = ball.Y;
                }
                index++;
            }

            try
            {
                spriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                foreach (Sprite sprite in ballSprites)
                {
                    sprite.Draw(spriteBatch, new Vector2(poolRectangle.X - (float)(Ball.Radius / 2), poolRectangle.Y - (float)(Ball.Radius / 2)));
                }
                spriteBatch.End();
            }
            catch
            {
            }
        }

        void HitBall(int x, int y)
        {
            poolState = PoolState.MovingBalls;

            //20 is the maximum velocity
            float v = 20f * (teams[playingTeamID - 1].Strength / 100.0f);

            //Calculates the cue angle, and the translate velocity (normal velocity)
            float dx = x - ballSprites[0].Position.X;
            float dy = y - ballSprites[0].Position.Y;
            float h = (float)(Math.Sqrt(Math.Pow(dx, 2) + Math.Pow(dy, 2)));
            float sin = dy / h;
            float cos = dx / h;
            ballSprites[0].IsBallInPocket = false;
            ballSprites[0].TranslateVelocity = new Vector2(v * cos, v * sin);
            Vector2 normalVelocity = new Vector2(ballSprites[0].TranslateVelocity.X, ballSprites[0].TranslateVelocity.Y);
            normalVelocity.Normalize();

            //Calculates the top spin/back spin velocity, in the same direction as the normal velocity, but in opposite angle
            float topBottomVelocityRatio = ballSprites[0].TranslateVelocity.Length() * (targetVector.Y / 100.0f);
            ballSprites[0].VSpinVelocity = new Vector2(-1.0f * topBottomVelocityRatio * normalVelocity.X, -1.0f * topBottomVelocityRatio * normalVelocity.Y);

            //xSound defines if the sound is coming from the left or the right
            double xSound = (float)(ballSprites[0].Position.X - 300.0f) / 300.0f;

            CreateSnapshot(GetBallPositionList());

            if (currentGameState != GameState.TestShot)
            {
                PlayCue(GameSound.Shot01);
            }

            //Calculates the ball positions as long as there are moving balls
            MoveBalls();

            fallenBallsProcessed = false;
        }

        #region IPocketObserver Members

        public void BallDropped(Ball ball)
        {
            if (currentGameState != GameState.TestShot)
                PlayCue(GameSound.Fall);
            else
                Console.WriteLine(string.Format("Ball '{0}' has fallen.", ball.Id));

            fallenBalls.Add(ball);
            pottedBalls.Add(ball);
            ball.IsBallInPocket = true;
        }

        private void ProcessFallenBalls()
        {
            fallenBallsProcessed = true;
            teams[playingTeamID - 1].FoulList.Clear();

            int redCount = 0;
            int fallenRedCount = 0;
            int wonPoints = 0;
            int lostPoints = 0;
            bool someInTable = false;

            foreach (Ball ball in ballSprites)
            {
                if (!ball.IsBallInPocket)
                {
                    if (ball.Points > 0)
                        someInTable = true;
                }

                if (ball.Points == 1)
                {
                    redCount++;
                }
            }

            foreach (Ball ball in ballSprites)
            {
                if (ball.Points == 1 && ball.IsBallInPocket)
                {
                    fallenRedCount++;
                }
            }

            foreach (Ball ball in pottedBalls)
            {
                if (ball.Points == 0)
                {
                    ball.ResetPositionAt(ball.InitPosition.X, ball.InitPosition.Y);
                    ball.IsBallInPocket = false;
                }
                else if (ball.Points > 1)
                {
                    if (fallenRedCount < redCount || teams[playingTeamID - 1].BallOn.Points != ball.Points)
                    {
                        if (currentGameState != GameState.TestShot)
                        {
                            if (fallenRedCount < redCount)
                                logList.Add(string.Format("{0} is back to table (there are still red balls on the table)", ball.Id));
                            else if (teams[playingTeamID - 1].BallOn.Points != ball.Points)
                                logList.Add(string.Format("{0} is back to table (expected: {1})", ball.Id, teams[playingTeamID - 1].BallOn.Id));
                        }

                        for (int points = ball.Points; points > 1; points--)
                        {
                            Ball candidateBall = GetCandidateBall(ball, points);
                            if (candidateBall != null)
                            {
                                ball.ResetPositionAt(candidateBall.InitPosition.X, candidateBall.InitPosition.Y);
                                ball.IsBallInPocket = false;
                                break;
                            }
                        }
                    }
                }
            }

            if (teams[playingTeamID - 1].BallOn == null)
                teams[playingTeamID - 1].BallOn = ballSprites[1];

            int strokenBallsCount = 0;
            foreach (Ball ball in strokenBalls)
            {
                //causing the cue ball to first hit a ball other than the ball on
                if (strokenBallsCount == 0)
                {
                    if (ball.Points != teams[playingTeamID - 1].BallOn.Points)
                    {
                        if (ball.Points == 1 || teams[playingTeamID - 1].BallOn.Points == 1 || fallenRedCount == redCount)
                        {
                            if (currentGameState != GameState.TestShot)
                                logList.Add(string.Format("foul: {0} was touched first, while {1} was expected", ball.Id, (fallenRedCount < redCount && teams[playingTeamID - 1].BallOn.Points > 1) ? "some color ball" : teams[playingTeamID - 1].BallOn.Id));

                            teams[playingTeamID - 1].FoulList.Add((teams[playingTeamID - 1].BallOn.Points < 4 ? 4 : teams[playingTeamID - 1].BallOn.Points));
                            break;
                        }
                    }
                }

                strokenBallsCount++;
            }

            //Foul: causing the cue ball to miss all object balls
            if (strokenBallsCount == 0)
                teams[playingTeamID - 1].FoulList.Add(4);

            foreach (Ball ball in pottedBalls)
            {
                //causing the cue ball to enter a pocket
                if (ball.Points == 0)
                {
                    teams[playingTeamID - 1].FoulList.Add(4);
                }
                else
                {
                    //causing a ball different than the target ball to enter a pocket
                    if (ball.Points != teams[playingTeamID - 1].BallOn.Points)
                    {
                        if (ball.Points == 1 || teams[playingTeamID - 1].BallOn.Points == 1 || fallenRedCount == redCount)
                        {
                            if (currentGameState != GameState.TestShot)
                                logList.Add(string.Format("{0} was potted, while {1} was expected", ball.Id, teams[playingTeamID - 1].BallOn.Id));

                            teams[playingTeamID - 1].FoulList.Add(teams[playingTeamID - 1].BallOn.Points < 4 ? 4 : teams[playingTeamID - 1].BallOn.Points);
                        }
                    }
                }
            }

            if (teams[playingTeamID - 1].FoulList.Count == 0)
            {
                foreach (Ball ball in pottedBalls)
                {
                    //legally potting reds or colors
                    wonPoints += ball.Points;

                    if (currentGameState != GameState.TestShot)
                        logList.Add(string.Format("Player {0} won {1} points", teams[playingTeamID - 1].CurrentPlayer.Name, wonPoints));
                }
            }
            else
            {
                teams[playingTeamID - 1].FoulList.Sort();
                lostPoints = teams[playingTeamID - 1].FoulList[teams[playingTeamID - 1].FoulList.Count - 1];

                if (currentGameState != GameState.TestShot)
                    logList.Add(string.Format("Player {0} lost {1} points", teams[playingTeamID - 1].CurrentPlayer.Name, lostPoints));
            }

            teams[playingTeamID - 1].Points += wonPoints;
            teams[awaitingTeamID - 1].Points += lostPoints;

            bool swappedPlayers = false;
            //check if it's other player's turn
            if ((wonPoints == 0 || lostPoints > 0) && currentGameState != GameState.TestShot)
            {
                swappedPlayers = true;
                outgoingShot.HasFinishedTurn = true;
                if (currentGameState != GameState.TestShot)
                    logList.Add(string.Format("Player {0} has finished turn", teams[playingTeamID - 1].CurrentPlayer.Name));

                playingTeamID = (playingTeamID == 1 ? 2 : 1);
                awaitingTeamID = (playingTeamID == 1 ? 2 : 1);

                cueSprite.Texture = (teams[playingTeamID - 1].Id == 1 ? cueTexture1 : cueTexture2);
            }

            if (!someInTable && currentGameState != GameState.TestShot)
            {
                UpdateGameState(GameState.GameOver);
                outgoingShot.GameOver = true;
                return;
            }

            int fallenBallsCount = fallenBalls.Count;
            for (int i = fallenBallsCount - 1; i >= 0; i--)
            {
                if (!fallenBalls[i].IsBallInPocket)
                {
                    fallenBalls.RemoveAt(i);
                }
            }

            teams[awaitingTeamID - 1].JustSwapped = true;
            teams[playingTeamID - 1].JustSwapped = swappedPlayers;

            if (swappedPlayers)
            {
                playerState = PlayerState.Aiming;
            }
            else
            {
                if (playerState == PlayerState.Aiming)
                {
                    if (fallenRedCount < redCount)
                    {
                        if (teams[playingTeamID - 1].BallOn.Points == 1)
                        {
                            playerState = PlayerState.Calling;
                        }
                    }
                }
            }

            if (currentGameState != GameState.TestShot)
            {
                teams[playingTeamID - 1].BallOn = GetNextBallOn(swappedPlayers, teams[playingTeamID - 1].BallOn);

                Texture2D ballOnTexture = null;
                switch (teams[playingTeamID - 1].BallOn.Points)
                {
                    case (int)BallValues.Red:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\RedBall");
                        break;
                    case (int)BallValues.Yellow:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\YellowBall");
                        break;
                    case (int)BallValues.Green:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\GreenBall");
                        break;
                    case (int)BallValues.Brown:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\BrownBall");
                        break;
                    case (int)BallValues.Blue:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\BlueBall");
                        break;
                    case (int)BallValues.Pink:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\PinkBall");
                        break;
                    case (int)BallValues.Black:
                        ballOnTexture = Content.Load<Texture2D>(@"Images\BlackBall");
                        break;
                }

                if (teams[playingTeamID - 1].Id == 1)
                    ballOn1.Texture = ballOnTexture;
                else
                    ballOn2.Texture = ballOnTexture;
            }

            targetVector = new Vector2(0, 0);

            strokenBalls.Clear();
            pottedBalls.Clear();

            if (currentGameState == GameState.Play)
            {
                teams[playingTeamID - 1].Attempts =
                teams[awaitingTeamID - 1].Attempts = 0;

                teams[playingTeamID - 1].AttemptsToWin =
                teams[awaitingTeamID - 1].AttemptsToWin = 0;

                teams[playingTeamID - 1].AttemptsNotToLose =
                teams[awaitingTeamID - 1].AttemptsNotToLose = 0;

                teams[playingTeamID - 1].AttemptsOfDespair =
                teams[awaitingTeamID - 1].AttemptsOfDespair = 0;

                teams[playingTeamID - 1].BestShot.LostPoints =
                teams[awaitingTeamID - 1].BestShot.LostPoints = 1000;

                teams[playingTeamID - 1].BestShot.WonPoints =
                teams[awaitingTeamID - 1].BestShot.WonPoints = 0;

                teams[playingTeamID - 1].BestShotSelected =
                teams[awaitingTeamID - 1].BestShotSelected = false;
            }
        }

        private Ball GetMinColouredball()
        {
            Ball minColouredball = null;
            int minPoints = 8;
            foreach (Ball ball in ballSprites)
            {
                if (ball.Points > 1 && ball.Points < minPoints && !ball.IsBallInPocket)
                {
                    minColouredball = ball;
                    minPoints = minColouredball.Points;
                }
            }
            return minColouredball;
        }

        private Ball GetCandidateBall(Ball ball, int points)
        {
            Ball candidateBall = null;
            Ball fallenBall = ball;
            while (candidateBall == null)
            {
                foreach (Ball b in ballSprites)
                {
                    if (b.Points == points) // && b.IsBallInPocket)
                    {
                        candidateBall = b;
                    }
                }
                if (candidateBall != null)
                {
                    foreach (Ball collisionBall in ballSprites)
                    {
                        if (!collisionBall.IsBallInPocket)
                        {
                            if (collisionBall.Id != candidateBall.Id)
                            {
                                float xd = (float)(candidateBall.InitPosition.X - collisionBall.X);
                                float yd = (float)(candidateBall.InitPosition.Y - collisionBall.Y);

                                float sumRadius = (float)(Ball.Radius * 0.5);
                                float sqrRadius = sumRadius * sumRadius;

                                float distSqr = (xd * xd) + (yd * yd);

                                if (Math.Round(distSqr) < Math.Round(sqrRadius))
                                {
                                    candidateBall = null;
                                    points--;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return candidateBall;
        }

        private void UpdatePlayerState(PlayerState playerState)
        {
            this.playerState = playerState;
            lastPlayerState = playerState;
            //switch (playerState)
            //{
            //    case PlayerState.None:
            //        break;
            //    case PlayerState.SelectingNumberOfPlayers:
            //        break;
            //    case PlayerState.Aiming:
            //        break;
            //    default:
            //        break;
            //}
        }

        private Ball GetNextBallOn(bool swappedPlayers, Ball lastBallOn)
        {
            Ball nextBallOn = null;
            if (swappedPlayers)
            {
                if (teams[playingTeamID - 1].BallOn == null)
                {
                    nextBallOn = GetRandomRedBall();
                }
                else if (teams[playingTeamID - 1].JustSwapped)
                {
                    nextBallOn = GetRandomRedBall();
                }
                else if (teams[playingTeamID - 1].BallOn.Points == 1)
                {
                    nextBallOn = GetRandomRedBall();
                }

                if (nextBallOn == null)
                {
                    nextBallOn = GetMinColouredball();
                }
            }
            else
            {
                if (lastBallOn.Points == 1)
                {
                    nextBallOn = GetMinColouredball();
                }
                else
                {
                    nextBallOn = GetRandomRedBall();
                    if (nextBallOn == null)
                    {
                        nextBallOn = GetMinColouredball();
                    }
                }

                if (nextBallOn == null)
                {
                    nextBallOn = GetRandomRedBall();
                }
            }
            return nextBallOn;
        }

        private Ball GetEquivalentBallOn()
        {
            Ball ret = null;
            if (teams[awaitingTeamID - 1] != null)
            {
                if (teams[playingTeamID - 1].BallOn != null)
                {
                    ret = teams[playingTeamID - 1].BallOn;

                    int redCount = 0;
                    int fallenRedCount = 0;
                    foreach (Ball ball in ballSprites)
                    {
                        if (ball.Points == 1)
                        {
                            redCount++;

                            if (ball.IsBallInPocket)
                            {
                                fallenRedCount++;
                            }
                        }
                    }

                    if (teams[playingTeamID - 1].BallOn != null)
                    {
                        string currentBallOnId = teams[playingTeamID - 1].BallOn.Id;
                        if (fallenRedCount < redCount && teams[playingTeamID - 1].BallOn.Points > 1)
                        {
                            foreach (Ball ball in ballSprites)
                            {
                                if (!ball.IsBallInPocket && (ball.Points > teams[playingTeamID - 1].BallOn.Points || (ball.Points == 2 && teams[playingTeamID - 1].BallOn.Points == 7)))
                                {
                                    ret = ball;
                                    break;
                                }
                            }
                        }
                        else if (teams[playingTeamID - 1].BallOn.Points == 1)
                        {
                            foreach (Ball ball in ballSprites)
                            {
                                if (!ball.IsBallInPocket && (ball.Points == 1 && string.Compare(ball.Id, teams[playingTeamID - 1].BallOn.Id) > 0 || (ball.Id == "red01" && teams[playingTeamID - 1].BallOn.Id == "red15")))
                                {
                                    ret = ball;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return ret;
        }

        private Ball GetRandomRedBall()
        {
            Ball redBallOn = null;

            List<int> validRedBalls = new List<int>();
            int i = 0;
            foreach (Ball ball in ballSprites)
            {
                if (ball.Points == 1 && !ball.IsBallInPocket)
                {
                    validRedBalls.Add(i);
                }
                i++;
            }

            int redCount = validRedBalls.Count;

            if (redCount > 0)
            {
                Random rnd = new Random(DateTime.Now.Second);
                int index = rnd.Next(redCount - 1);

                redBallOn = ballSprites[validRedBalls[index]];
            }
            return redBallOn;
        }

        private Vector2 SetTarget(int x, int y)
        {
            Vector2 vector = targetVector;
            int ballX = (int)(targetRectangle.X + targetRectangle.Width / 2.0);
            int ballY = (int)(targetRectangle.Y + targetRectangle.Height / 2.0);
            int ballRadius = targetRectangle.Width / 2;

            float xd = (float)(x - ballX);
            float yd = (float)(y - ballY);

            float sumRadius = (float)(targetRectangle.Width / 2);
            float sqrRadius = sumRadius * sumRadius;

            float distSqr = (xd * xd) + (yd * yd);

            if (Math.Round(distSqr) < Math.Round(sqrRadius))
            {
                vector = new Vector2(xd, yd);
            }

            return vector;
        }

        private void TrySetTargetOrStrength(int x, int y)
        {
            if (!SetStrength(x, y))
            {
                targetVector = SetTarget(x, y);
            }
        }

        private bool SetStrength(int x, int y)
        {
            bool ret = false;
            if (x >= thermometerRectangle.X && x <= thermometerRectangle.X + thermometerRectangle.Width && y >= thermometerRectangle.Y - 3 && y <= thermometerRectangle.Y + thermometerRectangle.Height + 3)
            {
                strengthSprite.Size = new Vector2(x - strengthRectangle.X, 4);
                int strength = (int)((100.0 * (x - strengthRectangle.X) / strengthRectangle.Width));

                teams[0].Strength = strength;
                teams[1].Strength = strength;
                teams[playingTeamID - 1].Strength = strength;
                ret = true;
            }
            else
                ret = false;

            return ret;
        }

        private PoolState poolState
        {
            get { return currentPoolState; }
            set
            {
                lastPoolState = currentPoolState;
                currentPoolState = value;
            }
        }

        #endregion

        #region IVisualKeyboardObserver Members

        public void HighLightKey(Vector2 position)
        {
            Vector2 newPosition = new Vector2(position.X + keyboardRectangle.X, position.Y + keyboardRectangle.Y);

            if (keySprite.Position != newPosition)
                soundBank.PlayCue(GameSound.Click01.ToString());

            keySprite.Position = newPosition;

        }

        public void KeyPressed(char? c)
        {
            soundBank.PlayCue(GameSound.Click02.ToString());
            switch (c)
            {
                case (char)Keys.Back:
                    if (signInPlayer.Name.Length > 0)
                        signInPlayer.Name = signInPlayer.Name.Substring(0, signInPlayer.Name.Length - 1);
                    break;
                case (char)Keys.Enter:
                    currentGameState = GameState.Setup;
                    if (signInPlayer.Name.Length == 0)
                        signInPlayer.Name = "Human";
                    Connect();
                    break;
                default:
                    signInPlayer.Name += c.ToString();
                    break;
            }
        }

        public void KeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Back:
                    if (signInPlayer.Name.Length > 0)
                        signInPlayer.Name = signInPlayer.Name.Substring(0, signInPlayer.Name.Length - 1);
                    break;
                case Keys.Enter:
                    currentGameState = GameState.Play;
                    Connect();
                    break;
                default:
                    signInPlayer.Name += (char)key;
                    break;
            }
        }

        #endregion

        void Connect()
        {
            ContractPerson currPerson = new ContractPerson();
            currPerson.Name = signInPlayer.Name;
            currPerson.ImageByteArray = signInPlayer.ImageByteArray;
        }

        /// <summary>
        /// This method checks to see if the current thread needs to be marshalled
        /// to the correct (UI owner) thread. If it does a new delegate is created
        /// which recalls this method on the correct thread
        /// </summary>
        /// <param name="sender"><see cref="Proxy_Singleton">SnookerServiceAgent</see></param>
        /// <param name="e">The event args</param>
        void SnookerServiceAgent_ProxyEvent(object sender, ProxyEventArgs e)
        {
            if (e.teamList.Length > 0)
            {
                ContractTeam[] receivedTeams = ((ContractTeam[])e.teamList);
                SetupMultiPlayerGame(receivedTeams);
            }
        }

        private void SnookerServiceAgent_ProxyCallBackEvent(object sender, ProxyCallBackEventArgs e)
        {
            switch (e.callBackType)
            {
                case CallBackType.UserEnter:
                    UserEnter(e.team, e.person);
                    break;
                case CallBackType.UserLeave:
                    UserLeave(e.team, e.person);
                    break;
                case CallBackType.WaitForUserShot:
                    WaitForUserShot(e.team, e.person);
                    break;
                case CallBackType.ReceivePlay:
                    ReceivePlay(e.team, e.person, e.shot);
                    break;
            }
        }

        /// <summary>
        /// A delegate to allow a cross UI thread call to be marshalled to correct
        /// UI thread
        /// </summary>
        private delegate void SnookerServiceAgent_ProxyEvent_Delegate(object sender, ProxyEventArgs e);


        void XNASnooker_Exiting(object sender, EventArgs e)
        {
            if (rbtMultiPlayer.Checked)
                SnookerServiceAgent.GetInstance().ExitSnookerGame();
        }

        private void UserEnter(ContractTeam team, ContractPerson person)
        {
            logList.Add(string.Format("{0} has entered the game.", contractPerson.Name));
            if (team.Id != contractTeam.Id)
            {
                team.Players = new ContractPerson[] { person };
                List<ContractTeam> teams = new List<ContractTeam>();
                teams.Add(contractTeam);
                teams.Add(team);

                SetupMultiPlayerGame(teams.ToArray());
            }
        }

        private static void UserLeave(ContractTeam team, ContractPerson p)
        {
            if (rbtMultiPlayer.Checked)
                SnookerServiceAgent.GetInstance().ExitSnookerGame();
        }

        private static void WaitForUserShot(ContractTeam team, ContractPerson person)
        {
            playingTeamID = team.Id;
            awaitingTeamID = (team.Id == 1) ? 2 : 1;
        }

        private void ReceivePlay(ContractTeam team, ContractPerson person, Shot shot)
        {
            if (contractPerson.Name != person.Name)
            {
                logList.Add(string.Format("Player {0} received {1} snapshots", contractPerson.Name, shot.SnapshotList.Count()));

                incomingShotList.Add(shot);
            }
        }

        private static void ReceiveScore(ContractTeam team, ContractPerson person, int wonPoints, int lostPoints)
        {
            if (contractPerson.Name != person.Name)
            {
                teams[playingTeamID - 1].Points += lostPoints;
                teams[awaitingTeamID - 1].Points += wonPoints;
            }
        }

        protected override void OnExiting(object sender, EventArgs args)
        {
            if (rbtMultiPlayer.Checked)
                SnookerServiceAgent.GetInstance().ExitSnookerGame();
        }

        private void GenerateComputerShot()
        {
            cueDistance = 0;

            List<Ball> auxBalls = new List<Ball>();

            auxBalls.Clear();
            foreach (Ball b in ballSprites)
            {
                Ball auxBall = new Ball(null, null, null, null, new Vector2(b.Position.X, b.Position.Y), new Vector2((int)Ball.Radius, (int)Ball.Radius), b.Id, null, 0);
                auxBall.IsBallInPocket = b.IsBallInPocket;
                auxBalls.Add(auxBall);
            }

            int lastPlayerScore = teams[playingTeamID - 1].Points;
            int lastOpponentScore = teams[awaitingTeamID - 1].Points;
            int player1Score = teams[0].Points;
            int player2Score = teams[1].Points;
            string ballOnId = teams[playingTeamID - 1].BallOn.Id;

            int newPlayerScore = -1;
            int newOpponentScore = 1000;

            teams[playingTeamID - 1].Attempts++;
            if (teams[playingTeamID - 1].AttemptsToWin < maxComputerAttempts)
            {
                teams[playingTeamID - 1].AttemptsToWin++;
            }
            else if (teams[playingTeamID - 1].AttemptsNotToLose < maxComputerAttempts)
            {
                teams[playingTeamID - 1].AttemptsNotToLose++;
            }
            else
            {
                teams[playingTeamID - 1].AttemptsOfDespair++;
            }

            teams[playingTeamID - 1].Points = lastPlayerScore;
            teams[awaitingTeamID - 1].Points = lastOpponentScore;
            foreach (Ball b in ballSprites)
            {
                if (b.Id == ballOnId)
                {
                    teams[playingTeamID - 1].BallOn = b;
                    break;
                }
            }
            teams[0].Points = player1Score;
            teams[1].Points = player2Score;

            bool despair = (teams[playingTeamID - 1].AttemptsOfDespair > 0);

            UpdateGameState(GameState.TestShot);
            TestShot shot = GenerateRandomTestComputerShot(despair);
            teams[playingTeamID - 1].LastShot = shot;

            if (shot == null) // Game Over
            {
                teams[playingTeamID - 1].BestShot = null;
                UpdateGameState(GameState.GameOver);
            }
            else
            {
                while (poolState == PoolState.MovingBalls)
                {
                    MoveBalls();
                }

                calculatingPositions = false;
                ProcessFallenBalls();

                newPlayerScore = teams[playingTeamID - 1].Points;
                newOpponentScore = teams[awaitingTeamID - 1].Points;

                shot.WonPoints = newPlayerScore - lastPlayerScore;
                shot.LostPoints = newOpponentScore - lastOpponentScore;
                cueSprite.NewTarget = new Vector2(shot.Position.X, shot.Position.Y);

                double dx = ballSprites[0].DrawPosition.X - shot.Position.X;
                double dy = ballSprites[0].DrawPosition.Y - shot.Position.Y;
                double h = Math.Sqrt(dx * dx + dy * dy);
                teams[playingTeamID - 1].FinalCueAngle = (float)Math.Acos(dx / h);


                //decide whether this shot was better then the current best shot
                if (shot.LostPoints < teams[playingTeamID - 1].BestShot.LostPoints || 
                    shot.WonPoints > teams[playingTeamID - 1].BestShot.WonPoints)
                {
                    teams[playingTeamID - 1].BestShot.LostPoints = shot.LostPoints;
                    teams[playingTeamID - 1].BestShot.WonPoints = shot.WonPoints;
                    teams[playingTeamID - 1].BestShot.Position = shot.Position;
                    teams[playingTeamID - 1].BestShot.Strength = shot.Strength;
                }

                int i = 0;
                foreach (Ball b in ballSprites)
                {
                    Ball auxB = auxBalls[i];
                    b.Position = new Vector2(auxB.Position.X, auxB.Position.Y);
                    b.IsBallInPocket = auxB.IsBallInPocket;
                    i++;
                }

                if (newPlayerScore > lastPlayerScore ||
                    newOpponentScore == lastOpponentScore && (teams[playingTeamID - 1].AttemptsToWin >= maxComputerAttempts) ||
                    teams[playingTeamID - 1].AttemptsOfDespair > maxComputerAttempts
                    )
                {
                    teams[playingTeamID - 1].BestShotSelected = true;
                    teams[playingTeamID - 1].LastShot = teams[playingTeamID - 1].BestShot;
                }
            }

            teams[playingTeamID - 1].Points = lastPlayerScore;
            teams[awaitingTeamID - 1].Points = lastOpponentScore;
            teams[0].Points = player1Score;
            teams[1].Points = player2Score;
            foreach (Ball b in ballSprites)
            {
                if (b.Id == ballOnId)
                {
                    teams[playingTeamID - 1].BallOn = b;
                    break;
                }
            }

            int j = 0;
            foreach (Ball b in ballSprites)
            {
                Ball auxB = auxBalls[j];
                b.Position = new Vector2(auxB.Position.X, auxB.Position.Y);
                b.IsBallInPocket = auxB.IsBallInPocket;
                j++;
            }

            if (teams[playingTeamID - 1].BestShotSelected && teams[playingTeamID - 1].BestShot != null)
            {
                hitPosition = new Vector2(teams[playingTeamID - 1].BestShot.Position.X, teams[playingTeamID - 1].BestShot.Position.Y);
                cueSprite.NewTarget = new Vector2(teams[playingTeamID - 1].BestShot.Position.X + poolRectangle.X - 7, teams[playingTeamID - 1].BestShot.Position.Y + poolRectangle.Y - 7);

                ghostBallSprite.Position = new Vector2(hitPosition.X + poolRectangle.X - 7, hitPosition.Y + poolRectangle.Y - 7);

                teams[playingTeamID - 1].Strength = teams[playingTeamID - 1].BestShot.Strength;
                this.playerState = PlayerState.Aiming;

                teams[playingTeamID - 1].LastShot = teams[playingTeamID - 1].BestShot;

                UpdateCuePosition(0, (int)teams[playingTeamID - 1].BestShot.Position.X + poolRectangle.X, (int)teams[playingTeamID - 1].BestShot.Position.Y + poolRectangle.Y);

                poolState = PoolState.PreparingCue;
            }
            teams[playingTeamID - 1].IsRotatingCue = true;
        }

        private TestShot GenerateRandomTestComputerShot(bool despair)
        {
            TestShot ret = null;

            //teams[playingTeamID - 1].BallOn = GetNextBallOn(teams[playingTeamID - 1].JustSwapped, teams[playingTeamID - 1].BallOn);
            Ball ghostBall = null;
            Random rnd = new Random(DateTime.Now.Second);

            List<Ball> ballOnList = new List<Ball>();

            if (teams[playingTeamID - 1].BallOn == null)
            {
                ballOnList = GetValidRedBalls();
            }
            else if (teams[playingTeamID - 1].BallOn.Points == 1)
            {
                ballOnList = GetValidRedBalls();
            }
            else
            {
                Ball redBall = GetRandomRedBall();
                if (redBall != null)
                {
                    foreach (Ball b in ballSprites)
                    {
                        if (b.Points > 1 && !b.IsBallInPocket)
                            ballOnList.Add(b);
                    }
                }
                else
                {
                    ballOnList.Add(teams[playingTeamID - 1].BallOn);
                }
            }

            ghostBall = GetRandomGhostBall(ballOnList, despair);

            if (ghostBall == null)
                ghostBall = GetNextBallOn(teams[playingTeamID - 1].JustSwapped, teams[playingTeamID - 1].BallOn);

            rnd = new Random(DateTime.Now.Second);
            int strength = rnd.Next(15);

            int strengthBase = 45;

            teams[playingTeamID - 1].Strength = strengthBase + strength;

            if (ghostBall != null)
            {
                ret = new TestShot(0, 0, new Vector2((int)ghostBall.X, (int)ghostBall.Y), teams[playingTeamID - 1].Strength);
                teams[playingTeamID - 1].TestPosition = new Vector2((int)ghostBall.X, (int)ghostBall.Y);
                teams[playingTeamID - 1].TestStrength = teams[playingTeamID - 1].Strength;
                HitBall((int)ghostBall.X, (int)ghostBall.Y);
            }
            return ret;
        }

        private void GenerateLastGoodComputerShot(TestShot shot)
        {
            int x = (int)shot.Position.X; //(int)currentPlayer.TestPosition.X;
            int y = (int)shot.Position.Y; //(int)currentPlayer.TestPosition.Y;
            teams[playingTeamID - 1].Strength = shot.Strength; //currentPlayer.TestStrength;

            Point p1 = new Point((int)(x + 1), (int)(y - Ball.Radius * 2.0) + 1);
            Point p2 = new Point((int)(x + 1), (int)(y + Ball.Radius * 2.0) + 1);
            Point p3 = new Point((int)(x - Ball.Radius * 2.0 + 1), (int)(y) - 1);
            Point p4 = new Point((int)(x + Ball.Radius * 2.0 + 1), (int)(y) - 1);

            p1 = new Point((int)(x), (int)(y - Ball.Radius * 2.0));
            p2 = new Point((int)(x), (int)(y + Ball.Radius * 2.0));
            p3 = new Point((int)(x - Ball.Radius * 2.0), (int)(y) - 1);
            p3 = new Point((int)(x + Ball.Radius * 2.0), (int)(y) - 1);

            int x1 = (int)(ballSprites[0].DrawPosition.X);
            int x2 = (int)(ballSprites[0].DrawPosition.Y);
            int y1 = (int)(x);
            int y2 = (int)(y);

            HitBall(x, y);
        }

        private List<Ball> GetValidRedBalls()
        {
            List<Ball> validRedBalls = new List<Ball>();

            foreach (Ball ball in ballSprites)
            {
                if (ball.Points == 1 && !ball.IsBallInPocket)
                {
                    validRedBalls.Add(ball);
                }
            }
            return validRedBalls;
        }

        private Ball GetRandomGhostBall(List<Ball> ballOnList, bool despair)
        {
            Ball randomGhostBall = null;

            List<Ball> ghostBalls = new List<Ball>();

            foreach (Ball ballOn in ballOnList)
            {
                List<Ball> tempGhostBalls = GetGhostBalls(ballOn, false);
                if (!despair)
                {
                    foreach (Ball ghostBall in tempGhostBalls)
                    {
                        ghostBalls.Add(ghostBall);
                    }
                }
                else
                {
                    //reflected by the top border
                    Ball mirroredBall = new Ball(null, null, null, null, new Vector2((int)(ballOn.X - Ball.Radius), (int)(-1.0 * ballOn.Y)), new Vector2((int)Ball.Radius, (int)Ball.Radius), "m1", null, 0);
                    tempGhostBalls = GetGhostBalls(mirroredBall, despair);
                    foreach (Ball ghostBall in tempGhostBalls)
                    {
                        ghostBalls.Add(ghostBall);
                    }
                    //reflected by the left border
                    mirroredBall = new Ball(null, null, null, null, new Vector2((int)(-1.0 * ballOn.X), (int)(ballOn.Y)), new Vector2((int)Ball.Radius, (int)Ball.Radius), "m2", null, 0);
                    tempGhostBalls = GetGhostBalls(mirroredBall, despair);
                    foreach (Ball ghostBall in tempGhostBalls)
                    {
                        ghostBalls.Add(ghostBall);
                    }
                    //reflected by the bottom border
                    mirroredBall = new Ball(null, null, null, null, new Vector2((int)(ballOn.X), (int)(ballOn.Y + (poolRectangle.Height * 2.0))), new Vector2((int)Ball.Radius, (int)Ball.Radius), "m3", null, 0);
                    tempGhostBalls = GetGhostBalls(mirroredBall, despair);
                    foreach (Ball ghostBall in tempGhostBalls)
                    {
                        ghostBalls.Add(ghostBall);
                    }
                    //reflected by the right border
                    mirroredBall = new Ball(null, null, null, null, new Vector2((int)(ballOn.X + (poolRectangle.Width * 2.0)), (int)(ballOn.Y)), new Vector2((int)Ball.Radius, (int)Ball.Radius), "m4", null, 0);
                    tempGhostBalls = GetGhostBalls(mirroredBall, despair);
                    foreach (Ball ghostBall in tempGhostBalls)
                    {
                        ghostBalls.Add(ghostBall);
                    }
                }
            }

            int ghostBallCount = ghostBalls.Count;
            if (ghostBallCount > 0)
            {
                Random rnd = new Random(DateTime.Now.Second);
                int index = rnd.Next(ghostBallCount);

                randomGhostBall = ghostBalls[index];
            }
            return randomGhostBall;
        }

        private List<Ball> GetGhostBalls(Ball ballOn, bool despair)
        {
            List<Ball> ghostBalls = new List<Ball>();

            int i = 0;
            foreach (Pocket pocket in pockets)
            {
                //distances between pocket and ball on center
                double dxPocketBallOn = pocket.HotSpotX - ballOn.X;
                double dyPocketBallOn = pocket.HotSpotY - ballOn.Y;
                double hPocketBallOn = Math.Sqrt(dxPocketBallOn * dxPocketBallOn + dyPocketBallOn * dyPocketBallOn);
                double a = dyPocketBallOn / dxPocketBallOn;

                //distances between ball on center and ghost ball center
                double hBallOnGhost = (Ball.Radius - 1.5) * 2.0;
                double dxBallOnGhost = hBallOnGhost * (dxPocketBallOn / hPocketBallOn);
                double dyBallOnGhost = hBallOnGhost * (dyPocketBallOn / hPocketBallOn);

                //ghost ball coordinates
                double gX = ballOn.X - dxBallOnGhost;
                double gY = ballOn.Y - dyBallOnGhost;
                double dxGhostCue = ballSprites[0].DrawPosition.X - gX;
                double dyGhostCue = ballSprites[0].DrawPosition.Y - gY;
                double hGhostCue = Math.Sqrt(dxGhostCue * dxGhostCue + dyGhostCue * dyGhostCue);

                //distances between ball on center and cue ball center
                double dxBallOnCueBall = ballOn.X - ballSprites[0].DrawPosition.X;
                double dyBallOnCueBall = ballOn.Y - ballSprites[0].DrawPosition.Y;
                double hBallOnCueBall = Math.Sqrt(dxBallOnCueBall * dxBallOnCueBall + dyBallOnCueBall * dyBallOnCueBall);

                //discards difficult ghost balls
                if (despair || (Math.Sign(dxPocketBallOn) == Math.Sign(dxBallOnCueBall) && Math.Sign(dyPocketBallOn) == Math.Sign(dyBallOnCueBall)))
                {
                    Ball ghostBall = new Ball(null, null, null, null, new Vector2((int)gX, (int)gY), new Vector2((int)Ball.Radius, (int)Ball.Radius), i.ToString(), null, 0);
                    ghostBalls.Add(ghostBall);
                    i++;
                }
            }

            return ghostBalls;
        }

        void ResetGame()
        {
            if (rbtMultiPlayer.Checked)
                SnookerServiceAgent.GetInstance().ExitSnookerGame();

            LoadBalls();
            foreach (Team team in teams)
            {
                team.FoulList = new List<int>();
                team.JustSwapped = false;
                team.BallOn = ballSprites[1];
                team.LastShot = null;
                team.BestShotSelected = false;
                team.Points = 0;
            }

            playingTeamID = 1;
            awaitingTeamID = 2;

            teams[playingTeamID - 1].BallOn = ballSprites[1];
            teams[awaitingTeamID - 1].BallOn = ballSprites[1];

            currentPoolState = PoolState.AwaitingShot;
            lastPoolState = PoolState.AwaitingShot;

            showOpponentsFadeDelay = maxShowOpponentsFadeDelay;
            showOpponentsDelay = maxShowOpponentsDelay;

            pottedBalls.Clear();
            fallenBalls.Clear();
            strokenBalls.Clear();
        }
    }
}
