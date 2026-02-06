using UnityEngine;

public class Pin : MonoBehaviour
{
    // 핀의 막대 부분
    [SerializeField]
    private GameObject square;

    public void SetInPinStuckToTarget()
    {
        // 핀의 막대 부분 활성화
        square.SetActive(true);
    }
}
