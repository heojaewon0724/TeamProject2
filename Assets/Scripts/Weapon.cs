using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float damage = 25f;

    // 무기 오브젝트에 Collider(Trigger 체크) 필요!
    private void OnTriggerEnter(Collider other)
    {
        // Enemy 스크립트가 붙은 적과 충돌하면
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }
}
