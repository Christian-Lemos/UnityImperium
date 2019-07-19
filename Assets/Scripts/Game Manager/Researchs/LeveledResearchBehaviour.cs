using UnityEngine;
using System.Collections;
using Imperium;

public abstract class LeveledResearchBehaviour : ResearchBehaviour
{
    protected int level = 1;

    public abstract void UpdateLevel();
}
