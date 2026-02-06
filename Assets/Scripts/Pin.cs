using System.Collections;
using UnityEngine;

public class Pin : MonoBehaviour
{
    // 핀의 막대 부분
    [SerializeField]
    private GameObject square;
    [SerializeField]
    private float moveTime = 0.2f;

    public void SetInPinStuckToTarget()
    {
        StopCoroutine(nameof(MoveTo));
        // 핀의 막대 부분 활성화
        square.SetActive(true);
    }

    public void MoveOneStep(float moveDistance)
    {
        StartCoroutine(nameof(MoveTo), moveDistance);
    }

    private IEnumerator MoveTo(float moveDistance)
    {
        Vector3 start = transform.position;
        Vector3 end = transform.position + Vector3.up * moveDistance;
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime / moveTime;
            transform.position = Vector3.Lerp(start, end, percent);

            yield return null;
        }
    }
}
