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
    private GameObject addBtn;
    [SerializeField]
    private GameObject howToPlayUi;

    [SerializeField]
    private TextMeshProUGUI CurrentLevelText;
    [SerializeField]
    private TextMeshProUGUI MovesLeft;


    [Header("Game Data")]
    [SerializeField]
    private SO_AudioChannel audioChannel;
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
            TransactionEventChannel.onGameOver += LevelFailed;
        }
        else
        {
            DebuggingTools.PrintMessage("TransactionEvent Channel is empty", DebuggingTools.DebugMessageType.ERROR, this);

        }
        if (uIChannel != null)
        {
            uIChannel.onUpdateLevelText += UpdateLevelAndMovesText;
            uIChannel.onUpdateMovesLeft += UpdateMovesText;


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
            TransactionEventChannel.onGameOver -= LevelFailed;
        }
        if (uIChannel != null)
        {
            uIChannel.onUpdateLevelText -= UpdateLevelAndMovesText;
            uIChannel.onUpdateMovesLeft -= UpdateMovesText;


        }


    }

    private void Awake()
    {
        homeScreen.SetActive(true);
    }

    public void StartGame()
    {
        audioChannel.OnUiClick();
        homeScreen.SetActive(false);
        howToPlayUi.SetActive(true);
        

    }

    public void LevelComplete()
    {
        audioChannel.OnUiClick(); 
        inGameUi.SetActive(false);
        levelCompleteUi.SetActive(true);
        addBtn.SetActive(true);

    }

    public void NextLevel()
    {
        audioChannel.OnUiClick(); 
        uIChannel.OnNextLevel(true);
        levelCompleteUi.SetActive(false);
        inGameUi.SetActive(true);
        addBtn.SetActive(true);


    }



    public void BackToHome()
    {
        audioChannel.OnUiClick(); 
        levelCompleteUi.SetActive(false);
        inGameUi.SetActive(false);
        homeScreen.SetActive(true);
        addBtn.SetActive(true);

        uIChannel.OnBackToHomeAction();

    }

    public void AddExtraContainer()
    {
        audioChannel.OnUiClick(); 
        uIChannel.OnAddExtraContainer();
        addBtn.SetActive(false);
    }


    public void Undo()
    {
        audioChannel.OnUiClick(); 
        uIChannel.OnUnDo();
    }

    public void LevelFailed()
    {
        audioChannel.OnUiClick(); 
        inGameUi.SetActive(false);
        levelFailedUi.SetActive(true);

    }

    public void RetryLevel()
    {
        audioChannel.OnUiClick(); 
        uIChannel.OnNextLevel(false);

        levelCompleteUi.SetActive(false);
        inGameUi.SetActive(true);
        addBtn.SetActive(true);
        levelFailedUi.SetActive(false);
    }
 


    public void CloaseHowToPlay()
    {
        audioChannel.OnUiClick(); 
        howToPlayUi.SetActive(false);
        uIChannel.OnStartGame();
        inGameUi.SetActive(true);
        addBtn.SetActive(true);

    }


    public void Quit()
    {
        audioChannel.OnUiClick(); 
        Application.Quit();
    }


        

    public void UpdateLevelAndMovesText(int level,int movesAvailable)
    {
        CurrentLevelText.text = $"{level}";
        MovesLeft.text = $"{movesAvailable}";
    }
    public void UpdateMovesText(int movesAvailable)
    {
        MovesLeft.text = $"{movesAvailable}";
    }
}
