using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XoticEngine.Utilities;

namespace XoticEngine.ParticleSystem
{
    public abstract class ParticleModifier
    {
        public abstract void Update(Particle p);
        public abstract bool UpdateOnce { get; }
    }

    public class LifetimeModifier : ParticleModifier
    {
        ParticleModifier modifier;
        double startTime, endTime, length;

        public LifetimeModifier(ParticleModifier modifier, double startTime, double endTime)
        {
            this.modifier = modifier;
            this.startTime = startTime;
            this.endTime = endTime;
            this.length = endTime - startTime;
        }

        public override void Update(Particle p)
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

        public override bool UpdateOnce { get { return false; } }
    }

    #region Speed
    public class RandomSpawnDirectionModifier : ParticleModifier
    {
        float minSpeed, maxSpeed;
        double minAngle, maxAngle;

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
        public RandomSpawnDirectionModifier(float minSpeed, float maxSpeed, double minAngle, double maxAngle)
        {
            this.maxSpeed = minSpeed;
            this.minSpeed = maxSpeed;
            this.minAngle = minAngle;
            this.maxAngle = maxAngle;
        }

        public override void Update(Particle p)
        {
            double random = X.Random.NextDouble();
            double angle = random * (maxAngle - minAngle) + minAngle;
            Vector2 angleVector = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
            p.Speed = (X.Random.NextFloat() * (maxSpeed + minSpeed) + minSpeed) * angleVector;
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomSpawnSpeedModifier : ParticleModifier
    {
        Vector2 minSpeed, maxSpeed;

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

        public override void Update(Particle p)
        {
            p.Speed = new Vector2(X.Random.NextFloat() * (maxSpeed.X - minSpeed.X) + minSpeed.X, X.Random.NextFloat() * (maxSpeed.Y - minSpeed.Y) + minSpeed.Y);
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class AccelerationModifier : ParticleModifier
    {
        Vector2 acceleration;

        public AccelerationModifier(Vector2 acceleration)
        {
            this.acceleration = acceleration;
        }

        public override void Update(Particle p)
        {
            p.Speed += acceleration * (float)Time.DeltaTime;
        }

        public override bool UpdateOnce { get { return false; } }
    }

    public class ScaleBySpeedModifier : ParticleModifier
    {
        Vector2 scaleFactor, startScale;
        bool gotScale = false;

        public ScaleBySpeedModifier(Vector2 scaleFactor)
        {
            this.scaleFactor = scaleFactor / 1000f;
        }

        public override void Update(Particle p)
        {
            if (!gotScale)
            {
                startScale = p.Scale;
                gotScale = true;
            }

            p.Scale = startScale + p.Speed.Length() * scaleFactor;
        }

        public override bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region Rotation
    public class RandomSpawnRotationModifier : ParticleModifier
    {
        double minRot, maxRot;

        public RandomSpawnRotationModifier()
        {
            this.minRot = 0.0;
            this.maxRot = MathHelper.TwoPi;
        }

        public RandomSpawnRotationModifier(double minRotation, double maxRotation)
        {
            this.minRot = minRotation;
            this.maxRot = maxRotation;
        }

        public override void Update(Particle p)
        {
            p.Rotation = X.Random.NextDouble() * (maxRot - minRot) + minRot;
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomRotationSpeedModifier : ParticleModifier
    {
        double minRotSpeed, maxRotSpeed;

        public RandomRotationSpeedModifier()
        {
            this.minRotSpeed = -MathHelper.TwoPi;
            this.maxRotSpeed = MathHelper.TwoPi;
        }

        public RandomRotationSpeedModifier(double minRotationSpeed, double maxRotationSpeed)
        {
            this.minRotSpeed = minRotationSpeed;
            this.maxRotSpeed = maxRotationSpeed;
        }

        public override void Update(Particle p)
        {
            p.RotationSpeed = X.Random.NextDouble() * (maxRotSpeed - minRotSpeed) + minRotSpeed;
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class RotateBySpeedModifier : ParticleModifier
    {
        double rotOffset;

        public RotateBySpeedModifier(double rotationOffset)
        {
            this.rotOffset = rotationOffset;
        }

        public override void Update(Particle p)
        {
            p.Rotation = p.Speed.GetAngle() + rotOffset;
        }

        public override bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region LerpModifiers
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
            p.ParticleColor = Color.Lerp(color1, color2, (float)p.LifeTime);
        }

        public override bool UpdateOnce { get { return false; } }
    }

    public class SizeLerpModifier : ParticleModifier
    {
        Vector2 scale1, scale2;

        public SizeLerpModifier(Vector2 scale1, Vector2 scale2)
        {
            this.scale1 = scale1;
            this.scale2 = scale2;
        }

        public override void Update(Particle p)
        {
            p.Scale = Vector2.Lerp(scale1, scale2, (float)p.LifeTime);
        }

        public override bool UpdateOnce { get { return false; } }
    }
    #endregion

    #region SpawnShapes
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
            p.Position += new Vector2(X.Random.NextFloat() * width, X.Random.NextFloat() * height);
        }

        public override bool UpdateOnce { get { return true; } }
    }

    public class OutlineRectangleModifier : ParticleModifier
    {
        int width, height;

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

        public override void Update(Particle p)
        {
            if (X.Random.NextParity() == 1)
                p.Position += new Vector2(X.Random.NextFloat() * width, X.Random.Next(2) * height);
            else
                p.Position += new Vector2(X.Random.Next(2) * width, X.Random.NextFloat() * height);
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
            double angle = X.Random.NextDouble() * Math.PI * 2;
            float radius = X.Random.NextFloat();
            p.Position += new Vector2((float)Math.Cos(angle) * width * radius, (float)Math.Sin(angle) * height * radius);
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
            double angle = X.Random.NextDouble() * Math.PI * 2;
            p.Position += new Vector2((float)Math.Cos(angle) * width, (float)Math.Sin(angle) * height);
        }

        public override bool UpdateOnce
        { get { return true; } }
    }
    #endregion
}
