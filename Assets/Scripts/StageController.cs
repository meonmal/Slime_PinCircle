using UnityEngine;

public class StageController : MonoBehaviour
{
    // 핀 생성을 위한 핀 스포너 컴포넌트
    [SerializeField]
    private PinSpawner pinSpawner;
    // 현재 스테이지를 클리어하기 위해 던져야 하는 핀 갯수
    [SerializeField]
    private int throwblePinCount;
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

    private void Awake()
    {
        // 게임 하단에 배치되는 던져야 하는 핀 오브젝트 생성
        for (int i = 0; i < throwblePinCount; i++)
        {
            pinSpawner.SpawnThrowablePin(firstTPinPosition + Vector3.down * TPinDistance * i);
        }

        // 게임을 시작할 때 과녁에 배치되어 있는 핀 오브젝트 생성
        // stuckPinCount가 -1이 아니면 stuckPinCount 개수의 핀을 일정한 간격으로 배치
        if(stuckPinCount != -1)
        {
            for (int i = 0; i < stuckPinCount; i++)
            {
                // 과녁에 배치되는 핀의 개수에 따라 일정한 간격으로 배치될 때 배치 각도
                float angle = (360 / stuckPinCount) * i;

                pinSpawner.spawnStuckPin(angle);
            }
        }
        // stuckPinCount가 -1이면 stuckAngles 배열에 있는 각도에 따라 핀이 배치
        else
        {
            for(int i = 0; i<stuckPinAngles.Length; i++)
            {
                pinSpawner.spawnStuckPin(stuckPinAngles[i]);
            }
        }
    }
}
