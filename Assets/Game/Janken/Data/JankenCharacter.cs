
using System;
using Sirenix.OdinInspector;
using Spine.Unity;
using UnityEditor.Animations;
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
        [VerticalGroup("Data"), LabelWidth(60)] public string name = "";
        [VerticalGroup("Data"), LabelWidth(60)] public SkeletonDataAsset skeleton;
        [VerticalGroup("Data"), LabelWidth(60)] public AnimatorController animator;
    }
}