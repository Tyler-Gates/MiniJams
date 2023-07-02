using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;

namespace GamJamFusion
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        private Texture2D _background;

        private Texture2D _objectiveText;
        private Vector2 _objectiveTextPosition;


        private Dude _dudeObject;

        //private Texture2D _dude;
        //private Texture2D _dudeWalk;
        //private Vector2 _dudePosition;
        //private float _dudeAngle;
        //private bool _dudeAnimationSwitch;
        //private int _dudeAnimationWaitInterval;

        private Texture2D _meter;
        private float _meterScale;
        private Vector2 _meterPosition;

        private Texture2D _bloodHotZone;
        private Vector2 _bloodHotZonePosition;

        private Texture2D _heart;
        private Vector2 _heartPosition;

        private Random _rnd;
        private int _timeBetween;
        private int _movement;
        private int _signChanger;

        private Texture2D _loseScreen;
        private Texture2D _winScreen;

        private Boolean _killSwitch;
        private int _delayAfterWinLose;

        private Texture2D _meterBreak;
        private Vector2 _meterBreakPosition;

        private int _mAlphaValue;
        private int _fadeTimeBuffer;

        private Texture2D _armCharge;
        private Vector2 _armChargePosition;

        private Texture2D _armHit;
        private Vector2 _armHitPosition;
        private int _armSwingTimeInterval;
        private bool _armSwitch;

        private Texture2D _difficultyOptions;
        private Vector2 _difficultyOptionsPosition;

        private Texture2D _difficultySelect;
        private Vector2 _difficultySelectPosition;
        private float _difficulty;
        private int _difficultySelectWaitInterval;

        private bool _difficultySelectedStartGameSwitch;

        private Song _backgroundMusic;

        private SoundEffectInstance _shmack;
        private SoundEffectInstance _crack;
        private SoundEffectInstance _shing;
        private SoundEffectInstance _selectNoise;
        private SoundEffectInstance _enterNoise;
        private bool _shingSwitch;

        private bool _muteSwitch;
        private int _muteToggleWaitInterval;

        private Texture2D _nurseSmackFace;
        private Vector2 _nurseSmackFacePosition;

        private Texture2D _patientSmackFace;
        private Vector2 _patientSmackFacePosition;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferHeight = 512;
            _graphics.PreferredBackBufferWidth = 512;

            //_dudePosition = new Vector2(470,430);
            //_dudeAngle = 0;
            //_dudeAnimationWaitInterval = 200;
            //_dudeAnimationSwitch = false;

            _objectiveTextPosition = new Vector2(170, 160);

            _meterPosition = new Vector2(465, 342);
            _meterScale = 2f;

            _bloodHotZonePosition = new Vector2(428.5f, 320f);

            _heartPosition = new Vector2(428.5f, 336f);

            _rnd = new Random();
            _movement = 2;
            _timeBetween = 4;
            _signChanger = 1;

            _killSwitch = false;
            _delayAfterWinLose = 600;

            _meterBreakPosition = new Vector2(436, 210);

            _mAlphaValue = 0;
            _fadeTimeBuffer = 600;

            _armHitPosition = new Vector2(247, 403);
            _armChargePosition = new Vector2(247, 403);
            _armSwingTimeInterval = 182;
            _armSwitch = false;

            _difficultyOptionsPosition = new Vector2(170, 200);
            _difficultySelectPosition = new Vector2(90, 200);

            _difficulty = 1.2f;
            _difficultySelectWaitInterval = 330;

            _difficultySelectedStartGameSwitch = false;

            _shingSwitch = false;
            _muteSwitch = false;
            _muteToggleWaitInterval = 0;

            _nurseSmackFacePosition = new Vector2(197,372);
            _patientSmackFacePosition = new Vector2(235, 422);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            _shmack = Content.Load<SoundEffect>("smack").CreateInstance();
            _shmack.Volume = .05f;

            _crack = Content.Load<SoundEffect>("glassbreak").CreateInstance();
            _crack.Volume = .5f;

            _shing = Content.Load<SoundEffect>("shing").CreateInstance();
            _shing.Volume = .3f;

            _selectNoise = Content.Load<SoundEffect>("selectNoise").CreateInstance();
            _selectNoise.Volume = .3f;

            _enterNoise = Content.Load<SoundEffect>("enterNoise").CreateInstance();
            _enterNoise.Volume = .2f;

            _backgroundMusic = Content.Load<Song>("backgroundMusic");
            MediaPlayer.Volume = .1f;
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(_backgroundMusic);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            _dudeObject = new Dude(_spriteBatch);
            _dudeObject.LoadContent(Content);

            _background = Content.Load<Texture2D>("background");
            _objectiveText = Content.Load<Texture2D>("controlThePatient");
            _meter = Content.Load<Texture2D>("bloodMeter");
            _bloodHotZone = Content.Load<Texture2D>("bloodHotzone");
            _heart = Content.Load<Texture2D>("heart");
            _loseScreen = Content.Load<Texture2D>("loserscreen");
            _winScreen = Content.Load<Texture2D>("youWin");
            _meterBreak= Content.Load<Texture2D>("meterBreak");
            _armCharge = Content.Load<Texture2D>("armCharge");
            _armHit = Content.Load<Texture2D>("armHit");
            _difficultyOptions = Content.Load<Texture2D>("difficultySelect");
            _difficultySelect = Content.Load<Texture2D>("selectArrow");
            _nurseSmackFace = Content.Load<Texture2D>("smackFaceNurse");
            _patientSmackFace = Content.Load<Texture2D>("ouchy");
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            muteToggle(gameTime);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            if (!_killSwitch)
            {
                DifficultySelection(gameTime);
                if ((Keyboard.GetState().IsKeyDown(Keys.Space) || Keyboard.GetState().IsKeyDown(Keys.Enter)) && !_difficultySelectedStartGameSwitch)
                {
                    _enterNoise.Play();
                    _difficultySelectedStartGameSwitch = true;
                }
                if ( _difficultySelectedStartGameSwitch )
                {
                    _dudeObject.DudeMove();
                    if ( _dudeObject.dudeAngle >= 1.5f)
                    {
                        RandomHotSpotMovement(gameTime);
                        MeterFillLogic();
                        SmackEM(gameTime);
                    }
                }
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                ResetGame();
                _killSwitch = false;
            }
            else
            {
                if ( _fadeTimeBuffer <= 0 && _delayAfterWinLose <= 0 )
                {
                    _mAlphaValue += 6;
                }
                _fadeTimeBuffer -= gameTime.ElapsedGameTime.Milliseconds;
            }

            base.Update(gameTime);
        }

        private void DifficultySelection(GameTime gameTime)
        {
            if ((Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S)) && _difficulty < 1.4f && _difficultySelectWaitInterval <= 0 )
            {
                if (_selectNoise.State == SoundState.Playing)
                {
                    _selectNoise.Stop();
                }
                _selectNoise.Play();
                _difficulty += 0.2f;
                _difficultySelectPosition.Y += 48;
                _difficultySelectWaitInterval = 210;
            }
            else if ((Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W)) && _difficulty > 1f && _difficultySelectWaitInterval <= 0)
            {
                if (_selectNoise.State == SoundState.Playing)
                {
                    _selectNoise.Stop();
                }
                _selectNoise.Play();
                _difficulty -= 0.2f;
                _difficultySelectPosition.Y -= 48;
                _difficultySelectWaitInterval = 210;
            }
            _difficultySelectWaitInterval -= gameTime.ElapsedGameTime.Milliseconds;


        }

        protected override void Draw(GameTime gameTime)
        {
            Color color = new Color(255, 255, 255, MathHelper.Clamp(_mAlphaValue, 0, 255));

            _spriteBatch.Begin();

            _spriteBatch.Draw(_background, new Rectangle(0, 0, 512, 512), Color.White);

            if ( !_killSwitch )
            {
                if (!_difficultySelectedStartGameSwitch)
                {
                    _spriteBatch.Draw(_difficultySelect, _difficultySelectPosition, null, Color.White, 0f, new Vector2(_difficultySelect.Width / 2f, _difficultySelect.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                    _spriteBatch.Draw(_difficultyOptions, _difficultyOptionsPosition, null, Color.White, 0f, new Vector2(_difficultyOptions.Width / 2f, _difficultyOptions.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
                _dudeObject.DudeDraw(gameTime);

                if (_dudeObject.dudeAngle >= 1.5f)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Space))
                    {
                        _spriteBatch.Draw(_patientSmackFace, _patientSmackFacePosition, null, Color.White, _dudeObject.dudeAngle, new Vector2(_patientSmackFace.Width / 2f, _patientSmackFace.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                    }
                    SmackEMDrawing(gameTime);
                    MeterDraw();
                }
            }
            else
            {
                _delayAfterWinLose -= gameTime.ElapsedGameTime.Milliseconds;
                _dudeObject.DudeDraw(gameTime);
                if (_meterScale >= 15.5f)
                {
                    MeterDraw();
                    if (_delayAfterWinLose <= 0)
                    {
                        if( !_shingSwitch )
                        {
                            _shing.Play();
                            _shingSwitch = !_shingSwitch;
                        }
                        _spriteBatch.Draw(_winScreen, new Rectangle(0, 0, 512, 512), color);
                    }
                }
                else
                {
                    if (!_shingSwitch)
                    {
                        _crack.Play();
                        _shingSwitch = !_shingSwitch;
                    }
                    _spriteBatch.Draw(_meterBreak, _meterBreakPosition, null, Color.White, 0f, new Vector2(_meterBreak.Width / 2f, _meterBreak.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                    if (_delayAfterWinLose <= 0)
                    {
                        _spriteBatch.Draw(_loseScreen, new Rectangle(0, 0, 512, 512), color);
                    }
                }
            }
            _spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        private void muteToggle(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.M) && _muteToggleWaitInterval <= 0)
            {
                if (_muteSwitch)
                {
                    MediaPlayer.Pause();
                }
                else
                {
                    MediaPlayer.Resume();
                }
                _muteSwitch = !_muteSwitch;
                _muteToggleWaitInterval = 400;
            }
            _muteToggleWaitInterval -= gameTime.ElapsedGameTime.Milliseconds;
        }

        private void SmackEM(GameTime gameTime)
        {
            if ( Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                if(_heartPosition.Y >= 118f)
                {
                    _heartPosition.Y -= 250f * (gameTime.ElapsedGameTime.Milliseconds / 1000f) * _difficulty;
                }
            }
            else if (_heartPosition.Y < 337f)
            {
                _heartPosition.Y += 250f * (gameTime.ElapsedGameTime.Milliseconds / 1000f) * _difficulty;
            }
        }

        private void SmackEMDrawing(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                _spriteBatch.Draw(_nurseSmackFace, _nurseSmackFacePosition, null, Color.White, 0f, new Vector2(_nurseSmackFace.Width / 2f, _nurseSmackFace.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                if ( _armSwingTimeInterval <= 0 )
                {
                    _armSwitch = !_armSwitch;
                    _armSwingTimeInterval = 182;
                }
                _armSwingTimeInterval -= gameTime.ElapsedGameTime.Milliseconds;
                if ( _armSwitch )
                {
                    _shmack.Play();
                    _spriteBatch.Draw(_armHit, _armHitPosition, null, Color.White, 0f, new Vector2(_armHit.Width / 2f, _armHit.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
                else
                {
                    _spriteBatch.Draw(_armCharge, _armChargePosition, null, Color.White, 0f, new Vector2(_armCharge.Width / 2f, _armCharge.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
            }
            else
            {
                _armSwingTimeInterval = 72;
                _spriteBatch.Draw(_armCharge, _armChargePosition, null, Color.White, 0f, new Vector2(_armCharge.Width / 2f, _armCharge.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
            }
        }

        private void RandomHotSpotMovement(GameTime gameTime)
        {
            if ( _timeBetween <= 0 )
            {
                _timeBetween = _rnd.Next(200, 800);
                _movement = _rnd.Next(100, 200) * _signChanger;
                _signChanger = -_signChanger;
            }
            float movement = _movement * (gameTime.ElapsedGameTime.Milliseconds / 1000f) * _difficulty;
            _timeBetween -= gameTime.ElapsedGameTime.Milliseconds;
            if ( _bloodHotZonePosition.Y <= 337f - movement && _bloodHotZonePosition.Y >= 118f - movement)
            {
                _bloodHotZonePosition.Y += movement;
            }
        }

        private void MeterFillLogic()
        {
            if (_heartPosition.Y + 35 > _bloodHotZonePosition.Y && _heartPosition.Y - 35 < _bloodHotZonePosition.Y)
            {
                if (_meterScale < 15.5f)
                {
                    _meterScale += .05f;
                    _meterPosition.Y -= .025f * (_meter.Height);
                }
                else
                {
                    _killSwitch = true;
                }
            }
            else
            {
                if (_meterScale > 0f)
                {
                    _meterScale -= .05f;
                    _meterPosition.Y += .025f * (_meter.Height);
                }
                else
                {
                    _killSwitch = true;
                }
            }
        }
        private void MeterDraw()
        {
            _spriteBatch.Draw(_objectiveText, _objectiveTextPosition, null, Color.White, 0f, new Vector2(_objectiveText.Width / 2f, _objectiveText.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_meter, _meterPosition, null, Color.White, 0f, new Vector2(_meter.Width / 2f, _meter.Height / 2f), new Vector2(2f, _meterScale), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_bloodHotZone, _bloodHotZonePosition, null, Color.White, 0f, new Vector2(_bloodHotZone.Width / 2f, _bloodHotZone.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
            _spriteBatch.Draw(_heart, _heartPosition, null, Color.White, 0f, new Vector2(_heart.Width / 2f, _heart.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
        }

        private void ResetGame()
        {
            _dudeObject.Reset();

            _objectiveTextPosition = new Vector2(170, 160);

            _meterPosition = new Vector2(465, 342);
            _meterScale = 2f;

            _bloodHotZonePosition = new Vector2(428.5f, 320f);

            _heartPosition = new Vector2(428.5f, 336f);

            _rnd = new Random();
            _movement = 2;
            _timeBetween = 4;
            _signChanger = 1;

            _killSwitch = false;
            _delayAfterWinLose = 600;

            _meterBreakPosition = new Vector2(436, 210);

            _mAlphaValue = 0;
            _fadeTimeBuffer = 600;

            _armHitPosition = new Vector2(247, 400);
            _armChargePosition = new Vector2(247, 400);
            _armSwingTimeInterval = 182;
            _armSwitch = false;

            _difficultyOptionsPosition = new Vector2(170, 200);
            _difficultySelectPosition = new Vector2(90, 200);

            _difficulty = 1.2f;
            _difficultySelectWaitInterval = 140;

            _difficultySelectedStartGameSwitch = false;

            _shingSwitch = false;
        }
    }
}