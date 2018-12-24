using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.Models
{
    /// <remarks>
    /// Is based on the Unit class of Navi (2014/2015).
    /// </remarks>
    public sealed class Dynamic : Model
    {
        public Dynamic() { }

        public Dynamic(Model model) : base(model) { }

        public Dynamic(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath)
            : base(content, graphics, soundPlayer, imagePath)
        {
            if (Plant.State == Plant.ProgramState.Editor) InitForGame();
        }

        public Texture2D Angry { get; set; }

        private void InitForGame()
        {
        }

        /*
        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsSearching)
                spriteBatch.Draw(Angry, pixelBoundingBox, null, Color.White, 0.0f, pointValueTexturePivotPoint, SpriteEffects.None, 0.0f);
            else if (!IsSelected)
                base.Draw(spriteBatch);
            else if (IsSelected)
                spriteBatch.Draw(Selected, pixelBoundingBox, null, Color.White, 0.0f, pointValueTexturePivotPoint, SpriteEffects.None, 0.0f);
        }
        */
    }
}
