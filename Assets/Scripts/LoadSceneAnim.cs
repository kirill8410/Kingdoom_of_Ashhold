using System.Collections;
using UnityEngine;

public class LoadSceneAnim : MonoBehaviour
{
    [SerializeField] float time;
    [SerializeField]GameObject[] sprites;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Switch();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Switch()
    {
        StartCoroutine(Animation());
    }
    int i;
    IEnumerator Animation()
    {
        for(i = 0; i < sprites.Length; i++)
        {
            sprites[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(time);
            print("1");
        }
        for (i = 0; i < sprites.Length; i++)
        {
            sprites[i].gameObject.SetActive(false);
        }
        StopCoroutine(Animation());
    }
}
