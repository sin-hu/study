using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerHP : MonoBehaviour
{
    [Header("하트 UI 설정")]
    public GameObject heartPrefab;
    public Transform heartParent;
    public int maxHP = 5;
    private List<Image> heartImages = new List<Image>();
    private int currentHP;

    [Header("이미지 및 연출")]
    public SpriteRenderer characterSprite; 
    public Sprite idleSprite;    // 평소 펭귄
    public Sprite dashSprite;    // 대쉬 펭귄 (뽀뽀용)
    public GameObject kissLipsUI; // 입술 오브젝트

    private bool isDefending = false;

    void Start()
    {
        currentHP = maxHP;
        if (kissLipsUI != null) kissLipsUI.SetActive(false);
        SpawnHearts();
    }

    void SpawnHearts()
    {
        foreach (Transform child in heartParent) { Destroy(child.gameObject); }
        heartImages.Clear();
        for (int i = 0; i < maxHP; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartParent);
            heartImages.Add(newHeart.GetComponent<Image>());
        }
    }

    // 뽀뽀 연출: 대쉬 이미지로 바꾸고 입술 띄우기
    public IEnumerator KissEffectRoutine()
    {
        characterSprite.sprite = dashSprite;
        if (kissLipsUI != null) kissLipsUI.SetActive(true);
        characterSprite.color = Color.red; 

        yield return new WaitForSeconds(1.0f);

        characterSprite.sprite = idleSprite;
        if (kissLipsUI != null) kissLipsUI.SetActive(false);
        UpdateVisualStatus();
    }

    public void TakeDamage(int amount)
    {
        if (isDefending) { SetDefense(false); return; }
        currentHP = Mathf.Max(0, currentHP - amount);
        UpdateHeartUI();
        StartCoroutine(FlashColor(Color.red));
    }

    public void Heal(int amount)
    {
        currentHP = Mathf.Min(maxHP, currentHP + amount);
        UpdateHeartUI();
        StartCoroutine(FlashColor(Color.yellow)); // 친구맺기/회복 시 노란색
    }

    public void SetDefense(bool state)
    {
        isDefending = state;
        UpdateVisualStatus();
    }

    void UpdateVisualStatus()
    {
        // 방어 중이면 파란색, 아니면 흰색
        characterSprite.color = isDefending ? new Color(0.5f, 0.5f, 1f) : Color.white;
    }

    IEnumerator FlashColor(Color targetColor)
    {
        characterSprite.color = targetColor;
        yield return new WaitForSeconds(0.6f);
        UpdateVisualStatus();
    }

    void UpdateHeartUI()
    {
        for (int i = 0; i < heartImages.Count; i++)
        {
            Color c = heartImages[i].color;
            c.a = (i < currentHP) ? 1f : 0.2f;
            heartImages[i].color = c;
        }
    }
}