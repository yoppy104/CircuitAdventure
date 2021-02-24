using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EditCircuit{
    ///<summary> 回路編集画面のUIを管理する。 </summary>
    public class EditCircuitUI : MonoBehaviour
    {
        public const float DISTANCE_CHIP = 1.6f;

        public readonly static Vector3 CPU_STANDARD_POSITION = new Vector3(-1, 0, 0);

        [SerializeField] Button up_chip_button = null;
        [SerializeField] Button right_chip_button = null;
        [SerializeField] Button left_chip_button = null;
        [SerializeField] Button down_chip_button = null;

        public ChipUIFactory factory {
            get; set;
        } = null;

        // Start is called before the first frame update
        void Start()
        {
            up_chip_button.onClick.AddListener( () => {
                factory.GetObject(EditCircuitManager.NAME_UP_CHIP, transform);
            } );
            right_chip_button.onClick.AddListener( () => {
                factory.GetObject(EditCircuitManager.NAME_RIGHT_CHIP, transform);
            } );
            left_chip_button.onClick.AddListener( () => {
                factory.GetObject(EditCircuitManager.NAME_LEFT_CHIP, transform);
            } );
            down_chip_button.onClick.AddListener( () => {
                factory.GetObject(EditCircuitManager.NAME_DOWN_CHIP, transform);
            } );

            var temp = factory.GetObject(EditCircuitManager.NAME_CPU_CHIP, transform);
            temp.transform.position = CPU_STANDARD_POSITION;
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
