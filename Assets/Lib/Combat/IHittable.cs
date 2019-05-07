using Imperium.MapObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imperium.Combat
{
    public interface IHittable
    {
        void TakeHit(Bullet bullet);
    }
}
