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
        private bool paused = false;
        private Vector2 prevPosition;
        //Drawing
        private bool oldestInFront = true;
        //Particles
        private Particle particle;
        private List<Particle> particles;
        private double pps, queue;
        //Modifiers
        private List<IParticleModifier> modList, modOnceList;

        public ParticleEmitter(string name, Vector2 position, float depth, Particle particle, double particlesPerSecond, List<IParticleModifier> modifierList)
            : base(name, position, 0, Vector2.Zero, depth)
        {
            this.prevPosition = position;

            //Particles
            this.particle = particle;
            this.pps = particlesPerSecond;
            this.particles = new List<Particle>();

            //Modifiers
            this.modList = new List<IParticleModifier>();
            this.modOnceList = new List<IParticleModifier>();

            //Seperate all modifiers
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
            for (int i = particles.Count - 1; i >= 0; i--)
            {
                //If the particle is alive, update it, else remove it
                if (particles[i].Alive)
                {
                    //Set the depth
                    float depth = oldestInFront ? Depth - (float)(particles[i].RealLifeTime / 100000f) : Depth + (float)(particles[i].RealLifeTime / 100000f);
                    particles[i].Depth = MathHelper.Clamp(depth, 0, 1);

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
        public override void Draw(SpriteBatch gameBatch, SpriteBatch additiveBatch, SpriteBatch guiBatch)
        {
            //Draw each particle
            switch (DrawType)
            {
                case DrawMode.AlphaBlend:
                    foreach (Particle p in particles)
                        p.Draw(gameBatch);
                    break;
                case DrawMode.Additive:
                    foreach (Particle p in particles)
                        p.Draw(additiveBatch);
                    break;
                case DrawMode.Gui:
                    foreach (Particle p in particles)
                        p.Draw(guiBatch);
                    break;
            }

            base.Draw(gameBatch, additiveBatch, guiBatch);
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
                Particle p = particle.Fire();
                p.Position = pos;
                p.Depth = Depth;

                //Update all updateOnce modifiers
                for (int i = 0; i < modOnceList.Count; i++)
                    modOnceList[i].Update(p);

                //Add the particle
                particles.Add(p);
            }
        }

        public void Move(Vector2 position, bool relative)
        {
            //Set the new position, overwrite the previous position
            RelativePosition = relative ? RelativePosition + position : position;
            prevPosition = Position;
        }

        public Particle Particle
        { get { return particle; } set { particle = value; } }
        public bool Paused
        { get { return paused; } set { paused = value; } }
        public double ParticlesPerSecond
        { get { return pps; } set { pps = value; } }
        public List<IParticleModifier> ModifierList
        { get { return modList; } set { modList = value; } }
        public bool OldestInFront
        { get { return oldestInFront; } set { oldestInFront = value; } }
        public List<Particle> Particles
        { get { return particles; } }
    }
}
