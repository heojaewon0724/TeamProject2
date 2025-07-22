using UnityEngine;

// 플레이어 캐릭터를 사용자 입력에 따라 움직이는 스크립트
public class PlayerMovement : MonoBehaviour {
    public float moveSpeed = 5f; // 앞뒤 움직임의 속도
    public float rotateSpeed = 180f; // 좌우 회전 속도

    private Animator playerAnimator; // 플레이어 캐릭터의 애니메이터
    private PlayerInput playerInput; // 플레이어 입력을 알려주는 컴포넌트
    private Rigidbody playerRigidbody; // 플레이어 캐릭터의 리지드바디

    private void Start() {
        // 사용할 컴포넌트들의 참조를 가져오기
        playerInput = GetComponent<PlayerInput>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
    }

    // FixedUpdate는 물리 갱신 주기에 맞춰 실행됨
    private void FixedUpdate()
    {
        // 마우스 위치를 기준으로 캐릭터 회전
        Rotate();
        // 키보드 입력을 기준으로 캐릭터 이동
        Move();

        // 입력값에 따라 애니메이터의 Move 파라미터 값을 변경 (2D 이동량의 크기)
        float moveAmount = new Vector2(playerInput.moveHorizontal, playerInput.moveVertical).magnitude;
        playerAnimator.SetFloat("Move", moveAmount);
    }

    // 입력값에 따라 캐릭터를 앞뒤, 좌우로 움직임 (카메라 기준)
    private void Move() {
        // 카메라 기준 방향 구하기 (y=0으로 평면화)
        Vector3 camForward = Camera.main.transform.forward;
        camForward.y = 0f;
        camForward.Normalize();
        Vector3 camRight = Camera.main.transform.right;
        camRight.y = 0f;
        camRight.Normalize();
        // 카메라 기준 이동 방향 계산 (WASD/방향키 입력)
        Vector3 moveDirection =
            playerInput.moveVertical * camForward +
            playerInput.moveHorizontal * camRight;
        if (moveDirection.sqrMagnitude > 0.001f) {
            // 입력이 있을 때만 이동 (정규화된 방향 * 속도 * 시간)
            Vector3 moveDistance = moveDirection.normalized * moveSpeed * Time.fixedDeltaTime;
            // Rigidbody의 MovePosition으로 물리 이동
            playerRigidbody.MovePosition(playerRigidbody.position + moveDistance);
        }
        // 항상 velocity를 0으로 설정하여 물리 간섭 방지 (즉시 멈춤)
        playerRigidbody.linearVelocity = Vector3.zero;
        playerRigidbody.angularVelocity = Vector3.zero;
    }

    // 마우스 포인터 방향을 바라보도록 캐릭터 회전
    private void Rotate() {
        // 마우스 포인터가 가리키는 월드 위치 구하기 (카메라 기준)
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        // y=캐릭터 위치 평면(지면) 생성
        Plane groundPlane = new Plane(Vector3.up, transform.position);
        float rayDistance;
        // 마우스 광선이 평면과 만나는 지점 계산
        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 mouseWorldPos = ray.GetPoint(rayDistance); // 마우스가 가리키는 월드 좌표
            Vector3 lookDir = mouseWorldPos - transform.position; // 캐릭터에서 마우스까지 방향 벡터
            lookDir.y = 0f; // 수평 회전만 적용
            if (lookDir.sqrMagnitude > 0.001f)
            {
                // 해당 방향을 바라보도록 회전
                Quaternion targetRotation = Quaternion.LookRotation(lookDir);
                playerRigidbody.rotation = targetRotation;
            }
        }
    }
}