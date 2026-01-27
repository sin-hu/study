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

    [Header("전투 설정")]
    public float detectRange = 5f;
    public float jumpForce = 5f;
    public Button actionButton;
    public Button targetButton;

    private bool isEnemyInRange = false;
    private Rigidbody2D rb;
    private Coroutine activeRoutine;

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

            case "KISS":
                yield return StartCoroutine(KissRoutine(enemy, targetName));
                break;

            case "FRIEND":
                yield return StartCoroutine(FriendFlash(enemy));
                break;
                
            // DEFEND 케이스가 삭제되었습니다.
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

        yield return new WaitForSeconds(0.5f);

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
        // isPlayerDefending 관련 로직 삭제
        SpriteRenderer sr = normalPlayer.GetComponent<SpriteRenderer>();
        sr.sprite = originalSprite;
        sr.color = Color.white;
    }

    public void PlayerTakeDamage(int amount)
    {
        // 방어막 체크 로직 삭제 (무조건 데미지 입음)
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
        if (t == null) { EndTurn(); yield break; }
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
        if (actionButton != null) actionButton.interactable = s;
        if (targetButton != null) targetButton.interactable = s;
    }
}