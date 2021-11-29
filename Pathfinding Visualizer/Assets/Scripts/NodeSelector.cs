using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class NodeSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        PopulateList();
    }

    public void OnDropdownValueSelected()
    {
        NodesType nodesType = (NodesType)dropdown.value;
        GameManager.Instance.SetNode(nodesType);
    }
    private void PopulateList()
    {
        string[] types = Enum.GetNames(typeof(NodesType));
        List<string> names = new List<string>(types);
        dropdown.AddOptions(names);
    }
}
