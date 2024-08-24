using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowerUGUI : Unit
{
    [SerializeField] PetalSlot[] slots;

    private void Awake()
    {
        PetalSlot[] childSlots = GetComponentsInChildren<PetalSlot>();
        slots = slots.Concat(childSlots).ToArray();
    }

    private void Start()
    {
        Invoke(nameof(Init), 1);
    }
    private void Init()
    {
        switch (color)
        {
            case Color.White: CreateChild(4); break;
            case Color.Purple: CreateChild(2); break;
            case Color.Red: CreateChild(3); break;
            case Color.Green: CreateChild(1); break;
            case Color.Blue: CreateChild(0); break;
            case Color.Yelow: CreateChild(5); break;
        }
    }

    void CreateChild(int id)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].CreateChild(id);
            slots[i].transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("UI");
        }
    }
}


