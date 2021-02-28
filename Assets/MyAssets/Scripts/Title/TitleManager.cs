using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField] private Button start_button;

    [SerializeField] private GameObject check_tutorial;
    [SerializeField] private Button show_button;
    [SerializeField] private Button skip_button;

    // Start is called before the first frame update
    void Start()
    {
        start_button.onClick.AddListener(() => {
            check_tutorial.SetActive(true);
        });

        show_button.onClick.AddListener(() => {
            SceneManager.LoadScene("Tutorial2");
        });

        skip_button.onClick.AddListener(() => {
            SceneManager.LoadScene("EditCircuit");
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
#endif
        }
    }
}
