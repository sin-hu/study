using UnityEngine;
using TMPro; // TextMeshPro 사용을 위한 필수 라이브러리
using System.Collections;

public class SlotManager : MonoBehaviour
{
    [Header("UI 연결 (Inspector에서 드래그)")]
    public TextMeshProUGUI actionText; // 왼쪽 슬롯: ATTACK, DEFEND 등 표시
    public TextMeshProUGUI targetText; // 오른쪽 슬롯: YOU, ME 표시

    // 슬롯에 나타날 단어 리스트
    private string[] actions = { "ATTACK", "DEFEND", "JOKE" };
    private string[] targets = { "YOU", "ME" };

    // 회전 중인지 확인 (중복 클릭 방지)
    private bool isActionSpinning = false;
    private bool isTargetSpinning = false;

    void Start()
    {
        // 게임 시작 시 초기 텍스트 설정 (원하는 기본값으로 수정 가능)
        // actionText.text = "ACTION";
        // targetText.text = "TARGET";
    }

    // --- 버튼 클릭 이벤트 함수 ---

    public void OnClickActionSlot()
    {
        if (!isActionSpinning)
        {
            StartCoroutine(ActionSpinRoutine());
        }
    }

    public void OnClickTargetSlot()
    {
        if (!isTargetSpinning)
        {
            StartCoroutine(TargetSpinRoutine());
        }
    }

    // --- 슬롯 회전 로직 (코루틴) ---

    IEnumerator ActionSpinRoutine()
    {
        isActionSpinning = true;
        float duration = 1.0f; // 1초 동안 회전
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 배열 내 단어를 랜덤하게 셔플링
            actionText.text = actions[Random.Range(0, actions.Length)];
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f); // 0.1초 대기
        }

        isActionSpinning = false;
        CheckResults(); // 두 슬롯이 모두 멈췄는지 확인
    }

    IEnumerator TargetSpinRoutine()
    {
        isTargetSpinning = true;
        float duration = 1.0f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            targetText.text = targets[Random.Range(0, targets.Length)];
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }

        isTargetSpinning = false;
        CheckResults();
    }

    // --- 최종 결과 판정 ---

    void CheckResults()
    {
        // 두 슬롯이 모두 멈춘 상태일 때만 결과를 출력
        if (!isActionSpinning && !isTargetSpinning)
        {
            Debug.Log($"[슬롯 결과] {actionText.text} to {targetText.text}!");
        }
    }
}