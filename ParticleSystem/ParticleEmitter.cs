using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XoticEngine.GameObjects;

namespace XoticEngine.ParticleSystem
{
    public class ParticleEmitter : GameObject
    {
        //Paused
        bool paused = false;
        //Position, rotation, speed
        Vector2 prevPosition;
        Vector2 speed;
        Vector2 scale;
        double rotationSpeed;
        //Depth
        bool oldestInFront;
        float depth;
        //Particles
        List<Particle> particles;
        double pps;
        double queue = 0;
        double ttl;
        //Modifiers
        List<ParticleModifier> modList, modOnceList;
        //Texture
        Texture2D texture;
        Color particleColor;

        public ParticleEmitter(string name, Vector2 position, float depth, bool oldestInFront, Vector2 speed, Vector2 scale, float rotation, double rotationSpeed, Texture2D texture, Color color, double particlesPerSecond, double secondsToLive, List<ParticleModifier> modifierList)
            : base(name, position, rotation)
        {
            this.prevPosition = position;
            //Depth
            this.depth = depth;
            this.oldestInFront = oldestInFront;
            //Particle properties
            this.speed = speed;
            this.rotationSpeed = rotationSpeed;
            this.scale = scale;
            this.texture = texture;
            this.ParticleColor = color;
            this.ttl = secondsToLive;
            //Particles
            this.pps = particlesPerSecond;
            this.particles = new List<Particle>();
            //Modifiers
            this.modList = new List<ParticleModifier>();
            this.modOnceList = new List<ParticleModifier>();
            for (int i = 0; i < modifierList.Count; i++)
            {
                if (modifierList[i].UpdateOnce)
                    modOnceList.Add(modifierList[i]);
                else
                    modList.Add(modifierList[i]);
            }
        }

        public override void Update()
        {
            //If the emitter is not paused, shoot particles
            if (!paused)
                Shoot();

            //Update all particles
            for (int i = 0; i < particles.Count(); i++)
            {
                //If the particle is alive, update it, else remove it
                if (particles[i].Alive)
                {
                    //Set the depth
                    particles[i].Depth = oldestInFront ? depth - (float)(particles[i].RealLifeTime / 100000f) : depth + (float)(particles[i].RealLifeTime / 100000f);

                    //Update the particle modifiers
                    for (int m = 0; m < modList.Count; m++)
                        modList[m].Update(particles[i]);

                    //Update the particle
                    particles[i].Update();
                }
                else
                    particles.RemoveAt(i);
            }

            //Update the previous position
            prevPosition = Position;
        }

        public override void Draw(SpriteBatch gameBatch, SpriteBatch guiBatch)
        {
            //Draw each particle
            for (int i = 0; i < particles.Count(); i++)
                particles[i].Draw(gameBatch);

            base.Draw(gameBatch, guiBatch);
        }

        public void Shoot()
        {
            //Update the queue and the actual amount of particles to be spawned this tick
            queue += pps * Time.DeltaTime;
            int spawns = (int)queue;

            //Create all the particles
            Shoot(spawns);

            //Update the queue
            queue -= spawns;
        }
        public void Shoot(int amount)
        {
            //Create all the particles
            for (int n = amount; n > 0; n--)
            {
                //Calculate the particle position between the old and new position
                Vector2 pos = new Vector2(MathHelper.Lerp(prevPosition.X, Position.X, (float)n / amount), MathHelper.Lerp(prevPosition.Y, Position.Y, (float)n / amount));
                //Create a new particle
                Particle p = new Particle(pos, depth, speed, scale, Rotation, rotationSpeed, texture, particleColor, ttl);

                //Update all updateOnce modifiers
                for (int i = 0; i < modOnceList.Count; i++)
                    modOnceList[i].Update(p);

                //Add the particle
                particles.Add(p);
            }
        }

        public void Move(Vector2 position)
        {
            RelativePosition = position;
            prevPosition = Position;
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
        public bool OldestInFront
        { get { return oldestInFront; } set { oldestInFront = value; } }
        //Particle properties
        public Vector2 Speed
        { get { return speed; } set { speed = value; } }
        public double RotationSpeed
        { get { return rotationSpeed; } set { rotationSpeed = value; } }
        public Color ParticleColor
        { get { return particleColor; } set { particleColor = value; } }
        public List<Particle> Particles
        { get { return particles; } }
    }
}
