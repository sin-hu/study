using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChickGenerator : MonoBehaviour
{
    public GameObject obj;
    public float interval = 3.0f;

    void Start()
    {
        InvokeRepeating("SpawnObj", 0.1f, interval);
    }

    void SpawnObj()
    {
        Instantiate(obj, transform.position, transform.rotation);
    }
}
