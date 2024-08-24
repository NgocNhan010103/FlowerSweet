using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetalData : Unit
{
    public PetalDefind PetalDefind;
    public PetalSlotsDefind PetalSlotsDefind;

    public void InitChild(int id, PetalSlot slot)
    {
        PetalDefind.Id = id;
        PetalDefind.Name = color.ToString();
        PetalDefind.IdSlot = slot.id;
        PetalDefind.PetalColor = color;
    }
}
