using Microsoft.Xna.Framework.Graphics; // For Texture2D
using Microsoft.Xna.Framework; // For Vector2using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Drawing;
using System.Text;

namespace Snooker.Client.Core.Model
{
    public class Player
    {
        #region attributes
        string name = "";
        byte[] imageByteArray;
        System.Drawing.Image image;
        Texture2D texture;
        #endregion attributes

        #region constructor
        public Player() { }

        public Player(string name)
        {
            this.name = name;
        }
        #endregion constructor

        #region properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public byte[] ImageByteArray
        {
            get { return imageByteArray; }
            set
            {
                imageByteArray = value;
            }
        }

        public System.Drawing.Image Image
        {
            get { return image; }
            set
            {
                image = value;
            }
        }
        public Texture2D Texture
        {
            get { return texture; }
            set
            {
                texture = value;
            }
        }
        #endregion properties
    }
}
