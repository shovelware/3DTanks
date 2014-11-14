using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Primitives
{
    static public class FPS
    {
        static public int frameRate = 0;
        static public int frameCounter = 0;
        static TimeSpan elapsedTime = TimeSpan.Zero;

        static public void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        static public void Draw()
        {
            frameCounter++;
        }
    }
}
