using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace XoticEngine.GameObjects
{
    public class Animation
    {
        //Animation
        private readonly SpriteSheet sheet;
        private bool repeat, playing;
        //Time per frame, current frame
        private double tpf, frameTime;
        private int frame;

        public Animation(SpriteSheet sheet, double fps, bool repeat)
        {
            this.sheet = sheet;
            this.tpf = 1.0 / fps;
            this.frameTime = tpf;
            this.repeat = repeat;
            this.playing = true;
        }

        public void Update()
        {
            if (playing)
            {
                //Update the time
                frameTime -= Time.DeltaTime;
                //Move to the next frame
                if (frameTime <= 0)
                    NextFrame();
            }
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
                frame = repeat ? 0 : frame - 1;
                if (!repeat)
                    playing = false;
            }

            //Reset the frame time
            frameTime = tpf;
        }

        //Sprites
        public Texture2D CurrentFrame
        { get { return sheet[frame]; } }
        public Texture2D this[int frame]
        { get { return sheet[frame]; } }
        public SpriteSheet Sheet
        { get { return sheet; } }
        //Playing
        public bool Playing
        { get { return playing; } set { playing = value; } }
        public bool Repeat
        { get { return repeat; } set { repeat = value; } }
        //Frames
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
