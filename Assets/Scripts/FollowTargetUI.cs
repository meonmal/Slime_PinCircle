using UnityEngine;

public class FollowTargetUI : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    private RectTransform rectTransform;
    private Camera mainCamera;

    private void Awake()
    {
        // RectTransform 컴포넌트 정보 얻어오기
        rectTransform = GetComponent<RectTransform>();
        // "Camera" 태그를 가지고 있는 카메라 오브젝트의 Camera 컴포넌트 정보 얻어오기
        mainCamera = Camera.main;
    }

    public void Setup(Transform target)
    {
        // UI가 쫓아다닐 대상 설정
        this.target = target;
    }

    private void LateUpdate()
    {
        // 월드에 target이 없으면 UI 오브젝트 삭제
        if(target == null)
        {
            Destroy(gameObject);
            return;
        }

        // 오브젝트의 위치가 갱신된 이후에 UI도 함께 위치를 설정하도록 하기 위해
        // LateUpdate()에서 UI 위치 설정

        // 목표 오브젝트의 월드 좌표를 기준으로 화면에서의 좌표 값을 구해 UI 위치 설정
        rectTransform.position = mainCamera.WorldToScreenPoint(target.position);
    }
}
