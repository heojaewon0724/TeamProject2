using UnityEngine;
<<<<<<< HEAD
=======
using System.Collections;
>>>>>>> 90dd7007b2e7f1dae1b5324872491f5bfa98f5f0
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
<<<<<<< HEAD
	public class StarterAssetsInputs : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;



		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

		[Header("Combat Settings")]
		public bool attack;
		public bool reload;


#if ENABLE_INPUT_SYSTEM
		public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}


		public void OnLook(InputValue value)
		{
			if (cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		}

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}

		private void OnApplicationFocus(bool hasFocus)
		{
			SetCursorState(cursorLocked);
		}

		private void SetCursorState(bool newState)
		{
			Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		}

#if ENABLE_INPUT_SYSTEM
		public void OnAttack(InputValue value)
		{
			AttackInput(value.isPressed);
		}
		public void OnReload(InputValue value)
		{
			ReloadInput(value.isPressed);
		}
#endif

		public void AttackInput(bool newAttackState)
		{
			attack = newAttackState;

		}
		public void ReloadInput(bool newReloadstate) {
			reload = newReloadstate;
		}
	}
	
}
=======
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool sprint;
        public bool leftclick;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("Combat Settings")]
        public bool attack;

        [Header("Weapon Settings")]
        public GameObject weaponObject;      // 무기 오브젝트를 인스펙터에서 직접 드래그 연결
        public float attackActiveTime = 0.3f;

        private Collider weaponCollider;     
        private bool isAttacking = false;

#if ENABLE_INPUT_SYSTEM
        public void OnMove(InputValue value) => MoveInput(value.Get<Vector2>());
        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
                LookInput(value.Get<Vector2>());
        }
        public void OnJump(InputValue value) => JumpInput(value.isPressed);
        public void OnSprint(InputValue value) => SprintInput(value.isPressed);
        public void OnAttack(InputValue value) => AttackInput(value.isPressed);
#endif

        public void MoveInput(Vector2 newMoveDirection) => move = newMoveDirection;
        public void LookInput(Vector2 newLookDirection) => look = newLookDirection;
        public void JumpInput(bool newJumpState) => jump = newJumpState;
        public void SprintInput(bool newSprintState) => sprint = newSprintState;

        public void AttackInput(bool newAttackState)
        {
            attack = newAttackState;
            if (attack && !isAttacking)
            {
                StartCoroutine(AttackRoutine());
            }
        }

        private void Awake()
        {
            if (weaponObject != null)
            {
                // 무기 오브젝트에서 Collider 컴포넌트 찾기
                weaponCollider = weaponObject.GetComponent<Collider>();
                if (weaponCollider == null)
                    Debug.LogWarning("무기 오브젝트에 Collider 컴포넌트가 없습니다!");
                else
                    weaponCollider.enabled = false;
            }
            else
            {
                Debug.LogWarning("weaponObject가 Inspector에 할당되지 않았습니다!");
            }
        }

        private IEnumerator AttackRoutine()
        {
            yield return new WaitForSeconds(0.05f);
            isAttacking = true;
            if (weaponCollider != null)
                weaponCollider.enabled = true;

            yield return new WaitForSeconds(attackActiveTime);

            if (weaponCollider != null)
                weaponCollider.enabled = false;

            isAttacking = false;
            attack = false;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
>>>>>>> 90dd7007b2e7f1dae1b5324872491f5bfa98f5f0
