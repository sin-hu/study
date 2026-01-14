using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public floaat speed = 8f; //속도
    public float moveableRange = 5.5f; //이동 범위 제한

    void Update() //update 함수: 게임 플레이중 반복 호출되는 함수
    {
        //플레이어 이동 명령
        transform.Translate(Input.GetAxis(
            "Horizontal") * speed * Time.deltaTime, 0, 0);
        //플레이어 이동 범위 제한
        transform.position = new Vector2(Mathf.Clamp(
            transform.position.x, -moveableRange, moveableRange), //x축 제한
            transform.position.y); //y축은 제한할 필요x
    }
}
