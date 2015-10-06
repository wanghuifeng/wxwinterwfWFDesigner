using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.XNA
{
    public interface IVisualKeyboardObserver
    {
        void HighLightKey(Vector2 position);
        void KeyPressed(char? c);
        void KeyPressed(Keys key);
    }
}
