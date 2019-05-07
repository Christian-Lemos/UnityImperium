using UnityEngine;
using System.Collections;
using Assets.Lib;
using Imperium.Combat;
using System;
using Imperium.Misc;

[RequireComponent(typeof(ICombatable))]
public class ShieldRegenerator : MonoBehaviour
{

    private ICombatable m_combatable;
    private Timer m_timer;

    // Use this for initialization
    void Start()
    {
        m_combatable = GetComponent<ICombatable>();
        m_timer = new Timer(1f, true, () => { 
            m_timer.ResetTimer();
            m_combatable.CombatStats.Shields += m_combatable.CombatStats.ShieldRegen;
        });
    }

    private void OnDisable()
    {
        m_timer.timerSet = false;
        m_timer.ResetTimer();
    }

    private void OnEnable()
    {
        if(m_timer != null)
        {
            m_timer.timerSet = true;
            m_timer.ResetTimer();
        }
        
    }
}
