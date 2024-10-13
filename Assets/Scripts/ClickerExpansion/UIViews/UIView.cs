using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public abstract class UIView : MonoBehaviour
{
    protected TMP_Text _text;

    protected virtual void Start()
    {
        _text = GetComponent<TMP_Text>();
    }

    protected virtual void UpdateView()
    {
        
    }
}
