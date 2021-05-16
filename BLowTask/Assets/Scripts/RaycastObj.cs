using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastObj : MonoBehaviour
{

    Ray myRay;      
    RaycastHit hit; 
    //public GameObject cube;
    bool spawnCube = false;
    bool spawnSphere = false;

    bool editMode = false;
    bool editRotate = false;

    public Vector3 CubeSize;
    public Vector3 SphereSize;
    GameObject current;
    GameObject prev;
    GameObject next;
    [SerializeField]
    GameObject[] clones;
    GameObject[] newClones;
    GameObject clone1;

    bool autoSave;

    public void Start()
    {
        loadData();
        autoSave = true;
        StartCoroutine(Save());
    }
    void Update()
    {
        myRay = Camera.main.ScreenPointToRay(Input.mousePosition); 

        if (Physics.Raycast(myRay, out hit, 1000f))
        {

            if (Input.GetKeyDown(KeyCode.R)) editRotation();

            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("plane")) spawnObjects();
                if (hit.collider.CompareTag("clickable")) editMove();
                //Debug.Log(editRotate);
            }
        }
        if (Input.GetKeyDown(KeyCode.X)) clearScreen();


    }


    void clearScreen()
    {
        //Destroy(GameObject.FindWithTag("clickable"));
        clones = GameObject.FindGameObjectsWithTag("clickable");
        foreach (var clone in clones)
        {
            Destroy(clone);
            /*
            Debug.Log(clone.gameObject.transform.position.ToString());
            Debug.Log(clone.gameObject.transform.rotation.ToString());
            Debug.Log(clone.gameObject.transform.localScale.ToString());
            */
        }
        //string type = clones[0].gameObject.GetComponent<MeshFilter>().mesh.name;
        //Debug.Log(type);


    }
    void editRotation()
    {
        editRotate = !editRotate;
        //Debug.Log(editRotate);
        if (editRotate)
        {
            next.GetComponent<Renderer>().material.color = Color.blue;
            next.GetComponent<Drag>().enabled = false;
            next.GetComponent<Rotate>().enabled = true;
        }
        else
        {
            next.GetComponent<Renderer>().material.color = Color.red;
            next.GetComponent<Drag>().enabled = true;
            next.GetComponent<Rotate>().enabled = false;
        }
    }
    void editMove()
    {
        editMode = true;
        next = hit.collider.gameObject;
        if (next != current)
        {
            editRotate = false;
            prev = current;
            current = next;

            next.GetComponent<Renderer>().material.color = Color.red;
            next.GetComponent<Drag>().enabled = true;

            prev.GetComponent<Renderer>().material.color = Color.white;
            prev.GetComponent<Drag>().enabled = false;
            prev.GetComponent<Rotate>().enabled = false;




        }
    }
    void spawnObjects()
    {
        if (!editMode)
        {
            if (spawnCube)
            {
                //Instantiate(CreateCube(CubeSize), hit.point, Quaternion.identity);
                Instantiate(CreateCube(CubeSize), new Vector3(hit.point.x, hit.point.y + CubeSize.y / 2f, hit.point.z), Quaternion.identity);
            }
            if (spawnSphere)
            {
                //Instantiate(CreateSphere(SphereSize), hit.point, Quaternion.identity);
                Instantiate(CreateSphere(SphereSize), new Vector3(hit.point.x, hit.point.y + SphereSize.y / 2f, hit.point.z), Quaternion.identity);
            }
            //print(hit.point);
        }
    }

    GameObject CreateCube(Vector3 size)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.localScale = size;
        cube.gameObject.tag = "clickable";
        cube.AddComponent<Drag>();
        cube.AddComponent<Rotate>();
        Destroy(cube);
        return cube;

    }

    GameObject CreateSphere(Vector3 size)
    {
        GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = size;
        sphere.gameObject.tag = "clickable";
        sphere.AddComponent<Drag>();
        sphere.AddComponent<Rotate>();
        Destroy(sphere);
        return sphere;

    }

    public void enableCube()
    {
        editMode = false;
        spawnCube = true;
        spawnSphere = false;
    }

    public void enableSphere()
    {
        editMode = false;
        spawnSphere = true;
        spawnCube = false;
    }

    IEnumerator Save()
    {
        while(true)
        {
            yield return new WaitForSeconds(10f);
            serializeData();

            print("Saved "+ clones.Length +" objects");
            
        }

        
    }
    void serializeData()
    {
        clones = GameObject.FindGameObjectsWithTag("clickable");
        //clone1 = clones[0];


        //SceneData newData = new SceneData(clones[0]);
        SceneData[] newData = new SceneData[clones.Length];
        for (int i = 0; i < clones.Length; i++)
        {
            newData[i] = new SceneData(clones[i]);
            //string json = JsonUtility.ToJson(newData[i]);
            
            
            //Debug.Log(json);
        }
        string jsonS = JsonHelper.ToJson(newData, true);
        PlayerPrefs.SetString("SceneData", jsonS);
        //Debug.Log(jsonS);

    }


    void loadData()
    {
        string jsonL = PlayerPrefs.GetString("SceneData");
        SceneData[] newData = JsonHelper.FromJson<SceneData>(jsonL);
        //Debug.Log(newData.Length);
        Vector3 position;
        Quaternion rotation;
        Vector3 scale;
        for (int i = 0; i < newData.Length; i++)
        {
            position = new Vector3(newData[i].position[0], newData[i].position[1], newData[i].position[2]);
            rotation = new Quaternion(newData[i].rotation[0], newData[i].rotation[1], newData[i].rotation[2], 1.0f);
            scale = new Vector3(newData[i].scale[0], newData[i].scale[1], newData[i].scale[2]);

            if(newData[i].type == "Cube Instance") Instantiate(CreateCube(scale), position, rotation);
            else Instantiate(CreateSphere(scale), position, rotation);

        }

    }


    void spawnSaveData()
    {
        //newClones[0].transform
    }



}