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
        }
    }

    public void SpawnThrowablePin(Vector3 position)
    {
        // 핀 오브젝트 생성
        GameObject clone = Instantiate(pinPrefab, position, Quaternion.identity);

        // "Pin" 컴포넌트 정보
        Pin pin = clone.GetComponent<Pin>();

        // 방금 생성된 핀 오브젝트의 "Pin" 컴포넌트를 리스트에 추가
        throwablePins.Add(pin);
    }

    public void spawnStuckPin(float angle)
    {
        // 핀 오브젝트 생성
        GameObject clone = Instantiate(pinPrefab);

        // 핀이 과녁에 배치될 수 있도록 설정
        SetInPinStuckToTarget(clone.transform, angle);
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
}
