using UnityEngine;
using UnityEngine.UI; // 버튼(Button) 컴포넌트를 제어하기 위해 필수입니다.

public class BattleManager : MonoBehaviour
{
    [Header("전투 설정")]
    public float detectRange = 5f;      // 적을 감지할 사거리
    public SlotManager slotManager;    // 하이어라키의 SlotMachine 오브젝트 연결

    [Header("UI 버튼 연결")]
    public Button actionButton;        
    public Button targetButton;        

    private bool isEnemyInRange = false;

    void Update()
    {
        // 1. 우선 씬 안에 "Enemy" 태그를 가진 오브젝트가 있는지 찾습니다.
        GameObject[] enemies;
        
        try {
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
        } catch {
            // 유니티 에디터에 'Enemy' 태그 자체가 등록되지 않았을 경우를 대비한 안전장치
            SetButtonsInteractable(false);
            return;
        }

        bool found = false;

        // 2. 적이 한 명이라도 있을 때만 거리 계산을 수행합니다.
        if (enemies.Length > 0)
        {
            foreach (GameObject enemy in enemies)
            {
                if (enemy == null) continue; // 파괴된 적 오브젝트 예외 처리

                float distance = Vector2.Distance(transform.position, enemy.transform.position);
                
                if (distance <= detectRange)
                {
                    found = true;
                    break; // 사거리 내 적 발견 시 루프 종료
                }
            }
        }

        // 3. 상태 변화에 따른 버튼 활성화/비활성화 처리
        // found가 true이면 사거리 내 적이 있음, false이면 적이 없거나 멀리 있음
        if (found && !isEnemyInRange)
        {
            isEnemyInRange = true;
            SetButtonsInteractable(true);
            Debug.Log("적이 사거리 내 진입! 슬롯 활성화.");
        }
        else if (!found && isEnemyInRange)
        {
            isEnemyInRange = false;
            SetButtonsInteractable(false);
            Debug.Log("사거리 내에 적이 없습니다. 슬롯 비활성화.");
        }
    }

    /// <summary>
    /// 슬롯 버튼들의 활성화 상태를 조절합니다.
    /// </summary>
    void SetButtonsInteractable(bool state)
    {
        if (actionButton != null) actionButton.interactable = state;
        if (targetButton != null) targetButton.interactable = state;
    }

    // 사거리 시각화
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }
}