using Imperium.Misc;
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
    private Timer miningTimer;
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

    public void StartMining(GameObject asteroid)
    {
        asteroidController = asteroid.GetComponent<AsteroidController>();
        isMining = true;
        miningTimer.timerSet = true;
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
    }

    private void Mine()
    {
        miningTimer.ResetTimer();

        if (isMining && asteroidController != null)
        {
            ExtractResources();
        }
        else
        {
            miningTimer.timerSet = false;
        }
    }

    private void Start()
    {
        resourceStorageController = GetComponent<ResourceStorageController>();
        miningTimer = new Timer(miningInterval, false, Mine);
    }

    private void Update()
    {
        miningTimer.Execute();
    }
}