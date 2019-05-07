using Imperium.Misc;
using Imperium.Persistence;
using Imperium.Persistence.ShipModules;
using UnityEngine;

[RequireComponent(typeof(ShipController))]
[RequireComponent(typeof(ResourceStorageController))]
public class MineController : MonoBehaviour, ISerializable<MineControllerPersistance>
{
    public bool isMining;

    public int miningExtractionQuantity;
    public float miningInterval;

    private AsteroidController asteroidController;

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

    public MineControllerPersistance Serialize()
    {
        return new MineControllerPersistance(isMining, miningExtractionQuantity, miningInterval, miningTimer);
    }

    public ISerializable<MineControllerPersistance> SetObject(MineControllerPersistance serializedObject)
    {
        this.isMining = serializedObject.isMining;
        this.miningInterval = serializedObject.miningInterval;
        this.miningExtractionQuantity = serializedObject.miningExtractionQuantity;
        this.miningTimer = serializedObject.miningTimer;
        this.miningTimer.action = Mine;
        return this;
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

        uint remainingSpace = resourceStorageController.ResourceStorage.GetRemainingStorage();

        if (extractQuantity > remainingSpace)
        {
            extractQuantity = (int)remainingSpace;
        }

        resourceStorageController.ResourceStorage.Add(asteroidController.resourceType, (uint)extractQuantity);

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
        if(miningTimer == null)
        {
            miningTimer = new Timer(miningInterval, false, Mine);
        }
        
    }

    private void Update()
    {
        miningTimer.Execute();
    }
}