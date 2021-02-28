using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffect : MonoBehaviour
{
    public float speed_x;
    public float speed_y;

    public float limit_x;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var temp = transform.localPosition;

        temp.y += Mathf.Sin(Time.time) * speed_y * 0.01f;
        temp.x += speed_x * Time.deltaTime;

        if (temp.x > limit_x){
            transform.localPosition = new Vector3(-1f, 0, 0);
        }
        else{
            transform.localPosition = temp;
        }
    }
}
