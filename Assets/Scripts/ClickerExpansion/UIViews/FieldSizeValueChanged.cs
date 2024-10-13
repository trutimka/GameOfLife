using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;

public class FieldSizeValueChanged : MonoBehaviour
{
    [SerializeField] private TMP_InputField widthInputField;
    [SerializeField] private TMP_InputField heightInputField;
    
    [SerializeField] private GameObject _gameView;
    [SerializeField] private GameObject _tapToStart;
    
    public GameObject cellParent;

    private GridGenerator gridGenerator; 
    private void Start()
    {
        gridGenerator = cellParent.GetComponent<GridGenerator>();
        widthInputField.text = "25";
        heightInputField.text = "25";
    }
    
    public void SetupGameFieldSize()
    { 
        gridGenerator.SetupFieldSize(widthInputField.text, heightInputField.text);
        
        gameObject.SetActive(false);
        
        _gameView.SetActive(true);
        _tapToStart.SetActive(true);
    }
}
