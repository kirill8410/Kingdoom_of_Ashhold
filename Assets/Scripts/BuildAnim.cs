using System.Collections;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class BuildAnim : MonoBehaviour
{
    [SerializeField] GameObject[] doscs;
    private int i;
    [SerializeField] GameObject[] tower;

    void Start()
    {
        StartCoroutine(Anim());
    }

    IEnumerator Anim()
    {
        
        while(i < doscs.Length)
        {
            doscs[i].SetActive(true);
            i++;
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(2f);
        tower[0].SetActive(true);
        tower[1].SetActive(true);
        tower[2].SetActive(true);
        transform.DOMoveY(-10,1);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
