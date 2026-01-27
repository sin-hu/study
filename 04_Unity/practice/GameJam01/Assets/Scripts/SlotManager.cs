using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SlotManager : MonoBehaviour
{
    [Header("슬롯머신 본체 설정")]
    public Image slotMachineBody;
    public Sprite idleSprite;      // 슬롯머신_0
    public Sprite spinningSprite;  // 슬롯머신_2

    [Header("슬롯 아이콘 연결")]
    public Image actionSlotImage;
    public Image targetSlotImage;

    [Header("슬롯 아이콘 리스트 (Size: 5)")]
    // 순서: 0:공격, 1:뽀뽀, 2:친구맺기, 3:너, 4:나
    public List<Sprite> slotSprites; 

    [Header("슬롯 버튼 연결")]
    public Button actionSlotButton; 
    public Button targetSlotButton; 

    private bool isActionSpinning = false;
    private bool isTargetSpinning = false;
    private bool actionSelected = false;
    private bool targetSelected = false;

    private string finalAction;
    private string finalTarget;

    public void ResetSlots()
    {
        if (slotSprites.Count > 0) actionSlotImage.sprite = slotSprites[0];
        if (slotSprites.Count > 3) targetSlotImage.sprite = slotSprites[3]; // '너'
        
        slotMachineBody.sprite = idleSprite;
        actionSelected = false;
        targetSelected = false;

        if (actionSlotButton != null) actionSlotButton.interactable = true;
        if (targetSlotButton != null) targetSlotButton.interactable = true;
    }

    public void OnClickActionSlot()
    {
        if (!isActionSpinning) {
            if (actionSlotButton != null) actionSlotButton.interactable = false;
            StartCoroutine(ActionSpinRoutine());
        }
    }

    public void OnClickTargetSlot()
    {
        if (!isTargetSpinning) {
            if (targetSlotButton != null) targetSlotButton.interactable = false;
            StartCoroutine(TargetSpinRoutine());
        }
    }

    IEnumerator ActionSpinRoutine()
    {
        isActionSpinning = true;
        actionSelected = true;
        slotMachineBody.sprite = spinningSprite;

        float duration = 1.0f;
        float elapsed = 0f;
        while (elapsed < duration) {
            // 0(공격), 1(뽀뽀), 2(친구) 중에서 랜덤 선택
            int rand = Random.Range(0, 3); 
            actionSlotImage.sprite = slotSprites[rand];
            
            if (rand == 0) finalAction = "ATTACK";
            else if (rand == 1) finalAction = "KISS";
            else if (rand == 2) finalAction = "FRIEND";

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
        slotMachineBody.sprite = spinningSprite;

        float duration = 1.0f;
        float elapsed = 0f;
        while (elapsed < duration) {
            // 3(너), 4(나) 중에서 랜덤 선택
            int rand = Random.Range(3, 5); 
            targetSlotImage.sprite = slotSprites[rand];
            finalTarget = (rand == 3) ? "YOU" : "ME";

            elapsed += 0.1f;
            yield return new WaitForSeconds(0.1f);
        }
        isTargetSpinning = false;
        CheckResults();
    }

    void CheckResults()
    {
        if (!isActionSpinning && !isTargetSpinning) {
            slotMachineBody.sprite = idleSprite; 
            if (actionSelected && targetSelected) {
                BattleManager bm = Object.FindAnyObjectByType<BattleManager>();
                if (bm != null) bm.ExecuteResult(finalAction, finalTarget);
            }
        }
    }
}