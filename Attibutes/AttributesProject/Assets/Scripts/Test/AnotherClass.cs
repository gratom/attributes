using System.Collections;
using System.Collections.Generic;
using Tools;
using UnityEngine;

[Assert]
public class AnotherClass : MonoBehaviour
{
    [SerializeField] private Animation anim;

    // Update is called once per frame
    private void Update()
    {
    }

    public void test1()
    {
        anim.Stop();
    }
}