using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EnemyHP : MonoBehaviour
{
    [Header("UI 연결")]
    public GameObject statusUIRoot;   // EnemyStatus_Root (부모)
    public GameObject heartPrefab;    
    public Transform heartParent;     
    public TextMeshProUGUI nameText;  
    public SpriteRenderer characterSprite; // 물범 이미지

    [Header("적 설정")]
    public string enemyName = "물범";
    public int maxHP = 5;

    private List<Image> heartImages = new List<Image>();
    private int currentHP;
    private bool isDead = false;

    [HideInInspector] public bool isDefending = false; 

    void Start()
    {
        // 처음에는 UI를 숨김
        if (statusUIRoot != null) statusUIRoot.SetActive(false);
        if (nameText != null) nameText.text = enemyName;

        foreach (Transform child in heartParent) { Destroy(child.gameObject); }
        currentHP = maxHP;
        SpawnHearts();
    }

    void SpawnHearts()
    {
        for (int i = 0; i < maxHP; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartParent);
            newHeart.transform.localScale = Vector3.one; 
            Image img = newHeart.GetComponent<Image>();
            if (img != null) heartImages.Add(img);
        }
        heartImages.Reverse();
    }

    // 전투 시작 시 호출
    public void ShowUI() { if (statusUIRoot != null) statusUIRoot.SetActive(true); }

    public bool TakeDamage(int amount)
    {
        if (isDead) return true;

        if (isDefending)
        {
            SetDefense(false); // 방어 해제
            return false;
        }

        for (int i = 0; i < amount; i++)
        {
            currentHP--;
            if (currentHP >= 0 && currentHP < heartImages.Count)
            {
                int index = (heartImages.Count - 1) - currentHP;
                Color c = heartImages[index].color;
                c.a = 0.2f;
                heartImages[index].color = c;
            }
        }
        
        StartCoroutine(FlashColor(Color.red)); // 빨간색 피격 효과
        
        if (currentHP <= 0)
        {
            isDead = true;
            if (statusUIRoot != null) statusUIRoot.SetActive(false);
            StartCoroutine(FlyAwayRoutine());
        }
        return isDead;
    }

    public void Heal(int amount)
    {
        if (isDead) return;
        for (int i = 0; i < amount; i++)
        {
            if (currentHP < maxHP)
            {
                int index = (heartImages.Count - 1) - currentHP;
                Color c = heartImages[index].color;
                c.a = 1.0f;
                heartImages[index].color = c;
                currentHP++;
            }
        }
        StartCoroutine(FlashColor(Color.yellow)); // 친구맺기 노란색 효과
    }

    public void SetDefense(bool state)
    {
        isDefending = state;
        characterSprite.color = state ? new Color(0.5f, 0.5f, 1f) : Color.white; // 파란색 방어
    }

    IEnumerator FlashColor(Color targetColor)
    {
        characterSprite.color = targetColor;
        yield return new WaitForSeconds(0.6f);
        if (!isDefending) characterSprite.color = Color.white;
        else SetDefense(true); // 방어 중이면 파란색 유지
    }

    IEnumerator FlyAwayRoutine()
    {
        if (GetComponent<Collider2D>()) GetComponent<Collider2D>().enabled = false;
        float elapsed = 0f;
        while (elapsed < 2.0f)
        {
            transform.Translate(new Vector3(1.5f, 2f, 0) * 8f * Time.deltaTime, Space.World);
            transform.Rotate(Vector3.forward * 700f * Time.deltaTime);
            elapsed += Time.deltaTime;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}