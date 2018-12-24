using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Navi.Audio.Player;
using Navi.Screen;

namespace Navi.GUI.Widgets
{
    public sealed class TextButton : Label
    {
        private Texture2D shinyTexture;

        public TextButton(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, Color color, Color highlight, string fontPath)
            : base(content, graphics, soundPlayer, color, fontPath) 
        {
            shinyTexture = new Texture2D(graphics, 1, 1);
            shinyTexture.SetData(new Color[] { highlight });
            ActivateHighlights();
        }

        public TextButton(ContentManager content, GraphicsDevice graphics, SoundPlayer soundPlayer, string imagePath, string shinyImagePath, string fontPath)
            : base(content, graphics, soundPlayer, imagePath, fontPath)
        {
            shinyTexture = content.Load<Texture2D>(shinyImagePath);
            ActivateHighlights();
        }

        private void ActivateHighlights()
        {
            this.OnMouseOver += AddHighlight;
            this.OnMouseLeave += RemoveHighlight;
            this.OnClick += RemoveHighlight;  // to avoid a highlight in the last surface after a click
        }

        private void AddHighlight(SurfaceWidget widget, Vector2 mousePos)
        {
            this.IsMouseOver = true;
        }

        private void RemoveHighlight(SurfaceWidget widget, Vector2 mousePos)
        {
            this.IsMouseOver = false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsMouseOver)  // then draw highlight and then the text
            {
                spriteBatch.Draw(shinyTexture, pixelBoundingBox, null, Tint, radianRotation, pixelPivotPoint, Effect, 0.0f);
                DrawText(spriteBatch);  // allows to draw only the text above the highlighted texture
            }
            else
                base.Draw(spriteBatch);
        }
    }
}
