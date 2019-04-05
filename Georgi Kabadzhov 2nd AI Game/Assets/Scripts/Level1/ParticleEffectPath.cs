using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffectPath : MonoBehaviour
{

    public string pathName;
    public float time;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<ParticleSystem>().Play();
        iTween.MoveTo(gameObject, iTween.Hash("path", iTweenPath.GetPath(pathName), "time", time));        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
