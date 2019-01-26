using UnityEngine;

[RequireComponent(typeof(ShipController))]
public class MineController : MonoBehaviour
{
    public bool isMining;

    [SerializeField]
    private int miningExtractionQuantity;

    [SerializeField]
    private float miningInterval;

    [SerializeField]
    private float miningTimer;

    [SerializeField]
    private bool miningTimerSet;

    public int MiningExtractionQuantity
    {
        get
        {
            return miningExtractionQuantity;
        }

        set
        {
            if (value >= 0)
            {
                miningExtractionQuantity = value;
            }
        }
    }

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

    public void StartMining(GameObject asteroid)
    {
        isMining = true;
        miningTimerSet = true;
    }

    private void ExtractResources(AsteroidController asteroidController)
    {
    }

    private void FixedUpdate()
    {
        MiningStateControl();
    }

    private void MiningStateControl()
    {
        if (miningTimerSet)
        {
            miningTimer -= Time.fixedDeltaTime; //Change to Time.deltaTime if used in void Update().

            if (miningTimer <= 0)
            {
                miningTimer = MiningInterval;

                if (isMining)
                {
                    ExtractResources(null);
                }
                else
                {
                    miningTimerSet = false;
                }
            }
        }
    }

    private void Start()
    {
        miningTimer = MiningInterval;
    }
}