using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위한 필수 라이브러리
using System.Collections;

public class SlotManager : MonoBehaviour
{
    [Header("UI 연결 (Inspector에서 드래그)")]
    public TextMeshProUGUI actionText; // 왼쪽 슬롯: 공격, 방어, 개급 표시
    public TextMeshProUGUI targetText; // 오른쪽 슬롯: 너, 나 표시

    // 슬롯에 무작위로 나타날 단어 배열들
    private string[] actions = { "공격", "방어", "개그" };
    private string[] targets = { "너", "나" };

    // 각 슬롯이 회전 중인지 체크 (중복 클릭 방지)
    private bool isActionSpinning = false;
    private bool isTargetSpinning = false;

    void Start()
    {
        // 게임 시작 시 초기 안내 텍스트
        actionText.text = "클릭!";
        targetText.text = "클릭!";
    }

    /// <summary>
    /// 왼쪽(Action) 슬롯 버튼을 눌렀을 때 실행할 함수입니다.
    /// Inspector의 On Click() 목록에서 이 함수를 선택하세요.
    /// </summary>
    public void OnClickActionSlot()
    {
        // 이미 돌아가는 중이 아닐 때만 실행
        if (!isActionSpinning)
        {
            StartCoroutine(ActionSpinRoutine());
        }
    }

    /// <summary>
    /// 오른쪽(Target) 슬롯 버튼을 눌렀을 때 실행할 함수입니다.
    /// </summary>
    public void OnClickTargetSlot()
    {
        if (!isTargetSpinning)
        {
            StartCoroutine(TargetSpinRoutine());
        }
    }

    // --- 왼쪽(행동) 슬롯 회전 코루틴 ---
    IEnumerator ActionSpinRoutine()
    {
        isActionSpinning = true;
        float duration = 1.0f; // 1초 동안 회전
        float elapsed = 0f;

        while (elapsed < duration)
        {
            // 배열 안의 단어 중 하나를 랜덤하게 텍스트로 표시
            actionText.text = actions[Random.Range(0, actions.Length)];
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f); // 0.1초마다 단어 교체
        }

        isActionSpinning = false;
        CheckResults(); // 멈출 때마다 두 슬롯이 다 멈췄는지 체크
    }

    // --- 오른쪽(대상) 슬롯 회전 코루틴 ---
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

    /// <summary>
    /// 두 슬롯이 모두 멈췄을 때 최종 결과를 판단하는 함수입니다.
    /// </summary>
    void CheckResults()
    {
        // 두 슬롯이 모두 회전 중이 아닐 때만 결과 출력
        if (!isActionSpinning && !isTargetSpinning)
        {
            Debug.Log($"[슬롯 결과] {targetText.text}에게 {actionText.text}!");
            // 여기서 나중에 실제 대미지나 애니메이션 효과를 주면 됩니다.
        }
    }
}