using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class BuildButtonsAnimation : MonoBehaviour
{
    GameObject[] _icons;
    int angle;
    int corect;
    bool animation = false;

    // Update is called once per frame
    void Update()
    {
        if (_icons.Length > 0)
        {
            angle = 360 / _icons.Length;
            if (animation)
            {
                for (int i = 0; i < _icons.Length; i++)
                {
                    _icons[i].GetComponentInChildren<Button>(includeInactive: true).transform.eulerAngles =
                        new Vector3(0, _icons[i].GetComponentInChildren<Button>(includeInactive: true).transform.eulerAngles.y, -0);
                }
            }
            StartAnim();
        }
        
        // new Vector3(0, 0, (angle * (1) - 120))
    }

    public void SetIcons(GameObject[] icons)
    {
        _icons = icons;
        animation = true;
        switch (_icons.Length)
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

    bool started = false;
    void StartAnim()
    {
        if (GetComponent<Canvas>().enabled == true&& !started)
        {
            started = true;
            if (_icons.Length > 1)
                for(int i = 0; i < _icons.Length; i++)
                {
                    StartCoroutine(RotateIcons(_icons.Length - i));
                }
                
        }
        else if(GetComponent<Canvas>().enabled == false)
        {
            started = false;
            for (int i = 0; i < _icons.Length; i++)
            {
                _icons[i].transform.eulerAngles = new Vector3(_icons[i].transform.eulerAngles.x, _icons[i].transform.eulerAngles.y, 90);
                _icons[i].GetComponentInChildren<Button>().transform.eulerAngles = new Vector3(0, _icons[i].GetComponentInChildren<Button>().transform.eulerAngles.y, -0);
            }
        }

    }
    
    IEnumerator RotateIcons(int num)
    {
        if (_icons.Length > 0)
        {
            for (int i = 0; i < num * 360 / _icons.Length - 90 + corect; i++)
            {
                animation = true;
                _icons[num - 1].transform.Rotate(0, 0, 1);
                yield return new WaitForSeconds(0.005f);
            }
            animation = false;
        }
    }
}
