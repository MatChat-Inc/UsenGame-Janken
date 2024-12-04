using Luna;
using Luna.UI;
using Sirenix.Utilities;
using Spine;
using Spine.Unity;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.EventSystems;
using Event = Spine.Event;

namespace USEN.Games.Janken
{
    
    public class JankenCharacterList : FixedListView<JankenCharacterListCell, JankenCharacter>, IEventSystemHandler
    {
        public JankenCharacters jankenCharacters;
        public SkeletonMecanim skeletonMecanim;
        public Animator animator;
        
        private RectTransform RectTransform => (RectTransform)　transform;

        protected override void Awake()
        {
            base.Awake();
            
            if (!jankenCharacters.characters.IsNullOrEmpty()) 
                Data = jankenCharacters.characters;
        }
        
        // protected void Start()
        // {
        //     var content = _scrollRect.content;
        //     var children = content.GetComponentsInChildren<JankenCharacterListCell>();
        //
        //     if (children.Length > 0)
        //     {
        //         var firstCell = children[0];
        //         firstCell.Focus();
        //         
        //         foreach (var cell in children)
        //         {
        //             cell.OnCellClicked += OnCellClickOrSubmit;
        //             cell.OnCellSubmitted += OnCellClickOrSubmit;
        //         }
        //     }
        // }


        private void OnCellClickOrSubmit(int index, FixedListViewCell<JankenCharacter> listViewCell)
        {
            var categoryCell = (JankenCharacterListCell)listViewCell;
        }
        
        protected override void OnCellDeselected(int index, JankenCharacterListCell listViewCell)
        {
            listViewCell.text.color = Color.white;
        }

        protected override void OnCellSelected(int index, JankenCharacterListCell listViewCell)
        {
            listViewCell.text.color = Color.black;
            skeletonMecanim.skeletonDataAsset = listViewCell.Data.skeleton;
            skeletonMecanim.Initialize(true);
            animator.runtimeAnimatorController = listViewCell.Data.animator;
        }
    }

}
