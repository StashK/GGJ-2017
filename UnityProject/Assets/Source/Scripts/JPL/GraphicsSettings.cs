using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace JPL
{
    public class GraphicsSettings : MonoBehaviour
    {
        // Previous settings object
        public static SettingsHolder settingsHolder;

        // Rendering Settings
        private static Dictionary<string, int> qualityLevelOptions;
        private static Dictionary<string, int> textureResolutionOptions;
        private static Dictionary<string, int> anisoTropicOptions;
        private static Dictionary<string, int> antiAliasingOptions;

        // Shadow Settings
        private static Dictionary<string, int> shadowResolutionOptions;
        public static List<string> shadows;
        public static List<string> shadowRes;

        public static void Init()
        {
            settingsHolder = new SettingsHolder();
            SaveData.Initialize(false);

            qualityLevelOptions = new Dictionary<string, int>();
            textureResolutionOptions = new Dictionary<string, int>();
            anisoTropicOptions = new Dictionary<string, int>();
            antiAliasingOptions = new Dictionary<string, int>();

            shadows = new List<string>();
            shadowRes = new List<string>();

            LoadQualityLevelOptions();
            LoadTextureResolutionOptions();
            LoadAnisoTropicOptions();
            LoadAntiAliasingOptions();

        }

        ///////////////////////////////////////////////
        ///////////   RENDERING SETTINGS    ///////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Loads all quality levels
        /// </summary>
        private static void LoadQualityLevelOptions()
        {
            for (int i = 0; i < QualitySettings.names.Length; i++)
            {
                qualityLevelOptions.Add(QualitySettings.names[i], i);
            }

            // HARD LOAD SHADOWS, THANK YOU FOR HIDING SHADOWS UNITY.
            shadows.Add("Disabled");
            shadows.Add("Hard");
            shadows.Add("Hard");
            shadows.Add("HardSoft");
            shadows.Add("HardSoft");
            shadows.Add("HardSoft");

            // HARD LOAD SHADOW RESOLUTION, THANK YOU FOR HIDING SHADOWS UNITY.
            shadowRes.Add("Low");
            shadowRes.Add("Low");
            shadowRes.Add("Medium");
            shadowRes.Add("Medium");
            shadowRes.Add("High");
            shadowRes.Add("VeryHigh");
        }

        /// <summary>
        /// Loads all texture resolution options
        /// </summary>
        private static void LoadTextureResolutionOptions()
        {
            textureResolutionOptions.Add("Max", 0);
            textureResolutionOptions.Add("High", 1);
            textureResolutionOptions.Add("Medium", 2);
            textureResolutionOptions.Add("Low", 3);
        }

        /// <summary>
        /// Loads all anisotropic options
        /// </summary>
        private static void LoadAnisoTropicOptions()
        {
            anisoTropicOptions.Add("Disable", 0);
            anisoTropicOptions.Add("Enable", 1);
            anisoTropicOptions.Add("Force Enable", 2);
        }

        /// <summary>
        /// Loads all AntiAliasing options
        /// </summary>
        private static void LoadAntiAliasingOptions()
        {
            antiAliasingOptions.Add("None", 0);
            antiAliasingOptions.Add("MSAA 2X", 2);
            antiAliasingOptions.Add("MSAA 4X", 4);
            antiAliasingOptions.Add("MSAA 8X", 8);
        }

        ///////////////////////////////////////////////
        ////////////    SHADOW SETTINGS    ////////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Load all shadow options
        /// </summary>
        private static void LoadShadowResolutionOptions()
        {
            
        }

        ///////////////////////////////////////////////
        ///////////   RENDERING SETTERS     ///////////
        ///////////////////////////////////////////////

        /// <summary>
        /// Sets the quality level to the given parameter.
        /// </summary>
        /// <param name="qualityName">Name of the qualitylevel to set to</param>
        public static void SetQuality(string qualityName)
        {
            if (qualityLevelOptions.ContainsKey(qualityName))
            {
                QualitySettings.SetQualityLevel(qualityLevelOptions[qualityName]);
            }
            else
            {
                Debug.LogWarning("QualityLevel " + qualityName + " does not exist.");
            }
        }

        /// <summary>
        /// Sets the quality level based on the given index.
        /// </summary>
        /// <param name="qualityIndex">Index of the quality level to set to</param>
        public static void SetQuality(int qualityIndex)
        {
            if (qualityLevelOptions.ContainsValue(qualityIndex))
            {
                QualitySettings.SetQualityLevel(qualityIndex);
            }
            else
            {
                Debug.LogWarning("QualityLevel with index "+qualityIndex+" does not exist.");
            }
        }

        /// <summary>
        /// Set the texture resolution
        /// </summary>
        /// <param name="resolutionName">Name of the texture resolution (Max, High, Medium, Low)</param>
        public static void SetTextureResolution(string resolutionName)
        {
            if (textureResolutionOptions.ContainsKey(resolutionName))
            {
                QualitySettings.masterTextureLimit = textureResolutionOptions[resolutionName];
            }
            else
            {
                Debug.LogWarning("Texture resolution with name: " + resolutionName + ", does not exist.");
            }
        }

        /// <summary>
        /// Set the texture resolution based on index
        /// </summary>
        /// <param name="resolutionIndex">Set the resolution based on index (0 = Max, 1 = High, 2 = Medium, 3 = Low)</param>
        public static void SetTextureResolution(int resolutionIndex)
        {
            if (textureResolutionOptions.ContainsValue(resolutionIndex))
            {
                QualitySettings.masterTextureLimit = resolutionIndex;
            }
            else
            {
                Debug.LogWarning("Texture resolution with index: " + resolutionIndex + ", does not exist.");
            }
        }

        /// <summary>
        /// Set the aniso tropic filtering
        /// </summary>
        /// <param name="anisoName">Name of the aniso filtering to set to (Disable, Enable, Force Enable)</param>
        public static void SetAnisoTropic(string anisoName)
        {
            if (anisoTropicOptions.ContainsKey(anisoName))
            {
                switch (anisoTropicOptions[anisoName])
                {
                    case 0:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                        break;
                    case 1:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                        break;
                    case 2:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                        break;
                }
            }
            else
            {
                Debug.LogWarning("AnisoTropic filtering with name: " + anisoName + ", does not exist.");
            }
        }

        /// <summary>
        /// Set the aniso tropic filtering
        /// </summary>
        /// <param name="anisoName">Index of the aniso filtering to set to</param>
        public static void SetAnisoTropic(int anisoIndex)
        {
            if (anisoTropicOptions.ContainsValue(anisoIndex))
            {
                switch (anisoIndex)
                {
                    case 0:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
                        break;
                    case 1:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
                        break;
                    case 2:
                        QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
                        break;
                }
            }
            else
            {
                Debug.LogWarning("AnisoTropic filtering with index: " + anisoIndex + ", does not exist.");
            }
        }

        /// <summary>
        /// Set the anti aliasing
        /// </summary>
        /// <param name="antiAliasingName">Name of the anti aliasing to set to (None, MSAA 2X, MSAA 4X, MSAA 8X)</param>
        public static void SetAntiAliasing(string antiAliasingName)
        {
            if (antiAliasingOptions.ContainsKey(antiAliasingName))
            {
                QualitySettings.antiAliasing = antiAliasingOptions[antiAliasingName];
            }
            else
            {
                Debug.LogWarning("AntiAliasing with name: " + antiAliasingName + ", does not exist.");
            }
        }

        /// <summary>
        /// Set the anti aliasing
        /// </summary>
        /// <param name="antiAliasingIndex">Index of the anti aliasing to use (0, 2, 4, 8)</param>
        public static void SetAntiAliasing(int antiAliasingIndex)
        {
            if (antiAliasingOptions.ContainsValue(antiAliasingIndex))
            {
                QualitySettings.antiAliasing = antiAliasingIndex;
            }
            else
            {
                Debug.LogWarning("AntiAliasing index: " + antiAliasingIndex + ", does not exist.");
            }
        }

        /// <summary>
        /// Set the realtime reflection probes
        /// </summary>
        /// <param name="value">Turn realtime reflection probes true or false</param>
        public static void SetRealtimeReflectionProbes(bool value)
        {
            QualitySettings.realtimeReflectionProbes = value;
        }

        /// <summary>
        /// Set whether billboard objects face the camera position
        /// </summary>
        /// <param name="value">Turn this option on (true) or off (false)</param>
        public static void SetBillboardsFacingCamera(bool value)
        {
            QualitySettings.billboardsFaceCameraPosition = value;
        }

        ///////////////////////////////////////////////
        /////////    VARIUOUS GFX SETTERS     /////////
        ///////////////////////////////////////////////

        public static void SetVSync(int vSyncIndex)
        {
            QualitySettings.vSyncCount = vSyncIndex;
        }

        ///////////////////////////////////////////////
        //////  CURRENT QUALITY PRESET GETTERS   //////
        ///////////////////////////////////////////////

        /// <summary>
        /// Get all preset qualities
        /// </summary>
        /// <returns>Returns a string[] with all quality names</returns>
        public static string[] GetPresetQualities()
        {
            return qualityLevelOptions.Keys.ToArray<string>();
        }

        /// <summary>
        /// Returns the Anti Aliasing for the current Quality Level
        /// </summary>
        /// <returns>Index of the Anti Aliasing in the antiAliasingOptions dictionary</returns>
        public static int GetQualityAntiAliasing()
        {
            int aaCounter = 0;

            foreach (KeyValuePair<string, int> KVP in antiAliasingOptions)
            {
                if (KVP.Value == QualitySettings.antiAliasing)
                {
                    return aaCounter;
                }

                aaCounter++;
            }

            return 0;
        }

        /// <summary>
        /// Returns the current Texture Resolution divider. (0 = Max, 1 = High, 2 = Medium, 3 = Low)
        /// </summary>
        /// <returns>MasterTextureLimit (0 = Max, 1 = High, 2 = Medium, 3 = Low)</returns>
        public static int GetQualityTextureResolution()
        {
            return QualitySettings.masterTextureLimit;
        }

        /// <summary>
        /// Returns the current Aniso Tropic Filtering value
        /// </summary>
        /// <returns>An index relative to the AnisotropicFiltering enum. (Disable = 0, Enable = 1, ForceEnable = 2)</returns>
        public static int GetQualityAnisotropic()
        {
            int anisoValue = 0;

            switch (QualitySettings.anisotropicFiltering)
            {
                case AnisotropicFiltering.Disable:
                    anisoValue = 0;
                    break;
                case AnisotropicFiltering.Enable:
                    anisoValue = 1;
                    break;
                case AnisotropicFiltering.ForceEnable:
                    anisoValue = 2;
                    break;
            }

            return anisoValue;
        }

        /// <summary>
        /// Returns whether realtime reflection probes are on or off
        /// </summary>
        /// <returns></returns>
        public static bool GetQualityRealtimeReflectionProbes()
        {
            return QualitySettings.realtimeReflectionProbes;
        }

        /// <summary>
        /// Returns whether the billboards face the camera position
        /// </summary>
        /// <returns></returns>
        public static bool GetBillboardFacingCamera()
        {
            return QualitySettings.billboardsFaceCameraPosition;
        }

        /// <summary>
        /// Returns whether vSync is on or off.
        /// </summary>
        /// <returns></returns>
        public static int GetVSync()
        {
            return QualitySettings.vSyncCount; 
        }

        ///////////////////////////////////////////////
        //////  SAVE AND LOAD FROM PLAYERPREFS   //////
        ///////////////////////////////////////////////

        public static void SaveSettings()
        {

        }

        public static void LoadSettings()
        {

        }

        /// <summary>
        /// Class holding the previous settings of the player.
        /// </summary>
        public class SettingsHolder
        {
            public int qualityLevelIndex;

            public int antiAliasing;
            public int textureResolution;
            public int anisotropicFiltering;
            public int shadowRes;
            public int shadow;

            public bool realtimeReflection;
            public bool billboardsFaceCamera;
            public bool vSync;

            public void StoreSettings(int antiAliasing, int textureResolution, int anisotropicFiltering, int shadowRes, int shadow, bool realtimeReflectionProbes, bool billboardsFaceCamera, bool vSyncToggle)
            {
                this.qualityLevelIndex = QualitySettings.GetQualityLevel();
                this.antiAliasing = antiAliasing;
                this.textureResolution = textureResolution;
                this.anisotropicFiltering = anisotropicFiltering;
                this.shadowRes = shadowRes;
                this.shadow = shadow;

                this.realtimeReflection = realtimeReflectionProbes;
                this.billboardsFaceCamera = billboardsFaceCamera;
                this.vSync = vSyncToggle;
            }     

            public void SaveSettingsToPlayerPrefs()
            {
                SaveData.SetInt("QualityLevel", this.qualityLevelIndex);
                SaveData.SetInt("AntiAliasing", this.antiAliasing);
                SaveData.SetInt("TextureResolution", this.textureResolution);
                SaveData.SetInt("AnisotropicFiltering", this.anisotropicFiltering);
                SaveData.SetInt("ShadowRes", this.shadowRes);
                SaveData.SetInt("Shadow", this.shadow);

                SaveData.SetInt("RealtimeReflectionProbes", realtimeReflection ? 1 : 0);
                SaveData.SetInt("BillboardsFaceCamera", billboardsFaceCamera ? 1 : 0);
                SaveData.SetInt("VSync", vSync ? 1 : 0);

                SaveData.Save();

                Debug.Log("Saved data son");
            }

            public bool LoadSettingsFromPlayerPrefs()
            {
                this.qualityLevelIndex = SaveData.GetInt("QualityLevel");
                this.antiAliasing = SaveData.GetInt("AntiAliasing");
                this.textureResolution = SaveData.GetInt("TextureResolution");
                this.anisotropicFiltering = SaveData.GetInt("AnisotropicFiltering");
                this.shadowRes = SaveData.GetInt("ShadowRes");
                this.shadow = SaveData.GetInt("Shadow");

                this.realtimeReflection = SaveData.GetInt("RealtimeReflectionProbes") == 1 ? true : false;
                this.billboardsFaceCamera = SaveData.GetInt("BillboardsFaceCamera") == 1 ? true : false;
                this.vSync = SaveData.GetInt("VSync") == 1 ? true : false;

                return true;
            }
        }
    }
}
