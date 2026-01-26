using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // [중요] 이 변수를 'public'으로 두는 것이 핵심입니다!
    // 이렇게 하면 유니티 인스펙터 창에서 각 씬마다 다른 이름을 적어줄 수 있어요.
    [Header("이동할 씬의 정확한 이름을 적어주세요")]
    public string nextSceneName;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 부딪힌 대상이 Player 태그를 가졌는지 확인
        if (collision.CompareTag("Player"))
        {
            // 설정한 이름의 씬으로 이동합니다.
            SceneManager.LoadScene(nextSceneName);
        }
    }
}