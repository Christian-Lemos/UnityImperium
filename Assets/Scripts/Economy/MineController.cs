using UnityEngine;

[RequireComponent(typeof(ShipController))]
[RequireComponent(typeof(ResourceStorageController))]
public class MineController : MonoBehaviour
{
    public bool isMining;

    public int miningExtractionQuantity;
    public float miningInterval;

    private AsteroidController asteroidController;

    [SerializeField]
    private float miningTimer;

    [SerializeField]
    private bool miningTimerSet;

    private ResourceStorageController resourceStorageController;

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
        asteroidController = asteroid.GetComponent<AsteroidController>();
        isMining = true;
        miningTimerSet = true;
    }

    public void StopMining()
    {
        isMining = false;
    }

    private void ExtractResources()
    {
        int extractQuantity = (miningExtractionQuantity > asteroidController.ResourceQuantity) ? asteroidController.ResourceQuantity : miningExtractionQuantity;

        uint remainingSpace = resourceStorageController.resourceStorage.GetRemainingStorage();

        if (extractQuantity > remainingSpace)
        {
            extractQuantity = (int)remainingSpace;
        }

        resourceStorageController.resourceStorage.Add(asteroidController.resourceType, (uint)extractQuantity);

        asteroidController.ResourceQuantity -= extractQuantity;

        Debug.Log(resourceStorageController.resourceStorage.GetRemainingStorage());
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

                if (isMining && asteroidController != null)
                {
                    ExtractResources();
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
        resourceStorageController = GetComponent<ResourceStorageController>();
        miningTimer = MiningInterval;
    }
}