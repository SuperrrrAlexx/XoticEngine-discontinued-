using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScorpionEngine.GameObjects;

namespace ScorpionEngine.ParticleSystem
{
    public class ParticleEmitter : GameObject
    {
        //Paused
        bool paused = false;
        //Position, rotation, speed
        Vector2 prevPosition;
        Vector2 speed;
        Vector2 scale;
        double rotation;
        double rotationSpeed;
        float depth;
        //Particles
        List<Particle> particles;
        double pps;
        double queue = 0;
        double ttl;
        List<ParticleModifier> modList;
        //Texture
        Texture2D texture;
        Color particleColor;

        public ParticleEmitter(string name, Vector2 position, float depth, Vector2 speed, Vector2 scale, double rotation, double rotationSpeed, Texture2D texture, Color color, double particlesPerSecond, double secondsToLive, List<ParticleModifier> modifierList)
            : base(name, position)
        {
            //Position
            this.prevPosition = position;
            //Rotation
            this.rotation = rotation;
            this.rotationSpeed = rotationSpeed;
            //Particles
            this.depth = depth;
            this.speed = speed;
            this.scale = scale;
            this.texture = texture;
            this.ParticleColor = color;
            this.pps = particlesPerSecond;
            this.ttl = secondsToLive;
            this.modList = modifierList;
            this.particles = new List<Particle>();
        }

        public override void Update()
        {
            //If the emitter is not paused, shoot particles
            if (!paused)
                Shoot();

            //If there are particles
            if (particles.Count() > 0)
            {
                //Update all particles
                for (int i = particles.Count() - 1; i >= 0; i--)
                {
                    //If the particle is alive, update it, else remove it
                    if (particles[i].Alive)
                        particles[i].Update();
                    else
                        particles.RemoveAt(i);
                }
            }

            //Update the previous position
            prevPosition = Position;
        }

        public override void Draw(SpriteBatch s)
        {
            //If there are particles
            if (particles.Count() > 0)
            {
                //Draw each particle (last added particles first)
                for (int i = particles.Count() - 1; i >= 0; i--)
                    particles[i].Draw(s);
            }
        }

        public void Shoot()
        {
            //Update the queue and the actual amount of particles to be spawned this tick
            queue += pps * SE.Time.DeltaTime.TotalSeconds;
            int spawns = (int)queue;
            //Create all the particles
            for (int n = spawns; n > 0; n--)
            {
                //Calculate the particle position between the old and new position
                Vector2 pos = new Vector2(MathHelper.Lerp(prevPosition.X, Position.X, (float)n / spawns), MathHelper.Lerp(prevPosition.Y, Position.Y, (float)n / spawns));
                //Create a new particle and add it to the list
                Particle p = new Particle(pos, depth, speed, scale, rotation, rotationSpeed, texture, particleColor, ttl, modList);
                particles.Add(p);
            }
            queue -= spawns;
        }

        public void Move(Vector2 position)
        {
            RelativePosition = position;
            prevPosition = Position;
        }

        public void Shoot(int amount)
        {
            //Create all the particles
            for (int n = amount; n > 0; n--)
            {
                //Calculate the particle position between the old and new position
                Vector2 pos = new Vector2(MathHelper.Lerp(prevPosition.X, Position.X, (float)n / amount), MathHelper.Lerp(prevPosition.Y, Position.Y, (float)n / amount));
                //Create a new particle and add it to the list
                Particle p = new Particle(pos, depth, speed, scale, rotation, rotationSpeed, texture, particleColor, ttl, modList);
                particles.Add(p);
            }
        }

        //Particles shooting
        public bool Paused
        { get { return paused; } set { paused = value; } }
        public double ParticlesPerSecond
        { get { return pps; } set { pps = value; } }
        public double TimeToLive
        { get { return ttl; } set { ttl = value; } }
        public List<ParticleModifier> ModifierList
        { get { return modList; } set { modList = value; } }
        //Particle properties
        public Vector2 Speed
        { get { return speed; } set { speed = value; } }
        public double Rotation
        { get { return rotation; } set { rotation = value; } }
        public double RotationSpeed
        { get { return rotationSpeed; } set { rotationSpeed = value; } }
        public Color ParticleColor
        { get { return particleColor; } set { particleColor = value; } }
    }
}
