using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Crosstales.FB;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class MapControllerScript : MonoBehaviour {

    public Transform MapTransform;
    public Text solutionText;
    public Text statusText;
    public Text budgetText;
    [HideInInspector]
    public string solutionPath;
    [HideInInspector]
    public float budget;
    private GameObject[] mines;
    private GameObject[] depos;
    private bool complete = false;

    public void selectSolution()
    {
        mines = GameObject.FindGameObjectsWithTag("Mine");
        depos = GameObject.FindGameObjectsWithTag("Depo");
        budget = this.gameObject.GetComponent<MapGeneratorScript>().budget;
        solutionPath = FileBrowser.OpenSingleFile("Open Solution", "", "");
        string[] parts = solutionPath.Split(Path.DirectorySeparatorChar);
        if(parts.Length == 1)
        {
            parts = solutionPath.Split(Path.AltDirectorySeparatorChar);
        }
        solutionText.text = parts[parts.Length-1];
        statusText.text = "Ready";
        assignSolution();
    }

    void assignSolution()
    {
        List<GameObject> miners = new List<GameObject>(GameObject.FindGameObjectsWithTag("Miner"));
        List<GameObject> excavators = new List<GameObject>(GameObject.FindGameObjectsWithTag("Excavator"));
        List<GameObject> haulers = new List<GameObject>(GameObject.FindGameObjectsWithTag("Hauler"));
        using (StreamReader sr = new StreamReader(solutionPath))
        {
            while(sr.Peek() >= 0)
            {
                string instruction = sr.ReadLine();
                switch (instruction[0])
                {
                    case 'M':
                        if (miners.Count == 0)
                            break;
                        GameObject miner = miners[0];
                        miners.Remove(miner);
                        miner.GetComponent<WorkerScript>().setInstruction(instruction);
                        break;
                    case 'E':
                        if (excavators.Count == 0)
                            break;
                        GameObject excavator = excavators[0];
                        excavators.Remove(excavator);
                        excavator.GetComponent<WorkerScript>().setInstruction(instruction);
                        break;
                    case 'H':
                        if (haulers.Count == 0)
                            break;
                        GameObject hauler = haulers[0];
                        haulers.Remove(hauler);
                        hauler.GetComponent<WorkerScript>().setInstruction(instruction);
                        break;
                }
            }
        }
    }

    public void startSolution(float speed)
    {
        statusText.text = "Running";
        MapTransform.BroadcastMessage("StartSolution", speed);
    }

    public void backToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void spendBudget(float cost)
    {
        budget -= cost;
    }

    private void Update()
    {
        budgetText.text = "Budget: " + budget;
        if (!complete)
        {
            complete = true;
            foreach (GameObject mine in mines)
            {
                if (mine.GetComponent<MineScript>().resourceCount > 0)
                {
                    complete = false;
                }
            }
        }
        else
        {

        }
        
    }
}
