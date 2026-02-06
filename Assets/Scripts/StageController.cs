using UnityEngine;

public class StageController : MonoBehaviour
{
    // 핀 생성을 위한 핀 스포너 컴포넌트
    [SerializeField]
    private PinSpawner pinSpawner;
    // 현재 스테이지를 클리어하기 위해 던져야 하는 핀 갯수
    [SerializeField]
    private int throwblePinCount;

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
    }
}
