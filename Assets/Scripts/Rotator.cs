using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    private StageController stageController;
    // 오브젝트의 회전 속도
    [SerializeField]
    private float rotateSpeed = 50f;
    [SerializeField]
    private float maxRotateSpeed = 500f;
    // 오브젝트의 회전 방향
    [SerializeField]
    private Vector3 rotateAxis = Vector3.forward;

    private void Update()
    {
        // IsGameStart가 false라는 것은 게임 시작 전이라는 의미이기 때문에 리턴하여 실행하지 못하게 만든다.
        if (stageController.IsGameStart == false)
        {
            return;
        }

        // 오브젝트의 z축을 rotateSpeed의 속도 만큼 움직인다.
        transform.Rotate(rotateAxis, rotateSpeed * Time.deltaTime);
    }

    public void Stop()
    {
        rotateSpeed = 0;
    }

    public void RotateFast()
    {
        rotateSpeed = maxRotateSpeed;
    }
}
