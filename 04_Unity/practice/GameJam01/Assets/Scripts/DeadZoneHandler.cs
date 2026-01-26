using UnityEngine;
using UnityEngine.SceneManagement;

public class DeadZoneHandler : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            // 코루틴(StartCoroutine)을 거치지 않고 바로 호출
            SceneManager.LoadScene("Stage0");
        }
    }
}