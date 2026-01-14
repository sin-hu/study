using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public float speed = 8f; //속도
    public float moveableRange = 5.5f; //이동 범위 제한
    public float power = 1000f; //포탄 위력
    public GameObject cannonBall; //발사할 포탄 설정
    public Transform spawnPoint; //발사 위치 설정

    void Update() //update 함수: 게임 플레이중 반복 호출되는 함수
    {
        //플레이어 이동 명령
        transform.Translate(Input.GetAxis(
            "Horizontal") * speed * Time.deltaTime, 0, 0);
        //플레이어 이동 범위 제한
        transform.position = new Vector2(Mathf.Clamp(
            transform.position.x, -moveableRange, moveableRange), //x축 제한
            transform.position.y); //y축은 제한할 필요x

        //스페이스바 입력시 shoot함수 호출
        if (Input.GetKeyDown(KeyCode.Space))
        {
            shoot();
        }
    }

    //포탄 발사 함수
    void shoot()
    {
        GameObject newBullet =
            Instantiate(cannonBall, spawnPoint.position,
            Quaternion.identity) as GameObject;
        newBullet.GetComponent<Rigidbody2D>().AddForce(
            Vector3.up * power);
    }
} 