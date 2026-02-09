using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private StageController stageController;
    [SerializeField]
    private RectTransformMove menuPanel;

    private Vector3 inactivePosition = Vector3.left * 1080;
    private Vector3 activePosition = Vector3.zero;

    public void ButtonClickEventStart()
    {
        menuPanel.MoveTo(inactivePosition, AfterStartEvent);
    }

    private void AfterStartEvent()
    {
        Debug.Log("링크 스타트!");
        stageController.IsGameStart = true;
    }

    public void ButtonClickEventReset()
    {
        Debug.Log("게임 종료");
    }

    public void ButtonClickEventExit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }

    public void StageExit()
    {
        menuPanel.MoveTo(activePosition, AfterStageExitEvent);
    }

    private void AfterStageExitEvent()
    {
        SceneManager.LoadScene(0);
    }
}
