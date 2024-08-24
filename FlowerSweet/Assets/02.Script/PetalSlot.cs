using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalSlot : MonoBehaviour
{
    public int id;
    public PetalData currentObject;
    public State slotState = State.Empty;
    public Color typePetal;

    public void CreateChild(int id)
    {
        GameObject childPrefab = UnitUtils.GetChildById(id);
        if (childPrefab != null)
        {
            GameObject prefab = Instantiate(
                childPrefab, transform.position,transform.rotation);
            prefab.transform.SetParent(this.transform);
            prefab.transform.localPosition = Vector3.zero;
            prefab.transform.localRotation = childPrefab.transform.rotation;
            prefab.transform.localScale = childPrefab.transform.localScale;

            currentObject = prefab.GetComponent<PetalData>();
            if (currentObject != null)
            {
                currentObject.InitChild(id, this);
            }

            ChangeStateTo(State.Full);
        }
    }

    void ChangeStateTo(State targetState)
    {
        slotState = targetState;
    }

}   



public enum State
{
    Empty,
    Full
}
