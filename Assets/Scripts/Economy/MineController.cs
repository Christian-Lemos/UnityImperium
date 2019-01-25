using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class MineController : MonoBehaviour
{
    [SerializeField]
    private float miningInterval;

    public float MiningInterval
    {
        get
        {
            return miningInterval;
        }

        set
        {
            miningInterval = value;
        }
    }

    [SerializeField]
    private int miningExtractionQuantity;

    public int MiningExtractionQuantity
    {
        get
        {
            return miningExtractionQuantity;
        }

        set
        {
            if(value >= 0)
            {
                miningExtractionQuantity = value;
            }
            
        }
    }

    public bool isMining;

    [SerializeField]
    private bool miningTimerSet;
    [SerializeField]
    private float miningTimer;

    private void Start()
    {
        this.miningTimer = this.MiningInterval;
 
    }

    private void FixedUpdate()
    {
        MiningStateControl();
    }

    public void StartMining(GameObject asteroid)
    {
        this.isMining = true;
        this.miningTimerSet = true;
    }


    private void MiningStateControl()
    {
        if(miningTimerSet)
        {
            this.miningTimer -= Time.fixedDeltaTime; //Change to Time.deltaTime if used in void Update().

            if(this.miningTimer <= 0)
            {
                this.miningTimer = this.MiningInterval;

                if(this.isMining)
                {
                    ExtractResources(null);
                }
                else
                {
                    this.miningTimerSet = false;
                }
                
            }
        }
    }

    private void ExtractResources(AsteroidController asteroidController)
    {
       
    }








}
