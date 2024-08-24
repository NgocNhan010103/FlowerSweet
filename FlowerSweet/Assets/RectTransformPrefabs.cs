using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectTransformPrefabs : MonoBehaviour
{
    public GameObject prefab; 
public int sortingOrder = 999;
    public void Start()
    {
        GameObject instance = Instantiate(prefab, transform.position, Quaternion.identity);

        instance.transform.SetParent(GameObject.Find("UI").transform, false);
        
        MeshRenderer meshRenderer = instance.GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.sortingOrder = sortingOrder;
        }
    }

    

}
