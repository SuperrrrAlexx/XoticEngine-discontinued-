using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XoticEngine.ParticleSystem
{
    public abstract class ParticleModifier
    {
        public abstract void Update(Particle p);
        public abstract bool UpdateOnce { get; }
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
            this.maxAngle = 2 * Math.PI;
        }
        public RandomSpawnDirectionModifier(float minSpeed, float maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
            this.minAngle = 0;
            this.maxAngle = 2 * Math.PI;
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
            Vector2 angle = new Vector2((float)(Math.Cos(random * (maxAngle - minAngle) + minAngle)), (float)(Math.Sin(random * (maxAngle - minAngle) + minAngle)));
            p.Speed = (X.Random.NextFloat() * (maxSpeed + minSpeed) + minSpeed) * angle;
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
            p.Speed += acceleration * (float)X.Time.DeltaTime.TotalSeconds;
        }

        public override bool UpdateOnce { get { return false; } }
    }

    public class ScaleBySpeedModifier : ParticleModifier
    {
        Vector2 scaleFactor;

        public ScaleBySpeedModifier(Vector2 scaleFactor)
        {
            this.scaleFactor = scaleFactor / 1000;
        }

        public override void Update(Particle p)
        {
            p.Scale = p.Speed.Length() * scaleFactor;
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
            this.maxRot = 2 * Math.PI;
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
            float random = X.Random.NextFloat();
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
            double angle = X.Random.NextDouble() * Math.PI * 2;
            p.Position += new Vector2((float)Math.Cos(angle) * width, (float)Math.Sin(angle) * height);
        }

        public override bool UpdateOnce
        { get { return true; } }
    }
    #endregion
}
