using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XoticEngine.Achievements
{
    public interface IAchievement
    {
        void Update();
        void Draw(SpriteBatchHolder s);

        string Name { get; }
        bool Achieved { get; set; }
        bool Done { get; }
    }
}
