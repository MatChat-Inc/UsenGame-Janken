using Cysharp.Threading.Tasks;
using Luna.Extensions.Unity;
using Luna.UI.Navigation;
using Spine.Unity;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace USEN.Games.Janken
{
    public class JankenCharacterListCell : FixedListViewCell<JankenCharacter>
    {
        public Image avatar;
        public TextMeshProUGUI text;

        public override JankenCharacter Data
        {
            get => data;
            set
            {
                data = value;
                text.text = value.name;
                if (avatar != null)
                    avatar.sprite = value.avatar;
            }
        }

        protected override void Start()
        {
            OnCellSelected += OnSelected;
            OnCellDeselected += OnDeselected;
        }
        
        private void OnSelected(int arg1, FixedListViewCell<JankenCharacter> arg2)
        {
            text.color = Color.HSVToRGB(148f / 360, 0.9f, 0.6f);
        }
    
        private void OnDeselected(int arg1, FixedListViewCell<JankenCharacter> arg2)
        {
            text.color = Color.black;
        }

        // protected async void OnValidate()
        // {
        //     await UniTask.DelayFrame(5);
        //
        //     if (ringCollider != null)
        //     {
        //         // Cast a horizontal ray to ring collider to get collision point
        //         var ray = new Ray( new Vector3(ringCollider.transform.position.x + 1000f, background.transform.position.y, 0), Vector3.left);
        //         var hit = Physics2D.Raycast(ray.origin, ray.direction, 1000f, LayerMask.GetMask("Editor"));
        //         // Debug.Log(hit.point);
        //     
        //         if (hit.collider != null)
        //         {
        //             background.transform.SetX(hit.point.x);
        //         }
        //     }
        // }
    
    
    }
}


