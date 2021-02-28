using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyInput;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum Step{
        BEGINE,
        MAP,
        EXPLAIN_START,
        EXPLAIN_GOAL,
        EXPLAIN_COLOR_SYMBOL,
        EXPLAIN_SOUND_SYMBOL,
        EXPLAIN_CPU,
        CHIP_DRAG,
        COLOR_CHANGE,
        CHIP_REMOVE,
        EXPLAIN_MOVE,
        EXPLAIN_BOOL,
        EXPLAIN_GAIN,
        EXPAIN_GAME_START,
        FINISH
    }

    Step step = Step.BEGINE;

    [SerializeField] private GameObject begin_canvas;
    [SerializeField] private GameObject map_canvas;
    [SerializeField] private GameObject explain_start_canvas;
    [SerializeField] private GameObject explain_goal_canvas;
    [SerializeField] private GameObject explain_color_canvas;
    [SerializeField] private GameObject explain_sound_canvas;
    [SerializeField] private GameObject chip_drag_canvas;
    [SerializeField] private GameObject color_change_canvas;
    [SerializeField] private GameObject chip_remove_canvas;
    [SerializeField] private GameObject explain_move_canvas;
    [SerializeField] private GameObject exlpain_bool_canvas;
    [SerializeField] private GameObject explain_gain_canvas;
    [SerializeField] private GameObject explain_game_start;
    [SerializeField] private GameObject finish_canvas;

    [SerializeField] private EditCircuit.EditCircuitManager manager;
    [SerializeField] private EditCircuit.EditCircuitUI ui;

    // Start is called before the first frame update
    void Start()
    {
        ui.TutorialMode = true;
    }

    void Begin(){
        if (InputManager.CheckMouseLeftDown().isTouch){
            step = Step.MAP;
            begin_canvas.SetActive(false);
            map_canvas.SetActive(true);

            map_canvas.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => {
                ui.DisplayMap(true);
                finish_map = true;
            });
        }
    }

    bool finish_map = false;
    void Map(){
        if (finish_map){
            map_canvas.SetActive(false);
            explain_start_canvas.SetActive(true);

            step = Step.EXPLAIN_START;
        }
    }

    void ExplainStart(){
        if (InputManager.CheckMouseLeftDown().isTouch){
            step = Step.EXPLAIN_GOAL;
            explain_start_canvas.SetActive(false);
            explain_goal_canvas.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch(step){
            case Step.BEGINE:
                Begin();
                break;
            case Step.MAP:
                Map();
                break;
            case Step.EXPLAIN_START:
                ExplainStart();
                break;
            case Step.EXPLAIN_GOAL:
                break;
            case Step.EXPLAIN_COLOR_SYMBOL:
                break;
            case Step.EXPLAIN_SOUND_SYMBOL:
                break;
            case Step.EXPLAIN_CPU:
                break;
            case Step.CHIP_DRAG:
                break;
            case Step.CHIP_REMOVE:
                break;
            case Step.EXPLAIN_MOVE:
                break;
            case Step.EXPLAIN_BOOL:
                break;
            case Step.EXPLAIN_GAIN:
                break;
            case Step.EXPAIN_GAME_START:
                break;
            case Step.FINISH:
                break;
        }
    }
}
