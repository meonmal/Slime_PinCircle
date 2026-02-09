using System.Collections;
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
    // 핀 인덱트 Text가 배치된 Panel 오브젝트 회전체
    [SerializeField]
    private Rotator rotatorIndexPanel;
    // 메인 메뉴 이동을 위한 MainMenu 컴포넌트
    [SerializeField]
    private MainMenu mainMenu;
    // 현재 스테이지를 클리어하기 위해 던져야 하는 핀 갯수
    [SerializeField]
    private int throwablePinCount;
    // 스테이지 시작 시 과녁에 고정되어 있는 핀 갯수
    [SerializeField]
    private int stuckPinCount = -1;
    // 스테이지 시작 시 과녁에 고정되어 있는 모든 핀의 각도 배열
    [SerializeField]
    private int[] stuckPinAngles;

    // 게임 오버 사운드
    [SerializeField]
    private AudioClip audioGameOver;
    // 게임 클리어 사운드
    [SerializeField]
    private AudioClip audioGameClear;
    // 사운드 재생을 위한 AudioSource 컴포넌트
    private AudioSource audioSource;

    // 게임 화면 하단에 배치되는 던져야 할 핀들의 첫 번째 핀 위치
    private Vector3 firstTPinPosition = Vector3.down * 2;
    // 던져야 하는 핀들 사이의 배치 거리
    public float TPinDistance { private set; get; } = 1;

    // 게임오버 되었을 때 배경 색상
    private Color failBackGroundColor = new Color(0.4f, 0.1f, 0.1f);
    // 게임클리어가 되었을 때 배경 색상
    private Color clearBackgroundColor = new Color(0, 0.5f, 0.25f);
    
    // 게임 제어를 위한 변수
    public bool IsGameOver { set; get; } = false;
    public bool IsGameStart { set; get; } = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

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

        PlaySound(audioGameOver);

        // 0.5초 대기 후 스테이지 종료 이벤트 처리
        StartCoroutine(nameof(StageExit), 0.5f);
    }

    public void DecreaseThrowablePin()
    {
        throwablePinCount--;

        // 모든 핀을 과녁에 명중했을 때 Clear 처리
        if(throwablePinCount <= 0)
        {
            // 일반 메소드로 작성하면 마지막 핀을 던졌을 때
            // 핀의 충돌 여부에 관계 없이 무조건 클리어 된다.
            StartCoroutine(nameof(GameClear));
        }
    }

    private IEnumerator GameClear()
    {
        // Pin의 충돌 검사 이후에 GameClear()가 실행될 수 잇도록
        // 짧은 시간 대기한 이후 IsGameClear가 false이면 GameClear() 함수를 실행
        yield return new WaitForSeconds(0.1f);

        // GameOver()가 실행되어 IsGaemOver가 true면 코루틴 중지
        if(IsGameOver == true)
        {
            yield break;
        }

        // 배경 색상 변경
        mainCamera.backgroundColor = clearBackgroundColor;

        // 과녁 오브젝트를 빠르게 회전
        rotatorTarget.RotateFast();
        // Text가 배치되어 있는 Panel을 빠르게 회전
        rotatorIndexPanel.RotateFast();

        // 현재 스테이지 레벨 정보를 얻어와 레벨을 +1 한다
        // Get할 때 저장된 정보가 없으면 0이 반환되기 때문에 index가 0부터 시작하게 된다.
        int index = PlayerPrefs.GetInt(Constants.StageLevel);
        PlayerPrefs.SetInt(Constants.StageLevel, index + 1);

        PlaySound(audioGameClear);

        StartCoroutine(nameof(StageExit), 1f);
    }

    private IEnumerator StageExit(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        mainMenu.StageExit();
    }

    private void PlaySound(AudioClip newClip)
    {
        audioSource.Stop();
        audioSource.clip = newClip;
        audioSource.Play();
    }
}
