// Created by LunarEclipse on 2024-7-21 19:44.

using System;
using Cysharp.Threading.Tasks;
using Luna;
using Luna.Extensions;
using Luna.UI;
using Luna.UI.Navigation;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using USEN.Games.Common;
using USEN.Games.Roulette;

namespace USEN.Games.Janken
{
    public class JankenGameView : Widget
    {
        public TextMeshProUGUI confirmText;
        public BottomPanel bottomPanel;
        
        [FormerlySerializedAs("spineCharacter")] 
        public AssetReferenceGameObject spineMecanim;
        public JankenCharacterController characterController;
        
        public Sprite rouletteBackground;
        
        private int _playTimes = 0;
        private bool _isFinalGame = false;
        
        
        private async void Start()
        {
            HideControlButtons();

            if (characterController == null)
            {
                var prefab = await spineMecanim.LoadAssetAsync();
                var character = Instantiate(prefab, transform);
                characterController = character.GetComponent<JankenCharacterController>();
            }
            
            characterController.SwitchCharacter(JankenPreferences.SelectedCharacter);
            characterController.OnStateChange += (state) => {
                switch (state)
                {
                    case JankenCharacterController.JankenCharacterState.Waiting:
                        ++_playTimes;
                        break; 
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
            BgmManager.Resume();
            
            ResetControlButtons();
            characterController?.ResetState();
            
            bottomPanel.onExitButtonClicked += OnExitButtonClicked;
            bottomPanel.onConfirmButtonClicked += OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked += OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked += OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked += OnGreenButtonClicked;
            bottomPanel.onYellowButtonClicked += OnYellowButtonClicked;
        }

        private void OnDisable()
        {
            BgmManager.Pause();
            
            characterController?.StopJanken();
            
            bottomPanel.onExitButtonClicked -= OnExitButtonClicked;
            bottomPanel.onConfirmButtonClicked -= OnConfirmButtonClicked;
            bottomPanel.onRedButtonClicked -= OnRedButtonClicked;
            bottomPanel.onBlueButtonClicked -= OnBlueButtonClicked;
            bottomPanel.onGreenButtonClicked -= OnGreenButtonClicked;
            bottomPanel.onYellowButtonClicked -= OnYellowButtonClicked;
        }

        private void OnDestroy()
        {
            BgmManager.Resume();
            if (_isFinalGame)
                BgmManager.Play(R.Audios.BgmJanken);
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
                    HideControlButtons();
                    break;
            }
        }

        private void OnRedButtonClicked()
        {
            _isFinalGame = true;
            bottomPanel.redButton.gameObject.SetActive(false);
            R.Audios.BgmJankenFinal.Load().Then(BgmManager.Play);
        }

        private async void OnBlueButtonClicked()
        {
            
        }
        
        private async void OnGreenButtonClicked()
        {
            await Navigator.Push<RouletteGameSelectionView>((view) => {
                view.Category = RouletteManager.Instance.GetCategory("バツゲーム");
                // BgmManager.Resume();
                
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
            if (_playTimes >= 1) 
                bottomPanel.redButton.gameObject.SetActive(!_isFinalGame);
            bottomPanel.greenButton.gameObject.SetActive(true);
            bottomPanel.yellowButton.gameObject.SetActive(true);
        }
        
        private void HideControlButtons()
        {
            bottomPanel.greenButton.gameObject.SetActive(false);
            bottomPanel.yellowButton.gameObject.SetActive(false);
        }
        
        private void ResetControlButtons()
        {
            bottomPanel.confirmButton.gameObject.SetActive(true);
        }
        
        private void PopupConfirmView()
        {
            var orginalVolume = BgmManager.Volume;
            
            Navigator.ShowModal<PopupOptionsView>(
                builder: (popup) =>
                {
                    popup.onOption1 = () => Navigator.Pop();
                    popup.onOption2 = () =>
                    {
                        SFXManager.Stop();
                        Navigator.PopToRoot();
                    }; 
#if UNITY_ANDROID
                    popup.onOption3 = () => Android.Back();
#else
                    popup.onOption3 = () => Application.Quit();
#endif
                    BgmManager.Resume();
                    BgmManager.SetVolume(BgmManager.Volume * 0.6f, 0.3f);
                });
            
            BgmManager.SetVolume(orginalVolume, 0.3f);
        }
    }
}