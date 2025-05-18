using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonsAnimation : MonoBehaviour
{
    [SerializeField] GameObject[] icons;
    int angle;
    int corect;
    public bool animation=false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animation = true;
        switch (icons.Length)
        {
            case 2:
                corect = -90;
                break;
            case 3:
                corect = 0;
                break;
            case 4:
                corect = 45;
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
       
        angle = 360 / icons.Length;
        if (animation)
        {
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].GetComponentInChildren<Button>().transform.eulerAngles = new Vector3(0, icons[i].GetComponentInChildren<Button>().transform.eulerAngles.y, -0);
            }
        }
        StartAnim();
        
        
        // new Vector3(0, 0, (angle * (1) - 120))
    }

    bool started = false;
    void StartAnim()
    {
        if (GetComponent<Canvas>().enabled == true&& !started)
        {
            started = true;
            if (icons.Length > 1)
                for(int i = 0; i < icons.Length; i++)
                {
                    StartCoroutine(RotateIcons(icons.Length - i));
                }
                
        }
        else if(GetComponent<Canvas>().enabled == false)
        {
            started = false;
            for (int i = 0; i < icons.Length; i++)
            {
                icons[i].transform.eulerAngles = new Vector3(icons[i].transform.eulerAngles.x, icons[i].transform.eulerAngles.y, 90);
                icons[i].GetComponentInChildren<Button>().transform.eulerAngles = new Vector3(0, icons[i].GetComponentInChildren<Button>().transform.eulerAngles.y, -0);
            }
        }

    }
    
    IEnumerator RotateIcons(int num)
    {
        
        for (int i = 0; i < num*360/icons.Length-90+corect; i++)
        {
            animation = true;
            icons[num - 1].transform.Rotate(0, 0, 1);
            yield return new WaitForSeconds(0.005f);
        }
        animation = false;
        
        
    }
}
