using UnityEngine;

public class StageController : MonoBehaviour
{
    // 핀 생성을 위한 핀 스포너 컴포넌트
    [SerializeField]
    private PinSpawner pinSpawner;
    // 배경 색상 변경을 위한 Camera 컴포넌트
    [SerializeField]
    private Camera mainCamera;
    // 핀이 배치되는 타겟 오브젝트 회전체
    [SerializeField]
    private Rotator rotatorTarget;
    // 현재 스테이지를 클리어하기 위해 던져야 하는 핀 갯수
    [SerializeField]
    private int throwablePinCount;
    // 스테이지 시작 시 과녁에 고정되어 있는 핀 갯수
    [SerializeField]
    private int stuckPinCount = -1;
    // 스테이지 시작 시 과녁에 고정되어 있는 모든 핀의 각도 배열
    [SerializeField]
    private int[] stuckPinAngles;

    // 게임 화면 하단에 배치되는 던져야 할 핀들의 첫 번째 핀 위치
    private Vector3 firstTPinPosition = Vector3.down * 2;
    // 던져야 하는 핀들 사이의 배치 거리
    public float TPinDistance { private set; get; } = 1;

    // 게임오버 되었을 때 배경 색상
    private Color failBackGroundColor = new Color(0.4f, 0.1f, 0.1f);
    
    // 게임 제어를 위한 변수
    public bool IsGameOver { set; get; } = false;

    private void Awake()
    {
        // 게임 하단에 배치되는 던져야 하는 핀 오브젝트 생성
        for (int i = 0; i < throwablePinCount; i++)
        {
            pinSpawner.SpawnThrowablePin(firstTPinPosition + Vector3.down * TPinDistance * i, throwablePinCount-i);
        }

        // 게임을 시작할 때 과녁에 배치되어 있는 핀 오브젝트 생성
        // stuckPinCount가 -1이 아니면 stuckPinCount 개수의 핀을 일정한 간격으로 배치
        if(stuckPinCount != -1)
        {
            for (int i = 0; i < stuckPinCount; i++)
            {
                // 과녁에 배치되는 핀의 개수에 따라 일정한 간격으로 배치될 때 배치 각도
                float angle = (360 / stuckPinCount) * i;

                pinSpawner.SpawnStuckPin(angle, throwablePinCount+1+i);
            }
        }
        // stuckPinCount가 -1이면 stuckAngles 배열에 있는 각도에 따라 핀이 배치
        else
        {
            for(int i = 0; i<stuckPinAngles.Length; i++)
            {
                pinSpawner.SpawnStuckPin(stuckPinAngles[i], throwablePinCount+1+i);
            }
        }
    }

    public void GameOver()
    {
        // Pin 과 Pin이 충돌할 때 GameOver()가 중복으로 호출됨(오류 x, 최대 8번 정도)
        // 때문에 한번이라도 GameOver()가 호출되면 더 이상 실행되지 않도록 제어
        if(IsGameOver == true)
        {
            return;
        }

        IsGameOver = true;

        // 배경 색상 변경
        mainCamera.backgroundColor = failBackGroundColor;

        // 과녁 오브젝트 회전 중지
        rotatorTarget.Stop();
    }
}
