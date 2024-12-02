using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace USEN.Games.Janken
{
    [CreateAssetMenu(fileName = "JankenCharacters", menuName = "Janken/Characters")]
    public class JankenCharacters : ScriptableObject
    {
        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, DrawScrollView = false)]
        public List<JankenCharacter> characters;
    }
}