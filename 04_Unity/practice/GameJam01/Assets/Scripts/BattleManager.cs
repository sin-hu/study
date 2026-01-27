using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement; 

public class BattleManager : MonoBehaviour
{
    [Header("플레이어 오브젝트 설정")]
    public GameObject normalPlayer;

    [Header("KISS 이미지")]
    public Sprite kissSprite;
    private Sprite originalSprite;

    [Header("하트 UI 설정")]
    public GameObject heartPrefab;
    public Transform playerHeartParent;
    public int playerMaxHP = 5;
    private List<Image> playerHeartImages = new List<Image>();
    private int playerHP;
    [HideInInspector] public bool isPlayerDefending = false;

    [Header("전투 설정")]
    public float detectRange = 5f;
    public float jumpForce = 5f;
    public Button actionButton;
    public Button targetButton;

    private bool isEnemyInRange = false;
    private Rigidbody2D rb;
    private Coroutine activeRoutine;

    Color defendBlue = new Color(0f, 0.4f, 1f, 1f);
    Color friendYellow = new Color(1f, 0.9f, 0f, 1f);

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (normalPlayer != null)
            originalSprite = normalPlayer.GetComponent<SpriteRenderer>().sprite;

        ResetVisual();
        SpawnPlayerHearts();
        SetButtonsInteractable(false);
    }

    void SpawnPlayerHearts()
    {
        playerHP = playerMaxHP;
        foreach (Transform child in playerHeartParent) Destroy(child.gameObject);
        playerHeartImages.Clear();

        for (int i = 0; i < playerMaxHP; i++)
        {
            GameObject h = Instantiate(heartPrefab, playerHeartParent);
            playerHeartImages.Add(h.GetComponent<Image>());
        }
    }

    void Update()
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");
        bool found = enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= detectRange;

        if (found)
        {
            EnemyHP ehp = enemy.GetComponent<EnemyHP>();
            if (ehp != null) ehp.ShowUI();
        }

        if (found && !isEnemyInRange)
        {
            isEnemyInRange = true;
            SetButtonsInteractable(true);
        }
        else if (!found && isEnemyInRange)
        {
            isEnemyInRange = false;
            SetButtonsInteractable(false);
        }
    }

    public void ExecuteResult(string action, string targetName)
    {
        if (activeRoutine != null) StopCoroutine(activeRoutine);

        GameObject enemy = GameObject.FindWithTag("Enemy");
        activeRoutine = StartCoroutine(HandleAction(action, targetName, enemy));
    }

    IEnumerator HandleAction(string action, string targetName, GameObject enemy)
    {
        ResetVisual();

        switch (action)
        {
            case "ATTACK":
                if (targetName == "YOU" && enemy != null)
                    yield return StartCoroutine(AttackJumpRoutine(enemy));
                else
                {
                    PlayerTakeDamage(1);
                    yield return StartCoroutine(FlashColor(normalPlayer, Color.red));
                }
                break;

            case "DEFEND":
                if (targetName == "YOU" && enemy != null)
                {
                    enemy.GetComponent<SpriteRenderer>().color = defendBlue;
                    enemy.GetComponent<EnemyHP>().SetDefense(true);
                }
                else
                {
                    isPlayerDefending = true;
                    normalPlayer.GetComponent<SpriteRenderer>().color = defendBlue;
                }

                yield return new WaitForSeconds(0.01f);
                EndTurn();
                break;

            case "KISS":
                yield return StartCoroutine(KissRoutine(enemy, targetName));
                break;

            case "FRIEND":
                yield return StartCoroutine(FriendFlash(enemy));
                break;
        }

        activeRoutine = null;
    }

    IEnumerator FriendFlash(GameObject enemy)
    {
        SpriteRenderer psr = normalPlayer.GetComponent<SpriteRenderer>();
        SpriteRenderer esr = enemy != null ? enemy.GetComponent<SpriteRenderer>() : null;

        psr.color = friendYellow;
        if (esr != null) esr.color = friendYellow;

        HealPlayer(1);
        if (enemy != null) enemy.GetComponent<EnemyHP>().Heal(1);

        yield return new WaitForSeconds(0.15f);

        psr.color = Color.white;
        if (esr != null) esr.color = Color.white;

        EndTurn();
    }

    IEnumerator KissRoutine(GameObject enemy, string targetName)
    {
        if (kissSprite != null)
            normalPlayer.GetComponent<SpriteRenderer>().sprite = kissSprite;

        yield return new WaitForSeconds(0.6f);

        if (targetName == "YOU" && enemy != null)
        {
            enemy.GetComponent<EnemyHP>().TakeDamage(2);
            yield return StartCoroutine(FlashColor(enemy, Color.red));
        }
        else
        {
            PlayerTakeDamage(2);
            yield return StartCoroutine(FlashColor(normalPlayer, Color.red));
        }

        ResetVisual();
    }

    void ResetVisual()
    {
        isPlayerDefending = false;

        SpriteRenderer sr = normalPlayer.GetComponent<SpriteRenderer>();
        sr.sprite = originalSprite;
        sr.color = Color.white;
    }

    // ⭐ 이 부분이 수정되었습니다.
    public void PlayerTakeDamage(int amount)
    {
        // 1. 방어 상태라면 데미지를 입지 않고 방어막만 해제한 뒤 바로 종료합니다.
        if (isPlayerDefending) 
        {
            ResetVisual(); // 방어 해제 및 색상 복구
            return;        // 여기서 함수를 끝내야 아래 피 깎는 코드가 실행되지 않습니다.
        }

        // 2. 방어 상태가 아닐 때만 아래 코드가 실행되어 피가 깎입니다.
        for (int i = 0; i < amount; i++)
        {
            playerHP--;
            if (playerHP >= 0 && playerHP < playerHeartImages.Count)
            {
                Color c = playerHeartImages[playerHP].color;
                c.a = 0.2f;
                playerHeartImages[playerHP].color = c;
            }
        }

        if (playerHP <= 0)
        {
            SceneManager.LoadScene("Stage0");
        }
    }

    public void HealPlayer(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            if (playerHP < playerMaxHP)
            {
                Color c = playerHeartImages[playerHP].color;
                c.a = 1f;
                playerHeartImages[playerHP].color = c;
                playerHP++;
            }
        }
    }

    IEnumerator AttackJumpRoutine(GameObject enemy)
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

        yield return new WaitForSeconds(0.2f);

        enemy.GetComponent<EnemyHP>().TakeDamage(1);

        yield return StartCoroutine(FlashColor(enemy, Color.red));
    }

    IEnumerator FlashColor(GameObject t, Color c)
    {
        SpriteRenderer sr = t.GetComponent<SpriteRenderer>();

        sr.color = c;
        yield return new WaitForSeconds(0.3f);
        sr.color = Color.white;

        EndTurn();
    }

    void EndTurn()
    {
        SlotManager sm = Object.FindAnyObjectByType<SlotManager>();
        if (sm != null) sm.ResetSlots();

        if (isEnemyInRange) SetButtonsInteractable(true);
    }

    public void SetButtonsInteractable(bool s)
    {
        actionButton.interactable = s;
        targetButton.interactable = s;
    }
}