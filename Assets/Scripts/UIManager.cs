using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI Stuff")]
    [SerializeField]
    private GameObject homeScreen;
    [SerializeField]
    private GameObject inGameUi;
    [SerializeField]
    private GameObject levelCompleteUi;
    [SerializeField]
    private GameObject levelFailedUi;

    [SerializeField]
    private TextMeshProUGUI CurrentLevelText;
    [SerializeField]
    private TextMeshProUGUI MovesLeft;


    [Header("Game Data")]
    [SerializeField]
    private SO_UIChannel uIChannel;
    [SerializeField]
    private SO_TransactionEventChannel TransactionEventChannel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnEnable()
    {
        if (TransactionEventChannel != null)
        {
            TransactionEventChannel.onWin += LevelComplete;
        }
        else
        {
            DebuggingTools.PrintMessage("TransactionEvent Channel is empty", DebuggingTools.DebugMessageType.ERROR, this);

        }
        if (uIChannel != null)
        {
            uIChannel.onUpdateLevelText += UpdateLevelText;


        }
        else
        {
            DebuggingTools.PrintMessage("UI Channel is empty", DebuggingTools.DebugMessageType.ERROR, this);

        }

    }


    private void OnDisable()
    {
        if (TransactionEventChannel != null)
        {
            TransactionEventChannel.onWin -= LevelComplete;
        }
        if (uIChannel != null)
        {
            uIChannel.onUpdateLevelText -= UpdateLevelText;


        }


    }
    public void StartGame()
    {

        uIChannel.OnStartGame();
        homeScreen.SetActive(false);
        inGameUi.SetActive(true);


    }

    public void LevelComplete()
    {
        inGameUi.SetActive(false);
        levelCompleteUi.SetActive(true);
    }

    public void NextLevel()
    {
        uIChannel.OnNextLevel();
        levelCompleteUi.SetActive(false);
        inGameUi.SetActive(true);

    }



    public void BackToHome()
    {
        levelCompleteUi.SetActive(false);
        inGameUi.SetActive(false);
        homeScreen.SetActive(true);

        uIChannel.OnBackToHomeAction();

    }



    public void LevelFailed()
    {
        inGameUi.SetActive(false);
        levelFailedUi.SetActive(true);
    }

    public void Quit()
    {
        Application.Quit();
    }



    public void UpdateLevelText(int level)
    {
        CurrentLevelText.text = $"{level}";
    }
}
