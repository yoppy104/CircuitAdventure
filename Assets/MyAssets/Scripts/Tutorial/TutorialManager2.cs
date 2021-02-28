using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager2 : MonoBehaviour
{
    [SerializeField] private GameObject[] displays;
    [SerializeField] private Button next_button;
    [SerializeField] private Button prev_button;
    [SerializeField] private Text next_button_text;

    int now_index = 0;

    // Start is called before the first frame update
    void Start()
    {
        next_button.onClick.AddListener(() => {
            if (displays.Length - 1 == now_index){
                SceneManager.LoadScene("EditCircuit");
            }
            displays[now_index].SetActive(false);
            now_index += 1;
            displays[now_index].SetActive(true);

            if (now_index == displays.Length - 1){
                next_button_text.text = "終了";
            }
        });
        prev_button.onClick.AddListener(() => {
            if (now_index == 0) return;
            
            displays[now_index].SetActive(false);
            now_index -= 1;
            displays[now_index].SetActive(true);

            if (now_index == displays.Length - 2){
                next_button_text.text = "次へ";
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
