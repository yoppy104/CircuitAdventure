using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChipUI : MonoBehaviour
{
    private readonly Vector2 SCALE = new Vector2(0.7f, 0.7f);

    private Dictionary<string, GameObject> connecter_image = new Dictionary<string, GameObject>();

    ///<summary> 四隅の座標を管理 </summary>
    public class Corner{
        public Vector2 top_L = Vector2.zero;
        public Vector2 top_R = Vector2.zero;
        public Vector2 bottom_L = Vector2.zero;
        public Vector2 bottom_R = Vector2.zero;

        public Corner(){}

        public void Update(Vector3 pos, Vector2 scale){
            Vector2 temp = new Vector2(pos.x, pos.y);

            top_R = temp + scale;
            bottom_L = temp - scale;

            scale.x *= -1;
            top_L = temp + scale;

            scale.x *= -1;
            scale.y *= -1;
            bottom_R = temp - scale;
        }
    }

    public Corner corner = new Corner();

    // Start is called before the first frame update
    void Start()
    {
        string[] names = {"down", "left", "right", "up"};
        int ind = 0;
        foreach (Transform child in transform){
            Debug.Log(child.gameObject);
            connecter_image.Add(names[ind++], child.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        SetCornerPosition();
    }

    ///<summary> 座標を更新する。 </summary>
    public void SetPosition(float x, float y){
        transform.position = new Vector3(x, y, 0f);
        SetCornerPosition();
    }

    ///<summary> 四隅の座標を更新する。 </summary>
    public void SetCornerPosition(){
        corner.Update(transform.position, SCALE);
    }

    ///<summary> 指定座標上に乗っているか </summary>
    public bool CheckOnPoint(float x, float y){
        Vector2 top = corner.top_R;
        Vector2 bottom = corner.bottom_L;
        return (bottom.x <= x && x <= top.x) && (bottom.y <= y && y <= top.y);
    }

    ///<summary> 指定座標上に乗っているか </summary>
    public bool CheckOnPoint(Vector2 pos){
        return CheckOnPoint(pos.x, pos.y);
    }

    ///<summary> 他のChipUIと接触しているかどうか </summary>
    public bool CheckHit(ChipUI chip){
        Vector2[] check = {
            chip.corner.top_R,
            chip.corner.top_L,
            chip.corner.bottom_R,
            chip.corner.bottom_L
        };

        foreach (var pos in check){
            if (CheckOnPoint(pos)) return true;
        }

        return false;
    }

    void OnTriggerStay2D(Collider2D collision){
        // Debug.Log("hit");
    }
}
