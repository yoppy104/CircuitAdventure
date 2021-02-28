using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    [SerializeField] Button restart_button;
    [SerializeField] Button return_title_button;
    
    [SerializeField] GameObject clear;
    [SerializeField] GameObject fail;

    [SerializeField] GameObject clear_image;
    [SerializeField] GameObject fail_image;

    // Start is called before the first frame update
    void Start()
    {
        if (Common.SharedData.Instance.is_clear){
            clear.SetActive(true);
            clear_image.SetActive(true);
        }else{
            fail.SetActive(true);
            fail_image.SetActive(true);
        }
        Common.SharedData.Instance.is_clear = false;

        restart_button.onClick.AddListener(() => {
            Debug.Log("restart");
            SceneManager.LoadScene("EditCircuit");
        });

        return_title_button.onClick.AddListener(() => {
            Debug.Log("title");
            SceneManager.LoadScene("Title");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
