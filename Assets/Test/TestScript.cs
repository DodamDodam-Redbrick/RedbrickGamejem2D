using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class a
{
    public a(int v)
    {
        value = v;
    }
    public int value;
}

public class TestScript : MonoBehaviour
{
    Dictionary<int, a> dic= new Dictionary<int, a>();
    
    // Start is called before the first frame update
    void Start()
    {
        a aaa = new a(1);

        dic[0] = aaa;

        a bbb = dic[0];
        bbb.value = 2;

        Debug.Log($"aaa.value = {aaa.value}, bbb.value = {bbb.value}");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
