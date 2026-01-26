using UnityEngine;
using TMPro;
using UnityEngine.UI; // 버튼 제어를 위해 필수
using System.Collections;

public class SlotManager : MonoBehaviour
{
    [Header("UI 텍스트 연결")]
    public TextMeshProUGUI actionText;
    public TextMeshProUGUI targetText;

    [Header("슬롯 버튼 자체를 연결")]
    public Button actionSlotButton; // 액션 슬롯 버튼
    public Button targetSlotButton; // 타겟 슬롯 버튼

    private string[] actions = { "ATTACK", "DEFEND", "JOKE" };
    private string[] targets = { "YOU", "ME" };

    private bool isActionSpinning = false;
    private bool isTargetSpinning = false;
    private bool actionSelected = false;
    private bool targetSelected = false;

    public void ResetSlots()
    {
        actionText.text = "ATTACK";
        targetText.text = "TARGET";
        actionSelected = false;
        targetSelected = false;

        // 연출이 끝났으니 다시 버튼을 누를 수 있게 활성화
        if (actionSlotButton != null) actionSlotButton.interactable = true;
        if (targetSlotButton != null) targetSlotButton.interactable = true;
    }

    public void OnClickActionSlot()
    {
        // 돌아가는 중이 아닐 때만 실행
        if (!isActionSpinning) 
        {
            // 누르자마자 버튼 비활성화 (중복 클릭 원천 봉쇄)
            if (actionSlotButton != null) actionSlotButton.interactable = false;
            StartCoroutine(ActionSpinRoutine());
        }
    }

    public void OnClickTargetSlot()
    {
        if (!isTargetSpinning)
        {
            // 누르자마자 버튼 비활성화 (중복 클릭 원천 봉쇄)
            if (targetSlotButton != null) targetSlotButton.interactable = false;
            StartCoroutine(TargetSpinRoutine());
        }
    }

    IEnumerator ActionSpinRoutine()
    {
        isActionSpinning = true;
        actionSelected = true;
        float duration = 1.0f;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            actionText.text = actions[Random.Range(0, actions.Length)];
            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        isActionSpinning = false;
        CheckResults();
    }

    IEnumerator TargetSpinRoutine()
    {
        isTargetSpinning = true;
        targetSelected = true;
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

    void CheckResults()
    {
        if (!isActionSpinning && !isTargetSpinning && actionSelected && targetSelected)
        {
            BattleManager battleManager = Object.FindAnyObjectByType<BattleManager>();
            if (battleManager != null)
            {
                // 하단의 실행 버튼들도 잠금
                battleManager.SetButtonsInteractable(false);
                battleManager.ExecuteResult(actionText.text, targetText.text);
            }
        }
    }
}