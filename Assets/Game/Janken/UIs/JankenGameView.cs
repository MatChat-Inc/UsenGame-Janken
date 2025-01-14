// Created by LunarEclipse on 2024-7-21 19:44.

using Cysharp.Threading.Tasks;
using Luna;
using Luna.UI;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using USEN.Games.Common;
using USEN.Games.Roulette;

namespace USEN.Games.Janken
{
    public class JankenGameView : Widget
    {
        public TextMeshProUGUI confirmText;
        public BottomPanel bottomPanel;
        
        public JankenCharacterController characterController;
        
        public Sprite rouletteBackground;

        private void Start()
        {
            characterController.SwitchCharacter(JankenPreferences.SelectedCharacter);
            
            characterController.OnStateChange += (state) =>
            {
                switch (state)
                {
                    case JankenCharacterController.JankenCharacterState.Idle:
                        ResetControlButtons();
                        break;
                    case JankenCharacterController.JankenCharacterState.End:
                        ShowControlButtons();
                        break;
                }
            };
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) ||
                Input.GetButtonDown("Cancel")) {
                OnExitButtonClicked();
            } else if (Input.GetKeyDown(KeyCode.Return) ||
                       Input.GetButtonDown("Submit")) {
                OnConfirmButtonClicked();
            }
        }

        private void OnEnable()
        {
            ResetControlButtons();
            characterController.ResetState();
            
            bottomPanel.onExitButtonClicked += OnExitButtonClicked;
            bottomPanel.onConfirmButtonClicked += OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked += OnGreenButtonClicked;
            bottomPanel.onYellowButtonClicked += OnYellowButtonClicked;
        }

        private void OnDisable()
        {
            bottomPanel.onExitButtonClicked -= OnExitButtonClicked;
            bottomPanel.onConfirmButtonClicked -= OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked -= OnGreenButtonClicked;
            bottomPanel.onYellowButtonClicked -= OnYellowButtonClicked;
        }

        private void OnExitButtonClicked()
        {
            PopupConfirmView();
        }

        private void OnConfirmButtonClicked()
        {
            switch (characterController.State)
            {
                case JankenCharacterController.JankenCharacterState.Idle:
                    characterController.StartJanken();
                    bottomPanel.confirmButton.gameObject.SetActive(false);
                    confirmText.text = "もう一度じゃんけんをする";
                    break;
            }
        }

        private void OnRedButtonClicked()
        {
            
        }

        private async void OnBlueButtonClicked()
        {
            
        }
        
        private async void OnGreenButtonClicked()
        {
            await Navigator.Push<RouletteGameSelectionView>((view) => {
                view.Category = RouletteManager.Instance.GetCategory("バツゲーム");
                BgmManager.Resume();
                
                if (RoulettePreferences.DisplayMode == RouletteDisplayMode.Random)
                { 
                    Navigator.Push<USEN.Games.Roulette.RouletteGameView>(async (view) => {
                        view.RouletteData = RouletteManager.Instance.GetRandomRoulette();
                    });
                }
            });
        }

        private async void OnYellowButtonClicked()
        {
            BgmManager.Pause();
            await Navigator.Push<CommendView>();
            BgmManager.Resume();
        }
        
        private void ShowControlButtons()
        {
            bottomPanel.redButton.gameObject.SetActive(true);
            bottomPanel.greenButton.gameObject.SetActive(true);
            bottomPanel.yellowButton.gameObject.SetActive(true);
        }
        
        private void HideControlButtons()
        {
            bottomPanel.redButton.gameObject.SetActive(false);
            bottomPanel.greenButton.gameObject.SetActive(false);
            bottomPanel.yellowButton.gameObject.SetActive(false);
        }
        
        private void ResetControlButtons()
        {
            HideControlButtons();
            bottomPanel.confirmButton.gameObject.SetActive(true);
        }
        
        private void PopupConfirmView()
        {
            Navigator.ShowModal<PopupOptionsView>(
                builder: (popup) =>
                {
                    popup.onOption1 = () => Navigator.Pop();
                    popup.onOption2 = () => Navigator.PopToRoot();
                    popup.onOption3 = () => SceneManager.LoadScene("GameEntries");
                });
        }
    }
}