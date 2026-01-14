using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyObj : MonoBehaviour
{
    public float deleteTime = 2.0f; //제거 시간
    void Start()
    {
        Destroy(gameObject, deleteTime); //오브젝트 제거
    }

    void Update()
    {
        
    }
}
