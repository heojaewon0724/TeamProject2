using UnityEngine;

public class AttackRange : MonoBehaviour
{
    public Enemy enemy; // 부모 Enemy 스크립트 참조 (Inspector에 할당)

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetAttacking(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            enemy.SetAttacking(false);
        }
    }
}
