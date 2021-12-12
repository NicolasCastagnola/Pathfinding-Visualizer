﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class MazeSelector : MonoBehaviour
{
    private TMP_Dropdown dropdown;
    private void Start()
    {
        dropdown = GetComponent<TMP_Dropdown>();
        PopulateList();
    }

    public void OnDropdownValueSelected()
    {
        Maze selectedAlgorithm = (Maze)dropdown.value;

        //GameManager.Instance.SetAlgorimth(selectedAlgorithm);
    }
    private List<string> FormatList()
    {
        string[] types = Enum.GetNames(typeof(Maze));
        List<string> names = new List<string>(types);

        List<string> returnList = new List<string>();

        foreach (var item in names)
        {
            string i = item.Replace("x", " ");

            returnList.Add(i);
        }

        return returnList;
    }

    public void PopulateList()
    {
        dropdown.AddOptions(FormatList());
    }

    public void Block()
    {
        GameManager.Instance.canSelect = false;
    }

    public void UnBlock()
    {
        GameManager.Instance.canSelect = true;
    }
}
