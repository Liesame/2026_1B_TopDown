using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Monster monsterData; // ScriptableObject 연결
    private int currentHp;

    private Rigidbody2D rb;
    void Start()
    {
        if (monsterData != null)
        {
            currentHp = monsterData.Hp; // 데이터로부터 초기 체력 설정
        }

        rb = GetComponent<Rigidbody2D>();
    }

    // 플레이어로부터 공격을 받을 때 호출되는 함수
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        Debug.Log($"{gameObject.name}이(가) {damage}의 데미지를 입음. 남은 체력: {currentHp}");

        if (currentHp <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // 이전 질문의 도전과제 데이터가 있다면 여기서 연동 가능
        if (GameDataManager.Instance != null)
        {
            GameDataManager.Instance.saveData.killEnemy = true;

            GameDataManager.Instance.AddScore(10);
        }

        Destroy(gameObject); // 적 제거
    }
}