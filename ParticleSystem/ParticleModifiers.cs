using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.ParticleSystem
{
    public interface IParticleModifier
    {
        void Update(Particle p);

        bool UpdateOnce { get; }
    }

    #region Lifetime modifiers
    public class LifetimeModifier : IParticleModifier
    {
        private IParticleModifier modifier;
        private double startTime, endTime, length;

        public LifetimeModifier(IParticleModifier modifier, double startTime, double endTime)
        {
            this.modifier = modifier;
            this.startTime = startTime;
            this.endTime = endTime;
            this.length = endTime - startTime;
        }

        public void Update(Particle p)
        {
            //Only update if the particle lifetime is within the begin and end times
            if (p.RealLifeTime >= startTime && p.RealLifeTime <= endTime)
            {
                p.LifeTime = (p.RealLifeTime - startTime) / length;
                //Update the modifier
                modifier.Update(p);
                //Reset the lifetime
                p.LifeTime = p.RealLifeTime;
            }
        }

        public bool UpdateOnce { get { return false; } }
    }

    public class RandomDeathModifier : IParticleModifier
    {
        public void Update(Particle p)
        {
            if (X.Random.NextDouble() < p.LifeTime * Time.DeltaTime)
                p.Die();
        }

        public bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region Speed modifiers
    public class RandomSpawnDirectionModifier : IParticleModifier
    {
        private float minSpeed, maxSpeed, minAngle, maxAngle;

        public RandomSpawnDirectionModifier(float speed)
        {
            this.maxSpeed = speed;
            this.minSpeed = speed;
            this.minAngle = 0;
            this.maxAngle = MathHelper.TwoPi;
        }
        public RandomSpawnDirectionModifier(float minSpeed, float maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
            this.minAngle = 0;
            this.maxAngle = MathHelper.TwoPi;
        }
        public RandomSpawnDirectionModifier(float minSpeed, float maxSpeed, float minAngle, float maxAngle)
        {
            this.maxSpeed = minSpeed;
            this.minSpeed = maxSpeed;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
        }

        public void Update(Particle p)
        {
            float angle = X.Random.NextFloat() * (maxAngle - minAngle) + minAngle;
            p.Speed += (X.Random.NextFloat() * (maxSpeed - minSpeed) + minSpeed) * angle.GetDirection();
        }

        public bool UpdateOnce { get { return true; } }
    }

    public class RandomSpawnSpeedModifier : IParticleModifier
    {
        private Vector2 minSpeed, maxSpeed;

        public RandomSpawnSpeedModifier(Vector2 speed)
        {
            this.maxSpeed = speed;
            this.minSpeed = -speed;
        }
        public RandomSpawnSpeedModifier(Vector2 minSpeed, Vector2 maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
        }

        public void Update(Particle p)
        {
            p.Speed += new Vector2(X.Random.NextFloat() * (maxSpeed.X - minSpeed.X) + minSpeed.X, X.Random.NextFloat() * (maxSpeed.Y - minSpeed.Y) + minSpeed.Y);
        }

        public bool UpdateOnce { get { return true; } }
    }

    public class AccelerationModifier : IParticleModifier
    {
        private Vector2 acceleration;

        public AccelerationModifier(Vector2 acceleration)
        {
            this.acceleration = acceleration;
        }

        public void Update(Particle p)
        {
            p.Speed += acceleration * (float)Time.DeltaTime;
        }

        public bool UpdateOnce { get { return false; } }
    }

    public class ScaleBySpeedModifier : IParticleModifier
    {
        private Vector2 scaleFactor, startScale;
        private bool gotScale = false;

        public ScaleBySpeedModifier(Vector2 scaleFactor)
        {
            this.scaleFactor = scaleFactor / 1000f;
        }

        public void Update(Particle p)
        {
            if (!gotScale)
            {
                startScale = p.Scale;
                gotScale = true;
            }

            p.Scale = startScale + p.Speed.Length() * scaleFactor;
        }

        public bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region Rotation modifiers
    public class RandomSpawnRotationModifier : IParticleModifier
    {
        private float minRot, maxRot;

        public RandomSpawnRotationModifier()
        {
            this.minRot = 0;
            this.maxRot = MathHelper.TwoPi;
        }

        public RandomSpawnRotationModifier(float minRotation, float maxRotation)
        {
            this.minRot = minRotation;
            this.maxRot = maxRotation;
        }

        public void Update(Particle p)
        {
            p.Rotation = X.Random.NextFloat() * (maxRot - minRot) + minRot;
        }

        public bool UpdateOnce { get { return true; } }
    }

    public class RandomRotationSpeedModifier : IParticleModifier
    {
        private float minRotSpeed, maxRotSpeed;

        public RandomRotationSpeedModifier()
        {
            this.minRotSpeed = -MathHelper.TwoPi;
            this.maxRotSpeed = MathHelper.TwoPi;
        }

        public RandomRotationSpeedModifier(float minRotationSpeed, float maxRotationSpeed)
        {
            this.minRotSpeed = minRotationSpeed;
            this.maxRotSpeed = maxRotationSpeed;
        }

        public void Update(Particle p)
        {
            p.RotationSpeed = X.Random.NextFloat() * (maxRotSpeed - minRotSpeed) + minRotSpeed;
        }

        public bool UpdateOnce { get { return true; } }
    }

    public class RotateBySpeedModifier : IParticleModifier
    {
        private float rotationOffset;

        public RotateBySpeedModifier(float rotationOffset)
        {
            this.rotationOffset = rotationOffset;
        }

        public void Update(Particle p)
        {
            p.Rotation = p.Speed.GetAngle() + rotationOffset;
        }

        public bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region Lerp modifiers
    public class ColorLerpModifier : IParticleModifier
    {
        private Color color1, color2;

        public ColorLerpModifier(Color color1, Color color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }

        public void Update(Particle p)
        {
            p.Color = Color.Lerp(color1, color2, (float)p.LifeTime);
        }

        public bool UpdateOnce { get { return false; } }
    }

    public class SizeLerpModifier : IParticleModifier
    {
        private Vector2 scale1, scale2;

        public SizeLerpModifier(Vector2 scale1, Vector2 scale2)
        {
            this.scale1 = scale1;
            this.scale2 = scale2;
        }

        public void Update(Particle p)
        {
            p.Scale = Vector2.Lerp(scale1, scale2, (float)p.LifeTime);
        }

        public bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region Spawn shape modifiers
    public class FilledRectangleModifier : IParticleModifier
    {
        private int width, height;

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

        public void Update(Particle p)
        {
            p.Position += new Vector2(X.Random.NextFloat() * width, X.Random.NextFloat() * height);
        }

        public bool UpdateOnce { get { return true; } }
    }

    public class OutlineRectangleModifier : IParticleModifier
    {
        private int width, height;

        public OutlineRectangleModifier(int length)
        {
            this.width = length;
            this.height = length;
        }
        public OutlineRectangleModifier(int width, int height)
        {
            this.width = width;
            this.height = height;
        }

        public void Update(Particle p)
        {
            if (X.Random.NextSign() == 1)
                p.Position += new Vector2(X.Random.NextFloat() * width, X.Random.Next(2) * height);
            else
                p.Position += new Vector2(X.Random.Next(2) * width, X.Random.NextFloat() * height);
        }

        public bool UpdateOnce { get { return true; } }
    }

    public class FilledCircleModifier : IParticleModifier
    {
        private int width, height;

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

        public void Update(Particle p)
        {
            double angle = X.Random.NextDouble() * MathHelper.TwoPi;
            float distance = X.Random.NextFloat();
            p.Position += new Vector2((float)Math.Cos(angle) * width * distance, (float)Math.Sin(angle) * height * distance);
        }

        public bool UpdateOnce
        { get { return true; } }
    }

    public class OutlineCircleModifier : IParticleModifier
    {
        private int width, height;

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

        public void Update(Particle p)
        {
            double angle = X.Random.NextDouble() * MathHelper.TwoPi;
            p.Position += new Vector2((float)Math.Cos(angle) * width, (float)Math.Sin(angle) * height);
        }

        public bool UpdateOnce
        { get { return true; } }
    }
    #endregion
}
