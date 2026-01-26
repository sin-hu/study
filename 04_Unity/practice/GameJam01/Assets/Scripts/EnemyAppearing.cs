using UnityEngine;
using System.Collections;

public class EnemyAppearing : MonoBehaviour
{
    public GameObject slotMachine;
    public float appearSpeed = 5.0f;    // 튀어나오는 속도
    public float rotationSpeed = 5.0f;  // 회전 속도

    private bool isFighting = false;

    public void StartEnemyAppear()
    {
        if (!isFighting)
        {
            isFighting = true;
            StartCoroutine(AppearAndRotateRoutine());
        }
    }

    IEnumerator AppearAndRotateRoutine()
    {
        // 1. 물범 활성화
        gameObject.SetActive(true);

        // 목표 위치: 현재 위치에서 위로 약 3만큼 (빙하 위)
        Vector3 targetPos = transform.position + new Vector3(0, 3.5f, 0);
        // 목표 회전: 가로 형태 (Z축 0도 또는 필요한 각도)
        Quaternion targetRot = Quaternion.Euler(0, 0, 0); 

        // 이동과 회전이 거의 완료될 때까지 반복
        while (Vector3.Distance(transform.position, targetPos) > 0.01f)
        {
            // 위치 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPos, appearSpeed * Time.deltaTime);
            // 회전 (세로에서 가로로 슥~ 돌아감)
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
            
            yield return null;
        }

        // 최종 위치 및 회전값 강제 고정
        transform.position = targetPos;
        transform.rotation = targetRot;

        // 2. 슬롯머신 UI 등장
        if (slotMachine != null) slotMachine.SetActive(true);
    }
}