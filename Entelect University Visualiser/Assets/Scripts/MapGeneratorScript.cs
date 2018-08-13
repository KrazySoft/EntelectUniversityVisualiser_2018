using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGeneratorScript : MonoBehaviour {

    public GameObject mapObject;
    public GameObject groundPrefab;
    public GameObject minePrefab;
    public GameObject depoPrefab;
    public GameObject minerPrefab;
    public GameObject excavatorPrefab;
    public GameObject haulerPrefab;
    public TextAsset[] mapFiles;
    public int mapNumber;

    //Map Details
    public int height;
    public int width;
    public int minerCount;
    public int excavatorCount;
    public int haulerCount;
    public int mineCount;
    public int depoCount;
    public int budget;

	// Use this for initialization
	void Start () {
        if (PlayerPrefs.HasKey("map_num"))
        {
            mapNumber = PlayerPrefs.GetInt("map_num");
        }
        string mapDetails = mapFiles[mapNumber].text;
        string[] mapLines = mapDetails.Split('\n');
        string[] mapSpecs = mapLines[0].Split(' ');
        height = int.Parse(mapSpecs[0]);
        width = int.Parse(mapSpecs[1]);
        minerCount = int.Parse(mapSpecs[2]);
        excavatorCount = int.Parse(mapSpecs[3]);
        haulerCount = int.Parse(mapSpecs[4]);
        mineCount = int.Parse(mapSpecs[5]);
        depoCount = int.Parse(mapSpecs[6]);
        budget = int.Parse(mapSpecs[7]);
        this.GetComponent<MapControllerScript>().budget = budget;
        for(int m = 0; m < minerCount; m++)
        {
            Instantiate(minerPrefab, mapObject.transform);
        }

        for(int e = 0; e < excavatorCount; e++)
        {
            Instantiate(excavatorPrefab, mapObject.transform);
        }

        for(int h = 0; h < haulerCount; h++)
        {
            Instantiate(haulerPrefab, mapObject.transform);
        }

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                GameObject ground = Instantiate(groundPrefab, mapObject.transform);
                ground.transform.position = new Vector3(x, y, 0);
            }
        }

        for(int mn = 0; mn <= mineCount; mn++)
        {
            string mineLine = mapLines[mn];
            string[] mineDetails = mineLine.Split(' ');
            int index = int.Parse(mineDetails[0]);
            string name = "mine_" + index;
            string element = mineDetails[1];
            Vector2 position = new Vector2(int.Parse(mineDetails[2]), int.Parse(mineDetails[3]));
            int resourceCount = int.Parse(mineDetails[4]);
            GameObject mine = Instantiate(minePrefab, mapObject.transform);
            mine.transform.position = position;
            mine.name = name;
            MineScript mineScript = mine.GetComponent<MineScript>();
            mineScript.index = index;
            mineScript.element = element;
            mineScript.resourceCount = resourceCount;

        }

        for(int d = mineCount+1; d <= depoCount+mineCount; d++)
        {
            string depoLine = mapLines[d];
            string[] depoDetails = depoLine.Split(' ');
            int index = int.Parse(depoDetails[0]);
            string name = "depo_" + index;
            string element = depoDetails[1];
            Vector2 position = new Vector2(int.Parse(depoDetails[2]), int.Parse(depoDetails[3]));
            GameObject depo = Instantiate(depoPrefab, mapObject.transform);
            depo.transform.position = position;
            depo.name = name;
            DepoScript depoScript = depo.GetComponent<DepoScript>();
            depoScript.element = element;
            depoScript.index = index;
        }
        Camera.main.transform.position = new Vector3(0, 0, -1);
        Rect bounds = new Rect(new Vector2(-5f, -2.5f), new Vector2(width+10f, height+5f));
        Camera.main.GetComponent<Camera2D>().cameraLimits = bounds;

    }

}
