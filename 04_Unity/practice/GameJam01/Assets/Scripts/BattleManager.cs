using UnityEngine;
using UnityEngine.UI; // 버튼(Button) 컴포넌트를 제어하기 위해 필수입니다.

public class BattleManager : MonoBehaviour
{
    [Header("전투 설정")]
    public float detectRange = 5f;      // 적을 감지할 사거리
    public SlotManager slotManager;    // 하이어라키의 SlotMachine 오브젝트 연결

    [Header("UI 버튼 연결")]
    // SlotMachine 자식인 ActionSlot과 TargetSlot을 각각 드래그해서 연결하세요.
    public Button actionButton;        
    public Button targetButton;        

    private bool isEnemyInRange = false;

    void Update()
    {
        // 1. 씬 내의 'Enemy' 태그를 가진 모든 적을 찾습니다.
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool found = false;

        foreach (GameObject enemy in enemies)
        {
            // 2. Player와 적 사이의 거리를 계산합니다.
            float distance = Vector2.Distance(transform.position, enemy.transform.position);
            
            if (distance <= detectRange)
            {
                found = true;
                break; // 한 명이라도 사거리 안에 있으면 루프 종료
            }
        }

        // 3. 적이 사거리 안에 들어왔을 때 처리
        if (found && !isEnemyInRange)
        {
            isEnemyInRange = true;
            SetButtonsInteractable(true); // 버튼 클릭 가능하게 활성화
            Debug.Log("적이 사거리 내 진입! 이제 슬롯을 클릭할 수 있습니다.");
        }
        // 4. 적이 사거리 밖으로 나갔을 때 처리
        else if (!found && isEnemyInRange)
        {
            isEnemyInRange = false;
            SetButtonsInteractable(false); // 버튼 클릭 비활성화
            Debug.Log("적이 사거리 밖으로 나갔습니다. 슬롯이 잠깁니다.");
        }
    }

    /// <summary>
    /// 슬롯 버튼들의 활성화 상태를 한꺼번에 조절하는 함수입니다.
    /// </summary>
    void SetButtonsInteractable(bool state)
    {
        if (actionButton != null) actionButton.interactable = state;
        if (targetButton != null) targetButton.interactable = state;
    }

    // 사거리를 Scene 뷰에서 시각적으로 확인하기 위한 기즈모
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}