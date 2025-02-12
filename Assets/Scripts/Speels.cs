using UnityEditorInternal;
using UnityEngine;
using DG.Tweening;

public class Speels : MonoBehaviour
{
    
    void Update()
    {
        Rotate(); 
    }

    private void Rotate()
    {
        transform.eulerAngles = new Vector3(transform.eulerAngles.x, Camera.main.transform.eulerAngles.y, transform.eulerAngles.z);
    }
}
