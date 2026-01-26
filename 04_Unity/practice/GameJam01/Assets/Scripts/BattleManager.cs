using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleManager : MonoBehaviour
{
    [Header("플레이어 설정")]
    public GameObject heartPrefab;
    public Transform playerHeartParent; // 왼쪽 상단 그룹
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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        SpawnPlayerHearts();
        SetButtonsInteractable(false);
    }

    void SpawnPlayerHearts()
    {
        playerHP = playerMaxHP;
        for (int i = 0; i < playerMaxHP; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, playerHeartParent);
            Image img = newHeart.GetComponent<Image>();
            if (img != null) playerHeartImages.Add(img);
        }
    }

    void Update()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        bool found = false;
        foreach (GameObject enemy in enemies)
        {
            if (enemy != null && Vector2.Distance(transform.position, enemy.transform.position) <= detectRange)
            {
                found = true;
                break;
            }
        }
        if (found && !isEnemyInRange) { isEnemyInRange = true; SetButtonsInteractable(true); }
        else if (!found && isEnemyInRange) { isEnemyInRange = false; SetButtonsInteractable(false); }
    }

    public void ExecuteResult(string action, string targetName)
    {
        GameObject enemy = GameObject.FindWithTag("Enemy");

        // 새로운 행동 시작 시 내 방어 상태 해제 (색상 복구)
        if (action != "DEFEND") ResetPlayerDefense();

        if (action == "ATTACK")
        {
            if (targetName == "YOU") { if (enemy != null) StartCoroutine(AttackJumpRoutine(enemy)); else EndTurn(); }
            else { PlayerTakeDamage(); StartCoroutine(GetHitVisual(gameObject, Color.red)); }
        }
        else if (action == "DEFEND")
        {
            if (targetName == "YOU" && enemy != null) 
            {
                EnemyHP ehp = enemy.GetComponent<EnemyHP>();
                if(ehp != null) { ehp.isDefending = true; enemy.GetComponent<SpriteRenderer>().color = Color.cyan; }
            }
            else 
            {
                isPlayerDefending = true;
                GetComponent<SpriteRenderer>().color = Color.cyan;
            }
            EndTurn();
        }
        else { EndTurn(); }
    }

    void PlayerTakeDamage()
    {
        if (isPlayerDefending) { ResetPlayerDefense(); return; }

        playerHP--;
        if (playerHP >= 0 && playerHP < playerHeartImages.Count)
        {
            Color c = playerHeartImages[playerHP].color;
            c.a = 0.2f;
            playerHeartImages[playerHP].color = c;
        }
    }

    void ResetPlayerDefense()
    {
        isPlayerDefending = false;
        GetComponent<SpriteRenderer>().color = Color.white;
    }

    IEnumerator AttackJumpRoutine(GameObject enemy)
    {
        if (rb != null) rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        yield return new WaitForSeconds(0.2f);
        
        EnemyHP hp = enemy.GetComponent<EnemyHP>();
        if (hp != null) hp.TakeDamage();

        yield return StartCoroutine(GetHitVisual(enemy, Color.red));
    }

    IEnumerator GetHitVisual(GameObject target, Color flashColor)
    {
        SpriteRenderer sr = target.GetComponent<SpriteRenderer>();
        EnemyHP enemyHP = target.GetComponent<EnemyHP>();
        
        // 방어 중인지 체크 (적 혹은 플레이어)
        bool currentlyDefending = (enemyHP != null) ? enemyHP.isDefending : isPlayerDefending;

        if (sr != null && !currentlyDefending)
        {
            sr.color = flashColor;
            yield return new WaitForSeconds(0.3f);
            sr.color = Color.white;
        }
        else { yield return new WaitForSeconds(0.3f); }
        
        EndTurn();
    }

    void EndTurn() { SlotManager sm = Object.FindAnyObjectByType<SlotManager>(); if (sm != null) sm.ResetSlots(); ResetButtons(); }
    void ResetButtons() { if (isEnemyInRange) SetButtonsInteractable(true); }
    public void SetButtonsInteractable(bool state) { if (actionButton != null) actionButton.interactable = state; if (targetButton != null) targetButton.interactable = state; }
}