using Imperium.Combat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Lib
{
    public interface ICombatable
    {
        CombatStats CombatStats { get; }
    }
}
