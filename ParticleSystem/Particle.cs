using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.Utilities;

namespace XoticEngine.ParticleSystem
{
    public class Particle
    {
        //Position, rotation, size
        Vector2 position;
        Vector2 speed;
        double rotation;
        double rotationSpeed;
        Vector2 scale;
        float depth;
        //Alive or dead
        bool alive = true;
        double ttl;
        //Lifetime
        double initialTTL;
        double lifeTime;
        double realLifeTime;
        //Texture
        Texture2D texture;
        Color particleColor;

        public Particle(Vector2 position, float depth, Vector2 speed, Vector2 scale, double rotation, double rotationSpeed, Texture2D texture, Color color, double ttl)
        {
            //Position, speed, rotation
            this.position = position;
            this.speed = speed;
            this.scale = scale;
            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;
            //Texture
            this.texture = texture;
            this.particleColor = color;
            this.depth = depth;
            //Time to live
            this.initialTTL = ttl;
            this.ttl = ttl;
            //Lifetime
            realLifeTime = 1 - (ttl / initialTTL);
            lifeTime = realLifeTime;
        }

        public void Update()
        {
            //Move and rotate the particle
            position += speed * (float)Time.DeltaTime;
            rotation += rotationSpeed * Time.DeltaTime;

            //Calculate the lifetime
            realLifeTime = 1f - (ttl / initialTTL);
            lifeTime = realLifeTime;

            //Update the time to live
            ttl -= Time.DeltaTime;
            //If the ttl <= 0, let the particle die
            if (ttl <= 0)
                alive = false;
        }

        public void Draw(SpriteBatch s)
        {
            //Draw the particle
            s.Draw(texture, position, null, particleColor, (float)rotation, new Vector2(0.5f), scale, SpriteEffects.None, depth);
        }

        public bool Alive
        { get { return alive; } }
        //Time to live
        public double TimeToLive
        { get { return ttl; } }
        public double InitalTimeToLive
        { get { return initialTTL; } }
        public double RealLifeTime
        { get { return realLifeTime; } }
        public double LifeTime
        { get { return lifeTime; } set { lifeTime = value; } }
        //Properties
        public Vector2 Position
        { get { return position; } set { position = value; } }
        public Vector2 Speed
        { get { return speed; } set { speed = value; } }
        public double Rotation
        { get { return rotation; } set { rotation = value; } }
        public double RotationSpeed 
        { get { return rotationSpeed; } set { rotationSpeed = value; } }
        public Vector2 Scale
        { get { return scale; } set { scale = value; } }
        public Color ParticleColor
        { get { return particleColor; } set { particleColor = value; } }
        public float Depth
        { get { return depth; } set { depth = value; } }
    }
}
