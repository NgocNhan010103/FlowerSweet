using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public Color color;
    public Type Type;
    public FlowerState FlowerState;

    public void ChangeTypePetal(Color type)
    {
        this.color = type;
    }

    public void ChangeFlowerState(FlowerState targetState)
    {
        FlowerState = targetState;
    }


}
public enum Color
{
    White,
    Red,
    Yelow,
    Purple,
    Blue,
    Green
}

public enum Type
{
    Pistil,
    Petal
}

public enum FlowerState
{
    Ready,
    Prepare
}
