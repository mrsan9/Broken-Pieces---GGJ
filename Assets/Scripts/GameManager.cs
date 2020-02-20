using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    #region UI
    //[EnumNamedArray(typeof(EPanel))]
    public List<GameObject> panels;
    EPanel currentActivePanel = EPanel.None;

    public Button gameplayLeftButton,gameplayRightButton;
    public Button menuPlayButton;
    public Button menuCreditsButton;
    public Button menuExitButton;
    public Button gameplayPauseButton;
    public Button gamePlayResumeButton;
    public Button gamePlayExitButton;
    public Button nextLevelButton;
    public Button creditBackButton;
    public Button gameOverRestartButton;
    public Button gameOverExitButton;
    #endregion


    #region Gameplay

    public float rotationTime = 1f;
    public GameObject currentObject;
    float currentYRotation;
    bool canRotate = true;
    public bool inputEnabled = true;
    Tween rotationTween;
    public System.Action OnRotationCompleted;
    public Image playerHappienessMeter;

    public Transform spawnPosition;
    public GameObject orthoCam;
    public GameObject persCam;

    public GameObject platformObject;
    public GameObject happinessLevel;

    public PatientHandler patientHandler;

    bool isOrtho = false;

    public bool isPatientTurn;

    public LevelManager levelManager;


    #endregion

    #region
    public int levelNo;
    int maxLevels;
    public List<GameObject> levelObjects;
    #endregion

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(this.gameObject);
        }

        maxLevels = levelObjects.Count;
        GetCurrentLevel();

        Debug.Log("LEVEL : " + levelNo);
            
        gameplayLeftButton.onClick.RemoveAllListeners();
        gameplayRightButton.onClick.RemoveAllListeners();
        menuPlayButton.onClick.RemoveAllListeners();
        menuCreditsButton.onClick.RemoveAllListeners();
        menuExitButton.onClick.RemoveAllListeners();
        gameplayPauseButton.onClick.RemoveAllListeners();
        gamePlayResumeButton.onClick.RemoveAllListeners();
        gamePlayExitButton.onClick.RemoveAllListeners();
        nextLevelButton.onClick.RemoveAllListeners();
        creditBackButton.onClick.RemoveAllListeners();
        gameOverExitButton.onClick.RemoveAllListeners();
        gameOverRestartButton.onClick.RemoveAllListeners();


        gameplayLeftButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); RotateLeft(); });
        gameplayRightButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); RotateRight(); });
        menuPlayButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnMenuPlayClicked(); });
        menuCreditsButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnMenuCreditsClicked(); });
        menuExitButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnExitClicked(); });
        gameplayPauseButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnGamePlayPauseClicked(); });
        gamePlayResumeButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnGameplayResumeClicked(); });
        gamePlayExitButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnExitClicked(); });
        nextLevelButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); LoadLevel(); });
        creditBackButton.onClick.AddListener(()=> { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); ActivatePanel(EPanel.MenuPanel); });
        gameOverRestartButton.onClick.AddListener(() => 
        { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f);
            DeleteLevelData();
            GetCurrentLevel();
            SaveCurrentLevel();
            UnityEngine.SceneManagement.SceneManager.LoadScene(0); 
        });
        gameOverExitButton.onClick.AddListener(() => { SoundManager.instance.PlayClip(EAudioClip.MENU_SFX, .5f); OnExitClicked(); });

        DisableAllPanels();
        
        if(levelNo > maxLevels)
        {
            ActivatePanel(EPanel.GameCompletedPanel);
        }
        else
        {
            ActivatePanel(EPanel.MenuPanel);
        }
    }


    void DisableAllPanels()
    {
        for (int i = 0; i < 7; i++)
        {
            panels[i].gameObject.SetActive(false);
        }
    }

    void ActivatePanel(EPanel panel)
    {
        if(currentActivePanel != EPanel.None)
        {
            panels[(int)currentActivePanel].gameObject.SetActive(false);
        }
        if(panel != EPanel.None)
        {
            panels[(int)panel].gameObject.SetActive(true);
        }
        currentActivePanel = panel;
    }

    public void LoadLevel()
    {
        SwitchCamera();
        ActivatePanel(EPanel.DialoguePanel);
        levelManager.LoadLevel();
    }

    public void OnDialoguesCompleted()
    {
        SwitchCamera();
        happinessLevel.GetComponent<Image>().fillAmount = 0;
        ActivatePanel(EPanel.GameplayPanel);
        SetCurrentGameObject(levelNo, true);

    }

    

    public void ShowObjectsAnimation()
    {

    }

    public void SwitchCamera()
    {       

        if(!isOrtho)
        {
            orthoCam.SetActive(false);
            persCam.SetActive(true);
        }
        else
        {
            orthoCam.SetActive(true);
            persCam.SetActive(false);
        }
        isOrtho = !isOrtho;
    }

    public void SetCurrentGameObject(int level,bool setActive = true)
    {
        currentObject = Instantiate( levelObjects[level - 1]);
        currentObject.transform.position = spawnPosition.position;
        currentObject.SetActive(setActive);
        currentYRotation = currentObject.transform.rotation.eulerAngles.y;
        PlayerController.instance.control = currentObject.GetComponent<ObjectControl>();
        PlayerController.instance.silhouetteGenerator = currentObject.GetComponent<ItemSilhouetteGenerator>();
    }

    public void RotateLeft()
    {      
        if (currentObject != null)
        {
            inputEnabled = false;
            gameplayLeftButton.interactable = false;
            gameplayRightButton.interactable = false;
            currentYRotation -= 90f;
            Quaternion quaternion = Quaternion.Euler(0, currentYRotation, 0);
            rotationTween =  currentObject.transform.DORotateQuaternion(quaternion, rotationTime).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                gameplayLeftButton.interactable = true;
                gameplayRightButton.interactable = true;
            if (OnRotationCompleted != null)
            {
                    // OnRotationCompleted();
                    PlayerController.instance.silhouetteGenerator.OnObjectRotated();
            }
            });
        }
    }

    public void RotateRight()
    {       
        if (currentObject != null)
        {
            inputEnabled = false;
            gameplayLeftButton.interactable = false;
            gameplayRightButton.interactable = false;
            currentYRotation += 90f;
            Quaternion quaternion = Quaternion.Euler(0, currentYRotation, 0);
            rotationTween = currentObject.transform.DORotateQuaternion(quaternion, rotationTime).SetEase(Ease.InOutSine).OnComplete(()=> 
            {
                gameplayLeftButton.interactable = true;
                gameplayRightButton.interactable = true;
            if(OnRotationCompleted != null)
            {
                    // OnRotationCompleted();
                    PlayerController.instance.silhouetteGenerator.OnObjectRotated();
                }
            });
        }
    }


    Tween camRotTween = null ;
    public bool isRot = false;
    public void RotatePerspective()
    {
        if (!isRot)
        {

            isRot = true;
            float currCamRot = persCam.transform.rotation.eulerAngles.y;
            currCamRot += 180f;

            Quaternion quaternion = Quaternion.Euler(0, currCamRot, 0);
            camRotTween = persCam.transform.DORotateQuaternion(quaternion, 1.1f).SetEase(Ease.InOutSine).OnComplete(() =>
            {
                isRot = false;
            });
        }
    }

    void OnMenuPlayClicked()
    {
        LoadLevel();
    }

    void OnMenuCreditsClicked()
    {
        ActivatePanel(EPanel.CreditsPanel);
    }

    void OnExitClicked()
    {
        Application.Quit();
    }

    void OnGamePlayPauseClicked()
    {
        ActivatePanel(EPanel.PausePanel);
        inputEnabled = false;
    }

    void OnGameplayResumeClicked()
    {
        ActivatePanel(EPanel.GameplayPanel);
        inputEnabled = true;
    }

    public void OnLevelCompleted()
    {
        Destroy(currentObject);
        ActivatePanel(EPanel.LevelCompletedPanel);
        playerHappienessMeter.fillAmount = 0;
        levelNo++;
        if(levelNo > maxLevels)
        {
            ActivatePanel(EPanel.GameCompletedPanel);
        }

        SaveCurrentLevel();
    }

    public void UpdateFill(float value)
    {
        StartCoroutine(UpdateFillCoroutine(value));
    }

    IEnumerator UpdateFillCoroutine(float value, float time = 0.5f)
    {
        float currentTime = 0f;
        float startValue = playerHappienessMeter.fillAmount;

        while (currentTime <= time)
        {
            currentTime += Time.deltaTime;
            playerHappienessMeter.fillAmount = Mathf.Lerp(startValue, value, currentTime / time);
            yield return null;
        }
    }

    public void GetCurrentLevel()
    {
        levelNo = PlayerPrefs.GetInt("Current_Level",1);
    }

    public void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt("Current_Level", levelNo);
    }

    private void OnDestroy()
    {
        SaveCurrentLevel();
    }

    public void DeleteLevelData()
    {
        PlayerPrefs.DeleteAll();
    }
}

public enum EPanel
{
    GameplayPanel = 0,
    GameOverPanel,
    LevelCompletedPanel,
    GameCompletedPanel,
    MenuPanel,
    CreditsPanel,
    PausePanel,
    DialoguePanel,
    None
}
