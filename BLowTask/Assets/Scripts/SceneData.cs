using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    public string type;
    public float[] position;
    public float[] rotation;
    public float[] scale;

    public SceneData(GameObject obj)
    {
        position = new float[3];
        rotation = new float[3];
        scale = new float[3];

        type = obj.GetComponent<MeshFilter>().mesh.name;

        position[0] = obj.transform.position.x;
        position[1] = obj.transform.position.y;
        position[2] = obj.transform.position.z;

        rotation[0] = obj.transform.rotation.x;
        rotation[1] = obj.transform.rotation.y;
        rotation[2] = obj.transform.rotation.z;

        scale[0] = obj.transform.localScale.x;
        scale[1] = obj.transform.localScale.y;
        scale[2] = obj.transform.localScale.z;

    }



}
