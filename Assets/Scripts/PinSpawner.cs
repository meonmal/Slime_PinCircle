using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class PinSpawner : MonoBehaviour
{
    [Header("Commons")]
    // StageController 컴포넌트 정보
    [SerializeField]
    private StageController stageController;
    // 핀 프리팹
    [SerializeField]
    private GameObject pinPrefab;
    // 핀에 숫자를 표시하는 Text UI
    [SerializeField]
    private GameObject textPinIndexPrefab;
    // 핀 Text가 배치되는 Panel Transform
    [SerializeField]
    private Transform textParent;

    [Header("Stuck Pin")]
    // 과녁 오브젝트의 Trnasform
    [SerializeField]
    private Transform targetTransform;
    // 과녁의 위치
    [SerializeField]
    private Vector3 targetPosition = Vector3.up * 2;
    // 과녁의 반지름
    [SerializeField]
    private float targetRadius = 0.8f;
    // 핀 막대 길이
    [SerializeField]
    private float pinLength = 1.5f;

    [Header("Throwble Pin")]
    // 게임 도중 마우스 클릭으로 배치되는 핀의 각도
    [SerializeField]
    private float bottomAngle = 270;
    // 하단에 생성되는 던져야 할 핀 오브젝트 리스트
    private List<Pin> throwablePins = new List<Pin>();

    private void Update()
    {
        // 게임오버시 실행x 
        if(stageController.IsGameOver == true)
        {
            return;
        }

        // 게임 진행 도중 플레이어가 마우스 왼쪽 버튼을 클릭하면 실행
        if(Input.GetMouseButtonDown(0) && throwablePins.Count > 0)
        {
            // throwablePins 리스트에 저장된 첫번째 핀을 과녁에 배치
            SetInPinStuckToTarget(throwablePins[0].transform, bottomAngle);
            // 방금 과녁에 배치한 첫번째 핀 요소를 리스트에서 삭제
            throwablePins.RemoveAt(0);

            // 과녁에 배치되지 않은 throwablePins 리스트의 모든 핀 위치 이동
            for(int i=0; i<throwablePins.Count; i++)
            {
                throwablePins[i].MoveOneStep(stageController.TPinDistance);
            }

            stageController.DecreaseThrowablePin();
        }
    }

    public void SpawnThrowablePin(Vector3 position, int index)
    {
        // 핀 오브젝트 생성
        GameObject clone = Instantiate(pinPrefab, position, Quaternion.identity);

        // "Pin" 컴포넌트 정보
        Pin pin = clone.GetComponent<Pin>();
        pin.Setup(stageController);

        // 방금 생성된 핀 오브젝트의 "Pin" 컴포넌트를 리스트에 추가
        throwablePins.Add(pin);

        // 핀 오브젝트에 표시되는 Text UI 생성
        SpawnTextUI(clone.transform, index);
    }

    public void SpawnStuckPin(float angle, int index)
    {
        // 핀 오브젝트 생성
        GameObject clone = Instantiate(pinPrefab);

        // "Pin" 컴포넌트 정보를 얻어와 Setup() 함수 호출
        Pin pin = clone.GetComponent<Pin>();
        pin.Setup(stageController);

        // 핀이 과녁에 배치될 수 있도록 설정
        SetInPinStuckToTarget(clone.transform, angle);

        // 핀 오브젝트에 표시되는 Text UI 생성
        SpawnTextUI(clone.transform, index);
    }

    private void SetInPinStuckToTarget(Transform pin, float angle)
    {
        // 타겟의 해당 각도에 핀이 꽃혔을 때 위치
        pin.position = Utils.GetPositionFromAngle(targetRadius + pinLength, angle) + targetPosition;
        // 핀 오브젝트 회전 설정
        pin.rotation = Quaternion.Euler(0, 0, angle);
        // 핀 오브젝트를 target의 자식으로 설정해서 target과 같이 회전하도록 한다.
        pin.SetParent(targetTransform);
        // 핀이 과녁에 배치되었을 때 설정
        pin.GetComponent<Pin>().SetInPinStuckToTarget();
    }

    private void SpawnTextUI(Transform target, int index)
    {
        // 숫자를 아타내는 Text UI를 생성하고, 부모를 textParent로 설정
        GameObject textClone = Instantiate(textPinIndexPrefab, textParent);
        // 계층 설정으로 바뀐 크기를 다시 (1, 1, 1)로 설정
        textClone.transform.localScale = Vector3.one;
        // UI가 쫓아다닐 대상 설정
        textClone.GetComponent<FollowTargetUI>().Setup(target);
        // UI에 표시되는 텍스트 내용
        textClone.GetComponent<TMPro.TextMeshProUGUI>().text = index.ToString();
    }
}
