using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Instructions : MonoBehaviour
{
    public static Instructions instance;
    public Text text;

    int index;
    public List<string> lines;
    bool _block = false;
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
     
    }
    public void next() // calles in another scripts
    {
        if (_block || index == lines.Count)
            return;
        text.text = lines[index++];
    }
    public void block()
    {
        _block = true;
    }
}
