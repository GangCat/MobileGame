using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class constructor : MonoBehaviour
{
    private ClassA classA = new ClassA();

    private void Awake()
    {
        Debug.Log("Awake");
        //classA = null;
    }
}

public class ClassA
{
    static int count = 0;
    int cou = 0;
    public ClassA()
    {
        count++;
        cou = count;
        Debug.Log("생성 : " + cou);
    }

    ~ClassA()
    {
        Debug.Log("해제: " + cou);
    }
}