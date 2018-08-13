using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerScript : MonoBehaviour {
    public enum WorkerType { Miner, Excavator, Hauler }

    public WorkerType type;
    public string Instructions;
    public string Inventory;
    public List<string> InstructionList;
    private float simulationSpeed;
    private GameObject[] mines;
    private GameObject[] depos;
    private GameObject controller;

    private void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Controller");
    }

    public void setInstruction(string instruction)
    {
        Instructions = instruction.Split('|')[1];
    }

    public void StartSolution(float speed)
    {
        simulationSpeed = speed;
        mines = GameObject.FindGameObjectsWithTag("Mine");
        depos = GameObject.FindGameObjectsWithTag("Depo");
        if(Instructions == null || Instructions.Length == 0)
        {
            return;
        }
        InstructionList = new List<string>(Instructions.Split(','));
        StartCoroutine("simulate");
    }

    private IEnumerator simulate()
    {
        while(InstructionList.Count > 0)
        {
            string instruction = InstructionList[0];
            InstructionList.RemoveAt(0);
            GameObject target = null;
            if (controller.GetComponent<MapGeneratorScript>().mineCount > int.Parse(instruction))
            {
                foreach (GameObject mine in mines)
                {
                    if (mine.GetComponent<MineScript>().index == int.Parse(instruction))
                    {
                        target = mine;
                        break;
                    }
                }
            }
            else
            {
                foreach (GameObject depo in depos)
                {
                    if (depo.GetComponent<DepoScript>().index == int.Parse(instruction))
                    {
                        target = depo;
                        break;
                    }
                }
            }
            while(target != null && Vector3.Distance(this.transform.position, target.transform.position) > 0)
            {
                if (this.transform.position.x != target.transform.position.x)
                {
                    Vector3 oldPos = this.transform.position;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, new Vector3(target.transform.position.x, this.transform.position.y), simulationSpeed);
                    float distance = Vector3.Distance(oldPos, this.transform.position);
                    controller.GetComponent<MapControllerScript>().spendBudget(distance);
                }
                else
                {
                    Vector3 oldPos = this.transform.position;
                    this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, simulationSpeed);
                    float distance = Vector3.Distance(oldPos, this.transform.position);
                    controller.GetComponent<MapControllerScript>().spendBudget(distance);
                }
                
                yield return new WaitForFixedUpdate();
            }
            if(target.GetComponent<MineScript>() != null)
            {
                List<string> inventory = new List<string>(Inventory.Split(','));
                if (!inventory.Contains(target.GetComponent<MineScript>().element.ToUpper()))
                {
                    target.GetComponent<MineScript>().mineResource();
                    Inventory += target.GetComponent<MineScript>().element.ToUpper()+",";
                }
            }else if (target.GetComponent<DepoScript>() != null)
            {
                List<string> inventory = new List<string>(Inventory.Split(','));
                if (inventory.Contains(target.GetComponent<DepoScript>().element.ToUpper()))
                {
                    inventory.Remove(target.GetComponent<DepoScript>().element.ToUpper());
                    Inventory = string.Join(",",inventory.ToArray());
                }
            }
        }
        yield return null;
    }
}
