using UnityEngine;
using UnityEngine.UI; // 버튼 컴포넌트 접근을 위해 필수

public class PlayerAttack : MonoBehaviour
{
    [Header("UI 연결")]
    public Button actionButton; // 에디터에서 Action 버튼을 여기에 드래그합니다.

    void Start()
    {
        // 1. 버튼 클릭 시 Attack 함수가 실행되도록 연결합니다.
        if (actionButton != null)
        {
            actionButton.onClick.AddListener(Attack);
        }
    }

    void Attack()
    {
        // 2. 여기에 공격 로직을 넣습니다.
        Debug.Log("캐릭터가 칼을 휘둘러 공격합니다!"); 
        
        // (참고) 애니메이터가 있다면 여기서 실행:
        // GetComponent<Animator>().SetTrigger("Attack");
    }
}
