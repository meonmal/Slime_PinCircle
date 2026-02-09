using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class RectTransformMove : MonoBehaviour
{
    [SerializeField]
    private float moveTime = 1.0f;
    private RectTransform rectTransform;
    private bool isMoved = false;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void MoveTo(Vector3 position, UnityAction action = null)
    {
        if(isMoved == false)
        {
            StartCoroutine(OnMove(position, action));
        }
    }

    private IEnumerator OnMove(Vector3 end, UnityAction action = null)
    {
        isMoved = true;

        Vector3 start = rectTransform.anchoredPosition;
        float percent = 0;

        while(percent < 1)
        {
            percent += Time.deltaTime / moveTime;
            rectTransform.anchoredPosition = Vector3.Lerp(start, end, percent);

            yield return null;
        }

        // action에 등록되어 있는 메소드가 있으면 실행
        // action?.Invoke(); 와 동일
        if(action != null)
        {
            action.Invoke();
        }

        isMoved = false;
    }
}
