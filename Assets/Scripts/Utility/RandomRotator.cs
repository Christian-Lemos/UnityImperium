using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    private void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}