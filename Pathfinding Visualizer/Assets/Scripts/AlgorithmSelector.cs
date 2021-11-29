using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AlgorithmSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        PopulateList();
    }

    public void OnDropdownValueSelected()
    {
        Algorithm selectedAlgorithm = (Algorithm)dropdown.value;
        GameManager.Instance.SetAlgorimth(selectedAlgorithm);
    }
    private void PopulateList()
    {
        string[] types = Enum.GetNames(typeof(Algorithm));
        List<string> names = new List<string>(types);
        dropdown.AddOptions(names);
    }
}
