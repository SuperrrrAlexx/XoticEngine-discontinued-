using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class Animation : GameObject
    {
        float depth;
        //Animation
        SpriteSheet sheet;
        bool repeat, playing;
        //Time per frame, current frame
        double tpf, frameTime;
        int frame;

        public Animation(string name, Vector2 position, float depth, SpriteSheet sheet, double fps, bool repeat)
            : base(name, position, 0)
        {
            this.depth = depth;
            this.sheet = sheet;
            this.tpf = 1.0 / fps;
            this.frameTime = tpf;
            this.repeat = repeat;
            this.playing = true;
        }

        public override void Update()
        {
            if (playing)
            {
                //Update the time
                frameTime -= Time.DeltaTime;
                //Move to the next frame
                if (frameTime <= 0)
                    NextFrame();
            }

            base.Update();
        }

        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw the current frame
            gameBatch.Draw(sheet[frame], Position, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);

            base.Draw(gameBatch, additiveBatch, guiBatch);
        }

        public void Stop()
        {
            //Stop the animation
            playing = false;
            frame = 0;
        }
        public void NextFrame()
        {
            //Move to the next frame
            frame++;
            if (frame >= Length)
            {
                frame = repeat ? frame - 1 : 0;
                if (!repeat)
                    playing = false;
            }

            //Reset the frame time
            frameTime = tpf;
        }

        public bool Playing
        { get { return playing; } set { playing = value; } }
        public int Length
        { get { return sheet.Length; } }
        public int Frame
        {
            get { return frame; }
            set
            {
                frame = value;
                frameTime = tpf;
            }
        }
        public double FPS
        { get { return 1.0 / tpf; } set { tpf = 1.0 / value; } }
    }
}
