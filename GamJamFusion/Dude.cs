using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
namespace GamJamFusion
{
    public class Dude
    {
        private SpriteBatch _spriteBatch;
        private Texture2D _dude;
        private Texture2D _dudeWalk;
        private Vector2 _dudePosition;
        public float dudeAngle;
        private bool _dudeAnimationSwitch;
        private int _dudeAnimationWaitInterval;

        public Dude(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _dudePosition = new Vector2(470, 430);
            dudeAngle = 0;
            _dudeAnimationWaitInterval = 200;
        }

        public void LoadContent(ContentManager content)
        {
            _dude = content.Load<Texture2D>("dude");
            _dudeWalk = content.Load<Texture2D>("dudeWalk");
        }

        public void DudeMove()
        {
            if (_dudePosition.X > 200)
            {
                _dudePosition.X -= 2;
            }
            else if (dudeAngle < 1.5f)
            {
                _dudePosition.Y -= .25f;
                dudeAngle += .05f;
            }
        }

        public void DudeDraw(GameTime gameTime)
        {
            if (_dudePosition.X > 200 && _dudePosition.X != 470)
            {
                if (_dudeAnimationSwitch)
                {
                    _spriteBatch.Draw(_dudeWalk, _dudePosition, null, Color.White, dudeAngle, new Vector2(_dude.Width / 2f, _dude.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
                else
                {
                    _spriteBatch.Draw(_dude, _dudePosition, null, Color.White, dudeAngle, new Vector2(_dude.Width / 2f, _dude.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
                }
                if (_dudeAnimationWaitInterval <= 0)
                {
                    _dudeAnimationSwitch = !_dudeAnimationSwitch;
                    _dudeAnimationWaitInterval = 200;
                }
            }
            else
            {
                _spriteBatch.Draw(_dude, _dudePosition, null, Color.White, dudeAngle, new Vector2(_dude.Width / 2f, _dude.Height / 2f), new Vector2(2f, 2f), SpriteEffects.None, 0f);
            }
            _dudeAnimationWaitInterval -= gameTime.ElapsedGameTime.Milliseconds;

        }

        public void Reset()
        {
            _dudePosition = new Vector2(470, 430);
            dudeAngle = 0;
        }
    }
}