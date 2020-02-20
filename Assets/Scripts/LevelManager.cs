using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    //public int levelNo;
    public GameObject button;
     List<GameObject> questionButtons= new List<GameObject>();
    //public List<Text> questionTexts;
    public Text answerText;
    public GameObject QuestionPanel;
    public List<Level> levels = new List<Level>();

    private void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            if(instance!=null)
            {
                Destroy(this);
            }
        }
    }

    private void OnEnable()
    {
        
    }
    
    public void LoadLevel()
    {
        answerText.gameObject.SetActive(false);
        for (int i = 0; i < levels[GameManager.instance.levelNo - 1].questions.Count; ++i)
        {
            GameObject quesButton = Instantiate(button, QuestionPanel.transform) as GameObject;
            quesButton.transform.GetChild(0).GetComponent<Text>().text = levels[GameManager.instance.levelNo - 1].questions[i];
            QuestionNumber questionNumberScript = quesButton.GetComponent<QuestionNumber>();
            questionNumberScript.questionNo = i;
            quesButton.GetComponent<Button>().onClick.AddListener(() => { QuestionNo(questionNumberScript.questionNo); });
            questionButtons.Add(quesButton);

        }
        GameManager.instance.patientHandler.patient.OnNewLevelStarted();
        isEnded = false;
    }

    public void QuestionNo(int no)
    {
        //print(no);

        if(GameManager.instance.isRot == false)
        {
            GameManager.instance.RotatePerspective();

            Destroy(questionButtons[no]);
            QuestionPanel.SetActive(false);
            answerText.text = levels[GameManager.instance.levelNo - 1].answers[no];
            answerText.gameObject.SetActive(true);
        }
        
    }

    bool isEnded = false;
    public void OnAnswerclicked()
    {
        if(GameManager.instance.isRot == false)
        {
            if (!isEnded)
            {
                GameManager.instance.RotatePerspective();
                answerText.gameObject.SetActive(false);

                QuestionPanel.SetActive(true);
            }
            if (QuestionPanel.transform.childCount == 0 && !isEnded)
            {
                //answerText.text = "start puzzle";
                answerText.gameObject.SetActive(true);
                answerText.text = "I've something for you which might help you to get better, but i need to fix it first";
                //GameManager.instance.OnDialoguesCompleted();
                GameManager.instance.patientHandler.CreateLevelParts();
                isEnded = true;
                questionButtons.Clear();
            }
        }

    }
}
[System.Serializable]
public class Level
{
    public List<string> questions= new List<string>();
    public List<string> answers = new List<string>();
}
