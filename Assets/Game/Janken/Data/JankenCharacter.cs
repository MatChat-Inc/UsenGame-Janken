
using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEngine;

namespace USEN.Games.Janken
{
    [Serializable]
    public class JankenCharacter
    {
        [TableColumnWidth(60, Resizable = false)]
        [PreviewField(Alignment = ObjectFieldAlignment.Center)] 
        public Sprite avatar;
        [TableColumnWidth(150, Resizable = true)]
        [VerticalGroup("Data"), LabelWidth(80)] public string name = "";
        [VerticalGroup("Data"), LabelWidth(80)] public Sprite NameSprite;
        [VerticalGroup("Data"), LabelWidth(80)] public SkeletonDataAsset skeleton;
        [VerticalGroup("Data"), LabelWidth(80)] public RuntimeAnimatorController animator;
    }
}