using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Others : MonoBehaviour
{
    [SerializeField] Vector3 inScreenPos;
    [SerializeField] Vector3 outScreenPos;

    [SerializeField] float tweenDuration;
    [SerializeField] public string menuName;
    public bool open;

    public void Open()
    {
        open = true;
        GetComponent<RectTransform>().DOAnchorPos3D(inScreenPos, tweenDuration).SetUpdate(true);

    }

    public void Close()
    {
        open = false;
        GetComponent<RectTransform>().DOAnchorPos3D(outScreenPos, tweenDuration).SetUpdate(true);
    }
}
