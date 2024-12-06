using Luna.UI;
using Luna.UI.Navigation;
using Sirenix.Utilities;
using Spine.Unity;
using UnityEngine;
using UnityEngine.EventSystems;

namespace USEN.Games.Janken
{
    
    public class JankenCharacterList : FixedListView<JankenCharacterListCell, JankenCharacter>, IEventSystemHandler
    {
        // public JankenCharacters jankenCharacters;
        public SkeletonMecanim skeletonMecanim;
        public Animator animator;
        
        private RectTransform RectTransform => (RectTransform)　transform;

        protected override void Awake()
        {
            base.Awake();
            
            var characters = JankenCharacters.Default?.characters;
            if (!characters.IsNullOrEmpty()) 
                Data = characters;
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
            JankenPreferences.SelectedCharacter = SelectedIndex;
            Navigator.Pop();
        }

        protected override void OnCellClicked(int index, JankenCharacterListCell listViewCell)
        {
            OnCellClickOrSubmit(index, listViewCell);
        }
        
        protected override void OnCellSubmitted(int index, JankenCharacterListCell listViewCell)
        {
            OnCellClickOrSubmit(index, listViewCell);
        }

        protected override void OnCellDeselected(int index, JankenCharacterListCell listViewCell)
        {
            listViewCell.nameText.color = Color.white;
        }

        protected override void OnCellSelected(int index, JankenCharacterListCell listViewCell)
        {
            listViewCell.nameText.color = Color.black;
            animator.runtimeAnimatorController = listViewCell.Data.animator;
            skeletonMecanim.skeletonDataAsset = listViewCell.Data.skeleton;
            skeletonMecanim.Initialize(true);
        }
    }

}
