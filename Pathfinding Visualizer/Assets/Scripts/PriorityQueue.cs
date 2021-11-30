﻿using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue<T>
{
    Dictionary<T, int> _allNodes = new Dictionary<T, int>();

    public void Put(T node, int cost)
    {
        if (!_allNodes.ContainsKey(node)) _allNodes.Add(node, cost);

        else _allNodes.Add(node, cost);
    }
    public int Count()
    {
        return _allNodes.Count;
    }
    public List<T> ReturnDictionaryToList()
    {
        List<T> l = new List<T>();

        l = _allNodes.Keys.ToList();

        return l;
    }

    public void Clear()
    {
        _allNodes.Clear();
    }

    public T Get()
    {
        if (Count() == 0) return default;

        T n = default;

        foreach (var item in _allNodes)
        {
            if (n == null) n = item.Key;
            if (item.Value < _allNodes[n]) n = item.Key;

        }

        _allNodes.Remove(n);

        return n;
    }
}
