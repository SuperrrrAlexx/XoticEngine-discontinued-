using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace ScorpionEngine.ParticleSystem
{
    public abstract class ParticleModifier
    {
        public abstract void Update(Particle p);
        public abstract bool UpdateOnce { get; }
    }

    //Random spawn modifiers
    public class RandomSpawnSpeedModifier : ParticleModifier
    {
        float minSpeed, maxSpeed;

        public RandomSpawnSpeedModifier(float speed)
        {
            this.maxSpeed = speed;
            this.minSpeed = 0;
        }
        public RandomSpawnSpeedModifier(float minSpeed, float maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed - minSpeed;
        }

        public override void Update(Particle p)
        {
            Vector2 direction = new Vector2(SE.Random.NextFloat() * SE.Random.NextParity(), SE.Random.NextFloat() * SE.Random.NextParity());
            direction.Normalize();
            p.Speed = (SE.Random.NextFloat() * (maxSpeed + minSpeed) * direction);
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomSpawnDirectionModifier : ParticleModifier
    {
        Vector2 minSpeed, maxSpeed;

        public RandomSpawnDirectionModifier(Vector2 speed)
        {
            this.maxSpeed = speed;
            this.minSpeed = -speed;
        }
        public RandomSpawnDirectionModifier(Vector2 minSpeed, Vector2 maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }

        public override void Update(Particle p)
        {
            p.Speed = new Vector2(SE.Random.NextFloat() * (maxSpeed.X - minSpeed.X) + minSpeed.X, SE.Random.NextFloat() * (maxSpeed.Y - minSpeed.Y) + minSpeed.Y);
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomSpawnRotationModifier : ParticleModifier
    {
        double minRot, maxRot;

        public RandomSpawnRotationModifier()
        {
            this.minRot = 0.0;
            this.maxRot = 2 * Math.PI;
        }

        public RandomSpawnRotationModifier(double minRotation, double maxRotation)
        {
            this.minRot = minRotation;
            this.maxRot = maxRotation;
        }

        public override void Update(Particle p)
        {
            p.Rotation = SE.Random.NextDouble() * (maxRot - minRot) + minRot;
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomRotationSpeedModifier : ParticleModifier
    {
        double minRotSpeed, maxRotSpeed;

        public RandomRotationSpeedModifier()
        {
            this.minRotSpeed = -2 * Math.PI;
            this.maxRotSpeed = 2 * Math.PI;
        }

        public RandomRotationSpeedModifier(double minRotationSpeed, double maxRotationSpeed)
        {
            this.minRotSpeed = minRotationSpeed;
            this.maxRotSpeed = maxRotationSpeed;
        }

        public override void Update(Particle p)
        {
            p.RotationSpeed = SE.Random.NextDouble() * (maxRotSpeed - minRotSpeed) + minRotSpeed;
        }

        public override bool UpdateOnce { get { return true; } }
    }

    //Lerp modifiers
    public class ColorLerpModifier : ParticleModifier
    {
        Color color1, color2;

        public ColorLerpModifier(Color color1, Color color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }

        public override void Update(Particle p)
        {
            p.ParticleColor = Color.Lerp(color2, color1, (float)(p.TimeToLive / p.InitalTimeToLive));
        }

        public override bool UpdateOnce { get { return false; } }
    }

    public class SizeLerpModifier : ParticleModifier
    {
        Vector2 scale1, scale2;

        public SizeLerpModifier()
        {
            this.scale1 = Vector2.One;
            this.scale2 = Vector2.Zero;
        }

        public SizeLerpModifier(Vector2 scale1, Vector2 scale2)
        {
            this.scale1 = scale1;
            this.scale2 = scale2;
        }

        public override void Update(Particle p)
        {
            p.Scale = Vector2.Lerp(scale2, scale1, (float)(p.TimeToLive / p.InitalTimeToLive));
        }

        public override bool UpdateOnce { get { return false; } }
    }

    public class FadeOutModifier : ParticleModifier
    {
        double fadeTime;
        Color startColor = Color.Transparent;

        public FadeOutModifier(double fadeTime)
        {
            this.fadeTime = fadeTime;
        }

        public override void Update(Particle p)
        {
            if (p.TimeToLive <= fadeTime)
            {
                if (startColor == Color.Transparent)
                    startColor = p.ParticleColor;

                p.ParticleColor = Color.Lerp(Color.Transparent, startColor, (float)(p.TimeToLive / fadeTime));
            }
        }

        public override bool UpdateOnce { get { return false; } }
    }

    //Other modifiers
    public class AccelerationModifier : ParticleModifier
    {
        Vector2 acceleration;

        public AccelerationModifier(Vector2 acceleration)
        {
            this.acceleration = acceleration;
        }

        public override void Update(Particle p)
        {
            p.Speed += acceleration * (float)SE.Time.DeltaTime.TotalSeconds;
        }

        public override bool UpdateOnce { get { return false; } }
    }

    //Spawn shape modifiers
    public class FilledRectangleModifier : ParticleModifier
    {
        int width, height;

        public FilledRectangleModifier(int length)
        {
            this.width = length;
            this.height = length;
        }
        public FilledRectangleModifier(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Update(Particle p)
        {
            p.Position += new Vector2(SE.Random.NextFloat() * width, SE.Random.NextFloat() * height);
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class OutlineRectangleModifier : ParticleModifier
    {
        int width, height;

        public OutlineRectangleModifier(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Update(Particle p)
        {
            if (SE.Random.NextParity() == 1)
                p.Position += new Vector2(SE.Random.NextFloat() * width, SE.Random.Next(2) * height);
            else
                p.Position += new Vector2(SE.Random.Next(2) * width, SE.Random.NextFloat() * height);
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class FilledCircleModifier : ParticleModifier
    {
        int width, height;

        public FilledCircleModifier(int radius)
        {
            this.width = radius;
            this.height = radius;
        }
        public FilledCircleModifier(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Update(Particle p)
        {
            double angle = SE.Random.NextDouble() * Math.PI * 2;
            float random = SE.Random.NextFloat();
            p.Position += new Vector2((float)Math.Cos(angle) * width * random, (float)Math.Sin(angle) * height * random);
        }

        public override bool UpdateOnce
        { get { return true; } }
    }

    public class OutlineCircleModifier : ParticleModifier
    {
        int width, height;

        public OutlineCircleModifier(int radius)
        {
            this.width = radius;
            this.height = radius;
        }
        public OutlineCircleModifier(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public override void Update(Particle p)
        {
            double angle = SE.Random.NextDouble() * Math.PI * 2;
            p.Position += new Vector2((float)Math.Cos(angle) * width, (float)Math.Sin(angle) * height);
        }

        public override bool UpdateOnce
        { get { return true; } }
    }
}
