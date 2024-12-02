using System;
using System.Collections.Generic;
using System.Linq;
using Luna.UI;
using Luna.UI.Navigation;
using UnityEngine;
using USEN.Games.Common;

namespace USEN.Games.Janken
{
    public class JankenCharacterView : Widget
    {
        public JankenCharacterList listView;
        public BottomPanel bottomPanel;

        public List<JankenCharacter> Categories
        {
            get => listView.Data;
            set => listView.Data = value;
        }

        void Start()
        {
            // listView.FocusOnCell(0);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) 
                Navigator.Pop();
        }

    }
}
