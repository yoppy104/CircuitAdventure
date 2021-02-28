using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnalyzeBarEffect : MonoBehaviour
{
    public float limit_y;
    public float min_y;
    public float delta_y;

    void Start(){
        
    }


    // Update is called once per frame
    void Update()
    {
        transform.localPosition += new Vector3(0, 1f, 0) * Time.deltaTime * delta_y;

        if (transform.localPosition.y > limit_y){
            transform.localPosition = new Vector3(0, min_y, 0);
        }
    }
}
