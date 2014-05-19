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
        #region Fields
        Vector2 minSpeed;
        Vector2 maxSpeed;
        Random random;
        #endregion

        #region Constructors
        public RandomSpawnSpeedModifier(Vector2 speed)
        {
            this.minSpeed = -speed;
            this.maxSpeed = speed;
            random = new Random();
        }

        public RandomSpawnSpeedModifier(Vector2 minSpeed, Vector2 maxSpeed)
        {
            this.minSpeed = minSpeed;
            this.maxSpeed = maxSpeed;
            random = new Random();
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            Vector2 speed = new Vector2((float)random.NextDouble() * (maxSpeed.X - minSpeed.X) + minSpeed.X, (float)random.NextDouble() * (maxSpeed.Y - minSpeed.Y) + minSpeed.Y);
            p.Speed = speed;
        }
        #endregion

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomSpawnRotationModifier : ParticleModifier
    {
        #region Fields
        Random random;
        double minRot = 0.0;
        double maxRot = 2 * Math.PI;
        #endregion

        #region Constructors
        public RandomSpawnRotationModifier()
        {
            random = new Random();
        }

        public RandomSpawnRotationModifier(double minRotation, double maxRotation)
        {
            random = new Random();
            this.minRot = minRotation;
            this.maxRot = maxRotation;
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            p.Rotation = random.NextDouble() * (maxRot - minRot) + minRot;
        }
        #endregion

        public override bool UpdateOnce { get { return true; } }
    }

    public class RandomRotationSpeedModifier : ParticleModifier
    {
        #region Fields
        double minRotSpeed = -2 * Math.PI;
        double maxRotSpeed = 2 * Math.PI;
        Random random;
        #endregion

        #region Constructors
        public RandomRotationSpeedModifier()
        {
            random = new Random();
        }

        public RandomRotationSpeedModifier(double minRotationSpeed, double maxRotationSpeed)
        {
            random = new Random();
            this.minRotSpeed = minRotationSpeed;
            this.maxRotSpeed = maxRotationSpeed;
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            p.RotationSpeed = random.NextDouble() * (maxRotSpeed - minRotSpeed) + minRotSpeed;
        }
        #endregion

        public override bool UpdateOnce { get { return true; } }
    }

    //Lerp modifiers
    public class ColorLerpModifier : ParticleModifier
    {
        #region Fields
        Color color1;
        Color color2;
        #endregion

        #region Constructors
        public ColorLerpModifier(Color color1, Color color2)
        {
            this.color1 = color1;
            this.color2 = color2;
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            p.ParticleColor = Color.Lerp(color2, color1, p.TimeToLive / p.InitalTimeToLive);
        }
        #endregion

        public override bool UpdateOnce { get { return false; } }
    }

    public class SizeLerpModifier : ParticleModifier
    {
        #region Fields
        Vector2 scale1 = Vector2.One;
        Vector2 scale2 = Vector2.Zero;
        #endregion

        #region Constructors
        public SizeLerpModifier()
        {
        }

        public SizeLerpModifier(Vector2 scale1, Vector2 scale2)
        {
            this.scale1 = scale1;
            this.scale2 = scale2;
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            p.Scale = Vector2.Lerp(scale2, scale1, p.TimeToLive / p.InitalTimeToLive);
        }
        #endregion

        public override bool UpdateOnce { get { return false; } }
    }

    public class FadeOutModifier : ParticleModifier
    {
        #region Fields
        float fadeTime;
        Color startColor = Color.Transparent;
        #endregion

        #region Constructors
        public FadeOutModifier(float fadeTime)
        {
            this.fadeTime = fadeTime;
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            if (p.TimeToLive <= fadeTime)
            {
                if (startColor == Color.Transparent)
                    startColor = p.ParticleColor;

                p.ParticleColor = Color.Lerp(Color.Transparent, startColor, p.TimeToLive / fadeTime);
            }
        }
        #endregion
        public override bool UpdateOnce { get { return false; } }
    }

    //Other modifiers
    public class AccelerationModifier : ParticleModifier
    {
        #region Fields
        Vector2 acceleration;
        #endregion

        #region Constructors
        public AccelerationModifier(Vector2 acceleration)
        {
            this.acceleration = acceleration;
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            p.Speed += acceleration * (float)SE.DeltaTime.TotalSeconds;
        }
        #endregion

        public override bool UpdateOnce { get { return false; } }
    }

    //Spawn shape modifiers
    public class FilledRectangleModifier : ParticleModifier
    {
        #region Fields
        Random random;
        int width;
        int height;
        #endregion

        #region Constructors
        public FilledRectangleModifier(int width, int height)
        {
            this.width = width;
            this.height = height;
            random = new Random();
        }
        #endregion

        #region Methods
        public override void Update(Particle p)
        {
            p.Position = new Vector2((float)random.NextDouble() * width + p.Position.X, (float)random.NextDouble() * height + p.Position.Y);
        }
        #endregion

        public override bool UpdateOnce { get { return true; } }
    }
}
