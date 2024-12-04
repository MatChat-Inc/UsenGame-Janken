using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace USEN.Games.Janken
{
    [CreateAssetMenu(fileName = "JankenCharacters", menuName = "Janken/Characters")]
    public class JankenCharacters : ScriptableObject
    {
        public static JankenCharacters Default
        {
            get
            {
                if (_default == null) 
                    _default = Resources.Load<JankenCharacters>("JankenCharacters");
                return _default;
            }
        }

        private static JankenCharacters _default;

        [TableList(ShowIndexLabels = true, AlwaysExpanded = true, DrawScrollView = false)]
        public List<JankenCharacter> characters;

    }
}