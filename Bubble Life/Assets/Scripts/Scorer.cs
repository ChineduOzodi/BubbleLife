using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class Scorer
{
    internal string name;
    internal int time;

    public Scorer(string _name, int _time)
    {
        name = _name;
        time = _time;
    }
    public Scorer()
    {
        name = "ABC";
        time = 10000;
    }
}
