using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonsAnimation : MonoBehaviour
{
    [SerializeField] GameObject[] icons;
    int angle;
    bool animation=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        angle=360/icons.Length;
        StartCoroutine(RotateIcons(0));
    }

    // Update is called once per frame
    void Update()
    {
        if (animation)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].GetComponentInChildren<Button>().transform.eulerAngles = new Vector3(0, icons[i].GetComponentInChildren<Button>().transform.eulerAngles.y, -0);
            }
        }
        
        
        
        // new Vector3(0, 0, (angle * (1) - 120))
    }
    
    IEnumerator RotateIcons(int num)
    {
        animation = true;
        for (int i = 0;i <360/icons.Length*(num+1);i++) { }
        if (num < icons.Length-1)
        {
            num++;
            StartCoroutine(RotateIcons(num));
        }
        else
        {
            animation = false;
        }
    }
}
