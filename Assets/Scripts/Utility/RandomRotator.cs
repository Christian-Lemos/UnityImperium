using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RandomRotator : MonoBehaviour
{
    [SerializeField]
    private float tumble;

    void Start()
    {
        GetComponent<Rigidbody>().angularVelocity = Random.insideUnitSphere * tumble;
    }
}