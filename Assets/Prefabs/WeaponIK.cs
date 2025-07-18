using UnityEngine;

public class WeaponIK : MonoBehaviour
{
    public Animator animator;
    public Transform leftHandle;
    public Transform rightHandle;

    void OnAnimatorIK(int layerIndex)
    {
        if (animator == null) return;

        // 오른손
        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandle.position);
        animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandle.rotation);

        // 왼손
        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1f);
        animator.SetIKPosition(AvatarIKGoal.LeftHand, leftHandle.position);
        animator.SetIKRotation(AvatarIKGoal.LeftHand, leftHandle.rotation);
    }
}
