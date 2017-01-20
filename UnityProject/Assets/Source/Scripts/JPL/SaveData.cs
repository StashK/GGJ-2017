using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace JPL
{
    public static class SaveData
    {

        private static bool _debug = true;                                  // debug saveData?

        private static Dictionary<string, string> _dataString;              // this contains all the string data
        private static Dictionary<string, float> _dataFloat;                // this contains all the float data
        private static Dictionary<string, int> _dataInt;                    // this contains all the int data
        

        public static void Initialize(bool debug)
        {
            // save settings
            _debug = debug;

            // create the data dictionaries
            _dataString = new Dictionary<string, string>();
            _dataFloat = new Dictionary<string, float>();
            _dataInt = new Dictionary<string, int>();

            // load
        }

        private static void WriteDebug(string s)
        {
            if (_debug)
            {
                Debug.Log(s);
                //Soomla.SoomlaUtils.LogDebug("SaveData", s);
            }
        }

        #region //************ PlayerPref wrapper
        public static void DeleteAll()
        {
            WriteDebug("SaveData.DeleteAll");
            PlayerPrefs.DeleteAll();
        }

        public static void DeleteKey(string key)
        {
            WriteDebug("SaveData.DeleteKey: " + key);
            DataDeleteKey(key);
        }

        public static float GetFloat(string key)
        {
            WriteDebug("SaveData.GetFloat: " + key);
            return GetDataFloat(key);
        }

        public static float GetFloat(string key, float defaultValue)
        {
            WriteDebug("SaveData.GetFloat: " + key + " " + defaultValue);
            return GetDataFloat(key, defaultValue);
        }

        public static int GetInt(string key)
        {
            WriteDebug("SaveData.GetInt: " + key);
            return GetDataInt(key);
        }

        public static int GetInt(string key, int defaultValue)
        {
            WriteDebug("SaveData.GetInt: " + key + " " + defaultValue);
            return GetDataInt(key, defaultValue);
        }

        public static string GetString(string key)
        {
            WriteDebug("SaveData.GetString: " + key);
            return GetDataString(key);
        }

        public static string GetString(string key, string defaultValue)
        {
            WriteDebug("SaveData.GetString: " + key + " " + defaultValue);
            return GetDataString(key, defaultValue);
        }

        public static bool HasKey(string key)
        {
            WriteDebug("SaveData.HasKey: " + key);
            return DataHasKey(key);
        }

        public static void Save()
        {
            WriteDebug("SaveData.Save");
            SaveDataToPlayerPrefs();
        }

        public static void SetFloat(string key, float value)
        {
            WriteDebug("SaveData.SetFloat: " + key + " = " + value);
            SetDataFloat(key, value);
        }

        public static void SetInt(string key, int value)
        {
            WriteDebug("SaveData.SetInt: " + key + " = " + value);
            SetDataInt(key, value);
        }

        public static void SetString(string key, string value)
        {
            WriteDebug("SaveData.SetString: " + key + " = " + value);
            SetDataString(key, value);
        }
        #endregion

        #region Get Data
        private static string GetDataString(string key)
        {
            // load it from current data
            if (_dataString.ContainsKey(key))
            {
                // return string from data
                return _dataString[key];
            }
            // get it from playerPrefs
            else
            {
                // load it from playerPrefs and save to data
                _dataString[key] = Utilities.Decrypt(PlayerPrefs.GetString(Utilities.Md5Sum(key)));
                // return string from data
                return _dataString[key];
            }
        }

        private static string GetDataString(string key, string defaultValue)
        {
            // load it from current data
            if (_dataString.ContainsKey(key))
            {
                // return string from data
                return _dataString[key];
            }
            // get it from playerPrefs
            else
            {
                // load it from playerPrefs and save to data
                _dataString[key] = Utilities.Decrypt(PlayerPrefs.GetString(Utilities.Md5Sum(key), defaultValue));
                // return string from data
                return _dataString[key];
            }
        }

        private static float GetDataFloat(string key)
        {
            // load it from current data
            if (_dataFloat.ContainsKey(key))
            {
                // return string from data
                return _dataFloat[key];
            }
            // get it from playerPrefs
            else
            {
                // load it from playerPrefs and save to data
                //float f;
                //float.TryParse(PlayerPrefs.GetString(Utilities.Md5Sum(key)), out f);
                //_dataFloat[key] = f;
                // return string from data
                _dataFloat[key] = PlayerPrefs.GetFloat(Utilities.Md5Sum(key));
                return _dataFloat[key];
            }
        }

        private static float GetDataFloat(string key, float defaultValue)
        {
            // load it from current data
            if (_dataFloat.ContainsKey(key))
            {
                // return string from data
                return _dataFloat[key];
            }
            // get it from playerPrefs
            else
            {
                // load it from playerPrefs and save to data
                _dataFloat[key] = PlayerPrefs.GetFloat(Utilities.Md5Sum(key), defaultValue);
                // return string from data
                return _dataFloat[key];
            }
        }

        private static int GetDataInt(string key)
        {
            // load it from current data
            if (_dataInt.ContainsKey(key))
            {
                // return string from data
                return _dataInt[key];
            }
            // get it from playerPrefs
            else
            {
                // load it from playerPrefs and save to data
                _dataInt[key] = PlayerPrefs.GetInt(Utilities.Md5Sum(key));
                // return string from data
                return _dataInt[key];
            }
        }

        private static int GetDataInt(string key, int defaultValue)
        {
            // load it from current data
            if (_dataInt.ContainsKey(key))
            {
                // return string from data
                return _dataInt[key];
            }
            // get it from playerPrefs
            else
            {
                // load it from playerPrefs and save to data
                _dataInt[key] = PlayerPrefs.GetInt(Utilities.Md5Sum(key), defaultValue);
                // return string from data
                return _dataInt[key];
            }
        }
        #endregion

        #region Set Data
        private static void SetDataString(string key, string value)
        {
            _dataString[key] = value;
        }

        private static void SetDataInt(string key, int value)
        {
            _dataInt[key] = value;
        }

        private static void SetDataFloat(string key, float value)
        {
            _dataFloat[key] = value;
        }
        #endregion

        #region Data functions
        private static bool DataHasKey(string key)
        {
            if (_dataString.ContainsKey(key) || _dataFloat.ContainsKey(key) || _dataInt.ContainsKey(key))
                return true;
            else
                return false;
        }

        private static void DataDeleteKey(string key)
        {
            // delete from playerprefs
            PlayerPrefs.DeleteKey(key);

            // delete it from data
            if (_dataString.ContainsKey(key))
            {
                _dataString.Remove(key);
            }
            if (_dataFloat.ContainsKey(key))
            {
                _dataFloat.Remove(key);
            }
            if (_dataInt.ContainsKey(key))
            {
                _dataInt.Remove(key);
            }
        }

        private static void SaveDataToPlayerPrefs()
        {
            // save the keys :D (this is what it's all about bitchesz)
            string saveDataStringKeys = "";
            string saveDataFloatKeys = "";
            string saveDataIntKeys = "";

            // save string sjizl
            foreach (string key in _dataString.Keys)
            {
                // save the key
                saveDataStringKeys += key + ";";
                // save to playerprefs
                PlayerPrefs.SetString(Utilities.Md5Sum(key), Utilities.Encrypt(_dataString[key]));
            }
            // save float sjizl
            foreach (string key in _dataFloat.Keys)
            {
                // save the key
                saveDataFloatKeys += key + ";";
                // save to playerprefs
                //PlayerPrefs.SetString(Utilities.Md5Sum(key), Utilities.Encrypt(""+_dataFloat[key]));
                PlayerPrefs.SetFloat(Utilities.Md5Sum(key), _dataFloat[key]);
            }
            // save int sjizl
            foreach (string key in _dataInt.Keys)
            {
                // save the key
                saveDataIntKeys += key + ";";
                // save to playerprefs
                //PlayerPrefs.SetString(Utilities.Md5Sum(key), Utilities.Encrypt(""+_dataInt[key]));
                PlayerPrefs.SetInt(Utilities.Md5Sum(key), _dataInt[key]);
            }

            // save the keys
            PlayerPrefs.SetString("saveDataStringKeys", saveDataStringKeys);
            PlayerPrefs.SetString("saveDataFloatKeys", saveDataFloatKeys);
            PlayerPrefs.SetString("saveDataIntKeys", saveDataIntKeys);

            PlayerPrefs.Save();
        }
        #endregion

        
    }
}