using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Snooker.Client.XNA
{
    public class VisualKeyboard
    {
        static Vector2 keySize = new Vector2(27, 27);
        static Vector2 keys1StartPosition = new Vector2(6, 5);
        static Vector2 keys2StartPosition = new Vector2(25, 41);
        static Vector2 keys3StartPosition = new Vector2(61, 77);
        static int keysHSpace = 36;

        static string KEYS1 = "QWERTYUIOP" + (char)8;
        static string KEYS2 = "ASDFGHJKL" + (char)13;
        static string KEYS3 = "ZXCVBNM";

        char? pendingKeyChar = '*';
        Keys pendingKey = Keys.None;
        IVisualKeyboardObserver observer;
        SoundBank soundBank;

        public VisualKeyboard(IVisualKeyboardObserver observer, SoundBank soundBank)
        {
            this.observer = observer;
            this.soundBank = soundBank;
        }

        public void MouseOver(Vector2 position)
        {
            char? ret = null;
            int i = 0;
            foreach (char c in KEYS1)
            {
                if (position.X >= keys1StartPosition.X + i * keysHSpace && position.X <= keys1StartPosition.X + i * keysHSpace + keySize.X &&
                    position.Y >= keys1StartPosition.Y && position.Y <= keys1StartPosition.Y + keySize.Y)
                {
                    ret = c;
                    observer.HighLightKey(new Vector2(keys1StartPosition.X + i * keysHSpace, keys1StartPosition.Y));
                    return;
                }
                i++;
            }
        }

        char? GetChar(Vector2 position)
        {
            char? ret = null;
            int i = 0;
            foreach (char c in KEYS1)
            {
                if (position.X >= keys1StartPosition.X && position.X <= keys1StartPosition.X + i * keysHSpace + keySize.X &&
                    position.Y >= keys1StartPosition.Y && position.Y <= keys1StartPosition.Y + keySize.Y)
                {
                    ret = c;
                    observer.HighLightKey(new Vector2(keys1StartPosition.X + i * keysHSpace, keys1StartPosition.Y));
                    break;
                }
                i++;
            }
            if (ret == null)
            {
                i = 0;
                foreach (char c in KEYS2)
                {
                    if (position.X >= keys2StartPosition.X && position.X <= keys2StartPosition.X + i * keysHSpace + keySize.X &&
                        position.Y >= keys2StartPosition.Y && position.Y <= keys2StartPosition.Y + keySize.Y)
                    {
                        ret = c;
                        observer.HighLightKey(new Vector2(keys2StartPosition.X + i * keysHSpace, keys2StartPosition.Y));
                        break;
                    }
                    i++;
                }
            }
            if (ret == null)
            {
                i = 0;
                foreach (char c in KEYS3)
                {
                    if (position.X >= keys3StartPosition.X && position.X <= keys3StartPosition.X + i * keysHSpace + keySize.X &&
                        position.Y >= keys3StartPosition.Y && position.Y <= keys3StartPosition.Y + keySize.Y)
                    {
                        ret = c;
                        observer.HighLightKey(new Vector2(keys3StartPosition.X + i * keysHSpace, keys3StartPosition.Y));
                        break;
                    }
                    i++;
                }
            }
            return ret;
        }

        public void KeyPressed(Vector2 position)
        {
            pendingKeyChar = GetChar(position);
        }

        public void KeyReleased(Vector2 position)
        {
            char? c = GetChar(position);
            if (c != null && pendingKeyChar != null)
            {
                if (c == pendingKeyChar)
                {
                    pendingKeyChar = '*';
                    observer.KeyPressed(c);
                }
            }
        }

        public void CheckKeys()
        {
            KeyboardState keyState = Keyboard.GetState();
            Keys[] keys = keyState.GetPressedKeys();
            foreach (Keys key in keys)
            {
                if ((key >= Keys.A && key <= Keys.Z) || key == Keys.Enter || key == Keys.Back)
                {
                    pendingKey = key;
                    break;
                }
            }
            if (pendingKey != Keys.None && keyState.IsKeyUp(pendingKey))
            {
                observer.KeyPressed(pendingKey);
                pendingKey = Keys.None;
            }
        }
    }
}
