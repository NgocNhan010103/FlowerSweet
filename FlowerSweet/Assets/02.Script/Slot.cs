using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Diagnostics;

public class Slot : MonoBehaviour
{   
    public int slotId;
    public Flower currentUnit;
    public SlotState slotState = SlotState.Empty;
    public SlotType slotType;
    public void CreateUnit(int id)
    {
        GameObject bloom = UnitUtils.GetUnitById(id);

        if (bloom != null)
        {
            GameObject prefab = Instantiate(
                bloom,transform.position, bloom.transform.rotation);
            prefab.transform.SetParent(this.transform);
            prefab.transform.localPosition = new Vector3(0, 1.37f, 0);
            
            currentUnit = prefab.GetComponent<Flower>();
            if (currentUnit != null)
            {
                currentUnit.InitFLowerData(slotId,id,FlowerState.Prepare);
            }

            ChangeStateTo(SlotState.Full);
        }
    }

    public void ChangeStateTo(SlotState targetState)
    {
        slotState = targetState;
    }

    public void ChangeTypeTo(SlotType targetType)
    {
        slotType = targetType;
    }

    public void ItemPlaced()
    {
        currentUnit = null;
        ChangeStateTo(SlotState.Empty);
    }
}

public enum SlotState
{
    Empty,
    Full
}

public enum SlotType
{
    Prepare,
    Playable,
    StartPlayable
}
