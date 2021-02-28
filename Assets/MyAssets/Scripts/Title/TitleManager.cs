using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button start_button;

    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(() => {
            SceneManager.LoadScene("EditCircuit");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
