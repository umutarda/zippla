using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class QuestionManager : MonoBehaviour
{
    [SerializeField] private TMP_Text description;
    [SerializeField] private TMP_Text optionsText;
    [SerializeField] private TextAsset questionsAsset;

    private Question[] questions;
    private int currentIndex;

    public static QuestionManager Instance;

    public int OptionsCount => optionsCount;
    private int optionsCount;
    private void Awake()
    {
        questions = Questions.Read(questionsAsset.text);
        Instance = this;

        for (int i=0; i<questions.Length; i++) 
	    {
		    int randPlace =  UnityEngine.Random.Range(i,questions.Length);
            
            Question temp = questions[i];
            questions[i] = questions[randPlace];
            questions[randPlace] = temp;

            questions[i].MixChoices();

        }
    }

    private void LoadQuestion(Question question, bool isDebug)
    {
        if (question == null) 
        {
             description.text = "";
             optionsText.text = "";
             return;
        }
        description.text = question.GetDescription();
        string[] options = question.GetOptions();
        
        optionsText.text = "";
        optionsCount = options.Length;

        for (int i=0; i<options.Length; i++) 
        {
            if (isDebug && i == question.GetCorrectOptionIndex()) 
            {
                optionsText.text += "<color=\"green\">";
                optionsText.text += (char)(65+i) + ") " + options[i]+"\n";
                optionsText.text += "</color>";
            }
            

            else 
            {
                optionsText.text += (char)(65+i) + ") " + options[i]+"\n";
            }
        }
    }
    
    public void MoveNextQuestion(bool isDebug=false)
    {
        if (currentIndex < questions.Length)
        {
            LoadQuestion(questions[currentIndex],isDebug);
            currentIndex++;
            //return questions[currentIndex-1];
        }
        else
        {
            SceneManager.LoadScene("Menu");
            Debug.LogWarning("All questions have been consumed.");
            //return null;
        }

        
    }

    public Question GetCurrentQuestion()
    {
        if (currentIndex > 0 && currentIndex <= questions.Length)
        {
            return questions[currentIndex - 1];
        }
        else
        {
            Debug.LogWarning("No current question available.");
            return null;
        }
    }
}
