using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private StageController stageController;
    [SerializeField]
    private RectTransformMove menuPanel;
    [SerializeField]
    private TextMeshProUGUI textLevelInMenu;
    [SerializeField]
    private TextMeshProUGUI textLevelInGame;

    private Vector3 inactivePosition = Vector3.left * 1080;
    private Vector3 activePosition = Vector3.zero;

    private void Awake()
    {
        // 현재 스테이지 레벨 정보 얻어오기
        int index = PlayerPrefs.GetInt(Constants.StageLevel);
        // StageLevel에 저장된 값은 0부터 시작하기 때문에 +1을 해서 표시
        // "Go" 버튼에 표시되는 스테이지 레벨 갱신
        textLevelInMenu.text = $"Level {index + 1}";
    }

    public void ButtonClickEventStart()
    {
        // 현재 스테이지 레벨 정보 얻어오기
        int index = PlayerPrefs.GetInt(Constants.StageLevel);
        // StageLevel에 저장된 값은 0부터 시작하기 때문에 +1을 해서 표시
        // 과녁 오브젝트에 표시되는 스테이지 레벨 갱신
        textLevelInGame.text = $"{index + 1}";

        menuPanel.MoveTo(inactivePosition, AfterStartEvent);
    }

    private void AfterStartEvent()
    {
        stageController.IsGameStart = true;
    }

    public void ButtonClickEventReset()
    {
        PlayerPrefs.SetInt(Constants.StageLevel, 0);
        SceneManager.LoadScene(0);
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
        // 게임을 클리어해서 StageLevel이 +1되기 때문에
        // 메뉴가 닫힐 때 다음 레벨이 표시되도록 텍스트 정보를 갱신

        // 현재 스테이지 레벨 정보 얻어오기
        int index = PlayerPrefs.GetInt(Constants.StageLevel);
        // StageLevel에 저장된 값은 0부터 시작하기 때문에 +1을 해서 표시
        // "GO" 버튼에 표시되는 스테이지 레벨 갱신(스테이지 클리어 이후)
        textLevelInMenu.text = $"Level {index + 1}";

        menuPanel.MoveTo(activePosition, AfterStageExitEvent);
    }

    private void AfterStageExitEvent()
    {
        // 현재 스테이지 레벨 정보 얻어오기
        int index = PlayerPrefs.GetInt(Constants.StageLevel);
        
        // 마지막 스테이지를 클리어 했을 때 처리
        if(index == SceneManager.sceneCountInBuildSettings)
        {
            // 현재는 마지막 스테이지 클리어 시 첫 번째 스테이지로 리셋
            PlayerPrefs.SetInt(Constants.StageLevel, 0);
            SceneManager.LoadScene(0);
            return;
        }

        // 현재 스테이지 인덱스에 해당하는 씬 로드
        SceneManager.LoadScene(index);
    }
}
