using System.Xml;
using System;
using UnityEngine;

public static class Questions 
{
    public static Question[] Read(string rawXMLText) 
    {
       
        XmlDocument questionsDoc = new XmlDocument();

        questionsDoc.LoadXml(rawXMLText);

        Question[] questions = new Question[questionsDoc.FirstChild.NextSibling.ChildNodes.Count];
        int optionCount = Int32.Parse(questionsDoc.FirstChild.NextSibling.Attributes["optionCount"].Value);

        XmlNode q = questionsDoc.FirstChild.NextSibling.FirstChild; //The first question
        int questionCounter = 0;

        while(q != null) 
        {
            XmlNode descriptionNode = q.FirstChild;
            string description = descriptionNode.InnerText.Trim(); //The first qu of the question
            string type = descriptionNode.Attributes["type"].Value;
            string[] options = new string[optionCount];
            int correctOptionIndex = -1;

            XmlNode aChoice = descriptionNode.NextSibling;
            for (int i=0; i<optionCount; i++) 
            {
                options[i] = aChoice.InnerText.Trim();
                
                if (aChoice.Attributes["value"] != null && aChoice.Attributes["value"].Value == "true")  correctOptionIndex = i;
                   
                aChoice = aChoice.NextSibling;
            }

           
            questions[questionCounter++] = new Question(description, options, correctOptionIndex);
            q = q.NextSibling; //The next question
        }

        return questions;
    }  
}

public class Question 
{
    private string description;
    private string description_type;
    private string[] options;
    private int correctOptionIndex;

    public Question (string _description, string[] _options, int _correctOptionIndex) 
    {
        description = _description;
        options = _options;
        correctOptionIndex = _correctOptionIndex;
    }

    public string GetDescription() 
    {
        return description;
    }

    public string[] GetOptions() 
    {
        return options;
    }

    public int GetCorrectOptionIndex() 
    {
        return correctOptionIndex;
    }

    public int GetOptionCount() 
    {
        return options.Length;
    }

    public void MixChoices() 
    {
        string correctOption = options[correctOptionIndex];
        for (int i=0; i<options.Length; i++) 
	    {
		    int randPlace =  UnityEngine.Random.Range(i,options.Length);
            string temp = options[i];
            options[i] = options[randPlace];
            options[randPlace] = temp;

        }

        for (int i=0; i<options.Length; i++) 
	    {
		    if (options[i] == correctOption) 
            {
                correctOptionIndex = i;
                break;
            }

        }
    }

}
