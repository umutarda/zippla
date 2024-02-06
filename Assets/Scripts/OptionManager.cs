using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PLAYERTWO.PlatformerProject;

public class OptionManager : MonoBehaviour
{
    private Transform[] spawnPoints;
    [SerializeField] private GameObject boxPrefab;

    public static OptionManager Instance;

    private List<GameObject> currentOptionBoxes;
    private void Awake()
    {
       
        // Set spawnPoints using all children of the owner object of this component
        spawnPoints = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            spawnPoints[i] = transform.GetChild(i);
        }

        currentOptionBoxes = new List<GameObject>();
        Instance = this;
    }

    public void CreateOptions()
    {
        // Create a list of all available spawn points
        List<Transform> availableSpawnPoints = new List<Transform>(spawnPoints);

        // Store the answer for this question
        int correctOptionIndex = QuestionManager.Instance.GetCurrentQuestion().GetCorrectOptionIndex();


        // Create 3 box clones from boxPrefab and randomly assign them to available spawn points
        for (int i = 0; i < QuestionManager.Instance.OptionsCount; i++)
        {
            // Choose a random spawn point from the available list and remove it from the list
            int index = Random.Range(0, availableSpawnPoints.Count);
            Transform spawnPoint = availableSpawnPoints[index];
            availableSpawnPoints.RemoveAt(index);


            // Instantiate a new box clone from boxPrefab
            GameObject box = Instantiate(boxPrefab, spawnPoint.position, Quaternion.identity);

            ParticleFXManager.Instance.CreateParticleFX("CreateBox",box.transform,Vector3.back);

            // Set the text of the first child of the box clone to A, B, or C depending on its position in the list
            TMP_Text textComponent = box.transform.GetChild(0).GetChild(0).GetComponent<TMP_Text>();
            textComponent.text = "" + (char) (65+i);

            // Check if this box is the correct answer and set up the OnBreak event if so
            if (i == correctOptionIndex)
            {
                Breakable breakableComponent = box.GetComponent<Breakable>();
                breakableComponent.OnBreak.AddListener(GameManager.Instance.Win);

                // Append "@C" to the box name to indicate that it is the correct answer
                box.name += "@C";
            }

            // Add the box to the list of current option boxes
            currentOptionBoxes.Add(box);
        }
    }


    public void DestroyCurrentOptionBoxes()
    {
        foreach (GameObject box in currentOptionBoxes)
        {
            Destroy(box);
        }
        currentOptionBoxes.Clear();
    }

    public GameObject GetCorrectBox()
    {
        foreach (GameObject box in currentOptionBoxes)
        {
            string[] splitName = box.name.Split('@');
            if (splitName.Length == 2 && splitName[1] == "C")
            {
                return box;
            }
        }
        return null;
    }

}
