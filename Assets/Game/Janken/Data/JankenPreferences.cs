// Created by LunarEclipse on 2024-9-9 5:33.

using UnityEngine;

namespace USEN.Games.Janken
{
    public static class JankenPreferences
    {
        public static int SelectedCharacter
        {
            get => PlayerPrefs.GetInt("Janken.SelectedCharacter", 0);
            set => PlayerPrefs.SetInt("Janken.SelectedCharacter", value);
        }
        
        public static int CommendationVideoOption
        {
            get => PlayerPrefs.GetInt("Janken.CommendationVideoOption", 0);
            set => PlayerPrefs.SetInt("Janken.CommendationVideoOption", value);
        }
        
        public static float BgmVolume
        {
            get => PlayerPrefs.GetFloat("Janken.BgmVolume", 1);
            set => PlayerPrefs.SetFloat("Janken.BgmVolume", value);
        }
        
        public static float SfxVolume
        {
            get => PlayerPrefs.GetFloat("Janken.SfxVolume", 1);
            set => PlayerPrefs.SetFloat("Janken.SfxVolume", value);
        }
    }
}