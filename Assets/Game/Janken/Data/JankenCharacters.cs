using System.Collections.Generic;
using System.Threading.Tasks;
using Luna;
using Luna.Extensions;
using Sirenix.OdinInspector;
using UnityEngine;

namespace USEN.Games.Janken
{
    [CreateAssetMenu(fileName = "JankenCharacters", menuName = "Janken/Characters")]
    public class JankenCharacters : ScriptableObject
    {
        public static JankenCharacters Default => DefaultAsset;
        public static readonly Asset<JankenCharacters> DefaultAsset = new("Assets/Game/Janken/Data/JankenCharacters.asset"); 

        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, DrawScrollView = false)]
        public List<JankenCharacter> characters;
    }
}