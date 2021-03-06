﻿using System;
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
        private Vector2 position = Vector2.Zero;
        private Vector2 speed, scale;
        private float rotation, rotationSpeed, depth;
        //Lifetime
        private double ttl, initialTTL, lifeTime;
        //Texture
        private Texture2D texture;
        private Color color;
        private Vector2 origin;

        public Particle(Texture2D texture, Color color, double ttl)
        {
            //Position, speed, rotation
            this.speed = Vector2.Zero;
            this.scale = Vector2.One;
            this.rotation = 0;
            this.rotationSpeed = 0;
            //Texture
            this.texture = texture;
            this.color = color;
            this.origin = new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
            //Time to live
            this.initialTTL = ttl;
            this.ttl = ttl;
            this.lifeTime = RealLifeTime;
        }
        public Particle(Vector2 speed, Texture2D texture, Color color, double ttl)
        {
            //Position, speed, rotation
            this.speed = speed;
            this.scale = Vector2.One;
            this.rotation = 0;
            this.rotationSpeed = 0;
            //Texture
            this.texture = texture;
            this.color = color;
            this.origin = new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
            //Time to live
            this.initialTTL = ttl;
            this.ttl = ttl;
            this.lifeTime = RealLifeTime;
        }
        public Particle(Vector2 speed, Vector2 scale, float rotation, float rotationSpeed, Texture2D texture, Color color, double ttl)
        {
            //Position, speed, rotation
            this.speed = speed;
            this.scale = scale;
            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;
            //Texture
            this.texture = texture;
            this.color = color;
            this.origin = new Vector2((float)texture.Width / 2, (float)texture.Height / 2);
            //Time to live
            this.initialTTL = ttl;
            this.ttl = ttl;
            this.lifeTime = RealLifeTime;
        }

        public virtual void Update()
        {
            //Move and rotate the particle
            position += speed * TimeF.DeltaTime;
            rotation += rotationSpeed * TimeF.DeltaTime;

            //Set the lifetime
            lifeTime = RealLifeTime;

            //Update the time to live
            ttl -= Time.DeltaTime;
        }
        public virtual void Draw(SpriteBatch s)
        {
            //Draw the particle
            s.Draw(texture, position, null, color, rotation, origin, scale, SpriteEffects.None, depth);
        }

        public virtual Particle Fire()
        {
            return new Particle(speed, scale, rotation, rotationSpeed, texture, color, initialTTL)
            {
                origin = this.origin
            };
        }

        public void Die()
        {
            //Set the time to live to 0
            ttl = 0;
        }

        //Time to live, lifetime
        public bool Alive
        { get { return ttl > 0; } }
        public double TimeToLive
        { get { return ttl; } }
        public double InitialTimeToLive
        { get { return initialTTL; } }
        public double RealLifeTime
        { get { return 1f - (ttl / initialTTL); } }
        public double LifeTime
        { get { return lifeTime; } set { lifeTime = value; } }
        //Position, speed, rotation
        public Vector2 Position
        { get { return position; } set { position = value; } }
        public Vector2 Speed
        { get { return speed; } set { speed = value; } }
        public float Rotation
        { get { return rotation; } set { rotation = value; } }
        public float RotationSpeed
        { get { return rotationSpeed; } set { rotationSpeed = value; } }
        //Drawing
        public Texture2D Texture
        { get { return texture; } set { texture = value; } }
        public Vector2 Scale
        { get { return scale; } set { scale = value; } }
        public Vector2 Origin
        { get { return origin; } set { origin = value; } }
        public Color Color
        { get { return color; } set { color = value; } }
        public float Depth
        { get { return depth; } set { depth = value; } }
    }
}
