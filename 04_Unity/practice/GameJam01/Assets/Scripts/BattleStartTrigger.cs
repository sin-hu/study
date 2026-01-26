using UnityEngine;

public class BattleStartTrigger : MonoBehaviour
{
    // 여기에 아까 만든 물범(Enemy_LeopardSeal)을 연결할 거예요.
    public EnemyAppearing enemyScript; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 닿은 물체가 "Player" 태그를 가졌는지 확인
        if (collision.CompareTag("Player"))
        {
            // 물범의 등장 함수 실행!
            if (enemyScript != null)
            {
                enemyScript.StartEnemyAppear();
            }

            // 한 번 싸우기 시작하면 이 감지판은 필요 없으니 꺼줍니다.
            gameObject.SetActive(false);
        }
    }
}