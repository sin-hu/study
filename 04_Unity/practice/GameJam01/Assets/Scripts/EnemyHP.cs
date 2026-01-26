using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class EnemyHP : MonoBehaviour
{
    [Header("HP UI")]
    public GameObject heartPrefab;    
    public Transform heartParent;     // EnemyHP_Group 연결
    public int maxHP = 5;

    private List<Image> heartImages = new List<Image>();
    private int currentHP;
    private bool isDead = false;

    [HideInInspector] public bool isDefending = false; 

    void Start()
    {
        foreach (Transform child in heartParent) { Destroy(child.gameObject); }
        currentHP = maxHP;
        SpawnHearts();
    }

    void SpawnHearts()
    {
        for (int i = 0; i < maxHP; i++)
        {
            GameObject newHeart = Instantiate(heartPrefab, heartParent);
            // 중요: 생성 시 크기가 0이 되지 않도록 1로 고정
            newHeart.transform.localScale = Vector3.one; 
            
            Image img = newHeart.GetComponent<Image>();
            if (img != null) heartImages.Add(img);
        }
        // 오른쪽 하트부터 깎이도록 리스트 반전
        heartImages.Reverse();
    }

    public bool TakeDamage()
    {
        if (isDead) return true;

        if (isDefending)
        {
            isDefending = false;
            GetComponent<SpriteRenderer>().color = Color.white; 
            return false;
        }

        currentHP--;
        
        if (currentHP >= 0 && currentHP < heartImages.Count)
        {
            int index = (heartImages.Count - 1) - currentHP;
            Color c = heartImages[index].color;
            c.a = 0.2f; 
            heartImages[index].color = c;
        }
        
        if (currentHP <= 0)
        {
            isDead = true;
            StartCoroutine(FlyAwayRoutine());
        }
        
        return isDead;
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