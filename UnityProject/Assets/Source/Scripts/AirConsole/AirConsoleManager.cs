using UnityEngine;
using System.Collections;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

public class AirConsoleManager : MonoBehaviour {

    enum DEVICESTATE { CONNECTED, DISCONNECTED }

    public static float BUTTON_COOLDOWN = 0.1f;

    public static AirConsoleManager Instance;
    private List<Player> playerList = new List<Player>();
    private Dictionary<int, Device> deviceList = new Dictionary<int, Device>();
    private bool isReady = false;
    public int PlayersAvailable
    {
        get
        {
            int i = 0;
            foreach (Player p in playerList)
            {
                if (p.state != Player.STATE.CLAIMED) i++;
            }
            return i;
        }
    }

    #region UNITY_CALLBACKS
    void OnEnable ()
    {
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        AirConsole.instance.onDeviceStateChange += OnDeviceStateChange;
        AirConsole.instance.onCustomDeviceStateChange += OnCustomDeviceStateChange;
        AirConsole.instance.onDeviceProfileChange += OnDeviceProfileChange;
        //AirConsole.instance.onAdShow += OnAdShow;
        //AirConsole.instance.onAdComplete += OnAdComplete;
        AirConsole.instance.onGameEnd += OnGameEnd;
    }

    void Awake ()
    {
        Instance = this;

        // populate players
        //for (int i = 0; i < 4; i++)
        //{
        //    playerList.Add(new Player(i));
        //}

    }

    void Update ()
    {
        // update all inputs
        foreach (Player p in playerList)
        {
            
            // updates all cooldown timers
            p.input.Update();
        }
    }

    void LateUpdate ()
    {
        ResetInput();
    }

    void OnLevelWasLoaded()
    {
        if (!isReady) return;

        Debug.Log("OnLevelWasLoaded");

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex == 2)
        {
            AirConsole.instance.ShowDefaultUI(false);
        }
        else
        {
            AirConsole.instance.ShowDefaultUI(true);
        }

        UpdateDeviceStates();
    }
    #endregion

    #region AIRCONSOLE_CALLBACKS
    void OnReady(string code)
    {
        Debug.Log("AirConsole.OnReady: " + code);
        isReady = true;

        AirConsole.instance.ShowAd();
    }

    void OnMessage(int from, JToken data)
    {
        Debug.Log(data.ToString());

        foreach (Player p in playerList)
        {
            if (p.DeviceId == from)
            {
                //Debug.Log("processing player " + p.PlayerId + " from device " + from + " : " + data);
                p.input.Process(data);
                return;
            }
        }

        // THIS NEEDS CHANING
        // update claim
        if (data.Value<bool>("claim"))
        {
            foreach (Player p in playerList)
            {
                if (p.state != Player.STATE.CLAIMED) p.Claim(from);
                UpdateDeviceStates();
            }
        }

        ////Show an Ad
        //if ((string)data == "show_ad")
        //{
        //    AirConsole.instance.ShowAd();
        //}
    }

    void OnConnect(int deviceId)
    {
        //Log to on-screen Console
        Debug.Log("Device: " + deviceId + " connected. " + AirConsole.instance.GetNickname(deviceId));

        // trying to find a player to reconnect
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].state == Player.STATE.DISCONNECTED && playerList[i].DeviceId == deviceId)
            {
                playerList[i].Claim(deviceId);

                // create device
                AddDevice(deviceId);
                return;
            }
        }
        // trying to find a player to claim
        for (int i = 0; i < playerList.Count; i++)
        {
            if (playerList[i].state == Player.STATE.UNCLAIMED)
            {
                playerList[i].Claim(deviceId);

                // create device
                AddDevice(deviceId);
                return;
            }
        }

        // create device
        playerList.Add(new Player(playerList.Count));
        playerList[playerList.Count - 1].Claim(deviceId);
        AddDevice(deviceId);
    }

    void OnDisconnect(int deviceId)
    {
        //Log to on-screen Console
        Debug.Log("Device: " + deviceId + " disconnected. \n \n");
        // trying to find a player to reconnect
        for (int i = 0; i < 4; i++)
        {
            if (playerList[i].state == Player.STATE.CLAIMED && playerList[i].DeviceId == deviceId)
            {
                playerList[i].Disconnect();
                DisconnectDevice(deviceId);
                return;
            }
        }

        DisconnectDevice(deviceId);
    }

    void OnDeviceStateChange(int deviceId, JToken data)
    {
        //Log to on-screen Console
        Debug.Log("Device State Change on device: " + deviceId + ", data: " + data + "\n \n");
    }

    void OnCustomDeviceStateChange(int deviceId, JToken custom_data)
    {
        //Log to on-screen Console
        Debug.Log("Custom Device State Change on device: " + deviceId + ", data: " + custom_data + "\n \n");
    }

    void OnDeviceProfileChange(int deviceId)
    {
        //Log to on-screen Console
        Debug.Log("Device " + deviceId + " made changes to its profile. \n \n");
    }

    //void OnAdShow()
    //{
    //    //Log to on-screen Console
    //    Debug.Log("On Ad Show \n \n");
    //    JPL.Core.Sounds.UpdateBackgroundVolume(0f);
    //    JPL.Core.Game.Pause();
    //}

    //void OnAdComplete(bool adWasShown)
    //{
    //    //Log to on-screen Console
    //    Debug.Log("Ad Complete. Ad was shown: " + adWasShown + "\n \n");
    //    JPL.Core.Sounds.UpdateBackgroundVolume(1f);
    //    JPL.Core.Game.Unpause();
    //    //LoadingScreen.LoadScene("Main_Menu");
    //}

    void OnGameEnd()
    {
        Debug.Log("OnGameEnd is called");
        Camera.main.enabled = false;
        Time.timeScale = 0.0f;
    }
    #endregion

    #region PLAYER_FUNCTIONS
    public Player GetPlayer (int playerId)
    {
        return playerList[playerId];
    }

    public bool DeviceHasPlayer (int deviceId)
    {
        foreach (Player p in playerList)
        {
            if (p.DeviceId == deviceId) return true;
        }
        return false;
    }

    public Player GetPlayerFromDevice (int deviceId)
    {
        foreach (Player p in playerList)
        {
            if (p.DeviceId == deviceId) return p;
        }
        return null;
    }
    #endregion

    public void ResetInput (bool force = false)
    {
        foreach (Player p in playerList)
        {
            p.input.Reset(force);
        }
    }

    public Dictionary<int, Device> GetDevices ()
    {
        return deviceList;
    }

    private void AddDevice (int deviceId)
    {
        if (!deviceList.ContainsKey(deviceId)) deviceList.Add(deviceId, new Device(this, deviceId));
        UpdateDeviceStates();
    }

    private void DisconnectDevice (int deviceId)
    {
        if (deviceList.ContainsKey(deviceId)) deviceList.Remove(deviceId);
        UpdateDeviceStates();
    }

    public void UpdateDeviceStates ()
    {
        StartCoroutine(SendDeviceStates());
    }

    private IEnumerator SendDeviceStates ()
    {
        while (!AirConsole.instance.IsAirConsoleUnityPluginReady() || !isReady)
        {
            yield return new WaitForEndOfFrame();
        }

        JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

        foreach (int i in deviceList.Keys)
        {
            j.AddField(i.ToString(), deviceList[i].GetJson());
        }
        AirConsole.instance.SetCustomDeviceState(j.Print());
    }

    public class Device
    {
        private AirConsoleManager manager;
        private int deviceId = -1;
        public int DeviceId
        {
            get { return deviceId; }
        }
        public int playerId
        {
            get
            {
                foreach (Player p in manager.playerList)
                {
                    if (p.DeviceId == deviceId) return p.PlayerId;
                }
                return -1;
            }
        }
        public Player player
        {
            get { return AirConsoleManager.Instance.GetPlayer(playerId); }
        }
        public string view
        {
            get
            {
                //if (Game.gameState == GAMESTATE.LOADING)
                //{
                //    return "Loading";
                //}
                //else if(Game.gameState == GAMESTATE.PLAYING || Game.gameState == GAMESTATE.LOADINGPLAY || Game.gameState == GAMESTATE.PAUSED)
                //{
                //    if (playerId != -1)
                //    {
                //        if(Game.gameState == GAMESTATE.PAUSED)
                //            return "Pause";
                //        else
                //            return "GamePlaying";
                //    }
                //    else if (manager.PlayersAvailable > 0)
                //        return "Join";
                //    else
                //        return "MaxPlayers";
                //}
                //else
                //{
                //    if (playerId != -1)
                //        return "MenuPlaying";
                //    else if (manager.PlayersAvailable > 0)
                //        return "Join";
                //    else
                //        return "MaxPlayers";
                //}
                return "Loading";
            }
        }
        public string customClass
        {
            get
            {
                if (view == "MenuPlaying")
                {
                    switch (player.joinState)
                    {
                        case Player.JOINSTATE.ACTIVATED:
                            return "activated";
                        case Player.JOINSTATE.LOCKEDIN:
                            return "locked";
                        case Player.JOINSTATE.NOTJOINED:
                            return "notjoined";
                    }
                }

                return "";
            }
        }
        public string color
        {
            get
            {
                switch (playerId)
                {
                    case 0:
                        return "blue";
                    case 1:
                        return "red";
                    case 2:
                        return "yellow";
                    case 3:
                        return "green";
                    default:
                        return "black";
                }
            }
        }
        public string NickName
        {
            get
            {
                if (AirConsoleManager.Instance.isReady)
                    return AirConsole.instance.GetNickname(deviceId);
                else
                    return "Guest";
            }
        }

        public Device (AirConsoleManager manager, int deviceId)
        {
            this.manager = manager;
            this.deviceId = deviceId;
        }

        public JSONObject GetJson ()
        {
            JSONObject j = new JSONObject(JSONObject.Type.OBJECT);

            j.AddField("playerId", playerId);
            j.AddField("view", view);
            j.AddField("color", color);
            j.AddField("class", customClass);

            Debug.Log(j.Print());

            return j;
        }
    }

    public class Player
    {
        public enum STATE { UNCLAIMED, CLAIMED, DISCONNECTED }
        public STATE state = STATE.UNCLAIMED;
        public enum JOINSTATE { NOTJOINED, ACTIVATED, LOCKEDIN }
        private JOINSTATE _joinstate = JOINSTATE.NOTJOINED;
        public JOINSTATE joinState
        {
            get { return _joinstate; }
            set
            {
                _joinstate = value;
                AirConsoleManager.Instance.UpdateDeviceStates();
            }
        }

        int playerId = -1;
        public int PlayerId
        {
            get { return playerId; }
        }

        int deviceId;
        public int DeviceId
        {
            get { return deviceId; }
        }

        public Input input { get; private set; }

        public Player (int playerId)
        {
            this.deviceId = -1;
            // save playerId
            this.playerId = playerId;
            // init Input
            input = new Input();

            Transform duck = Instantiate(JPL.Core.Prefabs.duck);
            duck.name = "duck_" + playerId;
            duck.GetComponent<Duck>().playerId = playerId;
            
        }

        #region PLAYER_FUNCTIONS
        public void Claim (int deviceId)
        {
            this.deviceId = deviceId;
            state = STATE.CLAIMED;
        }

        public void UnClaim ()
        {
            deviceId = -1;
            state = STATE.UNCLAIMED;
        }

        public void Disconnect ()
        {
            state = STATE.DISCONNECTED;
        }
        #endregion

        #region PLAYER_INPUT_FUNCTIONS
        public bool GetButtonDown (string key)
        {
            switch (key)
            {
                //// Gameplay keys
                //case InputAction.GamePlay.Jump:
                //    return input.jump;
                //case InputAction.GamePlay.Dash:
                //    return input.jump;
                //case InputAction.GamePlay.Shield:
                //    return input.shield;
                //case InputAction.GamePlay.Grab:
                //    return input.grab;
                //case InputAction.GamePlay.Pause:
                //    return input.start;
                //case InputAction.GamePlay.Exit:
                //    return input.select;
                //// Menu keys
                //case InputAction.Menu.Start:
                //    return input.start;
                //case InputAction.Menu.UISubmit:
                //    return input.jump;
                //case InputAction.Menu.UICancel:
                //    return input.grab;
                case InputAction.Gameplay.MoveLeft:
                    return input.moveLeft;
                case InputAction.Gameplay.MoveRight:
                    return input.moveRight;
                case InputAction.Gameplay.WeaponLeft:
                    return input.weaponLeft;
                case InputAction.Gameplay.WeaponRight:
                    return input.weaponRight;
            }

            return false;
        }

        public bool GetButtonUp (string key)
        {
            return GetButtonDown(key);
        }

        public float GetAxis (string key)
        {
            switch (key)
            {
                //case InputAction.GamePlay.MoveHorizontal:
                //    return input.x;
                //case InputAction.GamePlay.MoveVertical:
                //    return input.y;
            }

            return 0f;
        }
        #endregion
    }

    public class Input
    {

        public bool moveLeft { get; private set; }
        public bool moveRight { get; private set; }
        public bool weaponLeft { get; private set; }
        public bool weaponRight { get; private set; }

        /// <summary>
        /// processes the raw data
        /// </summary>
        /// <param name="data"></param>
        public void Process (JToken data)
        {
            ///Debug.Log(data.ToString());
            // update joystick input
            //x = data["movement"].Value<float>("x") / 50f;
            //y = data["movement"].Value<float>("y") / 50f * -1f;

            //Debug.Log("X: " + x + " Y: " + y);
            //data.Value<bool>("jump")


            if (data.Value<string>("element") == "move-left")
            {
                moveLeft = data["data"].Value<bool>("pressed");
            }

            if (data.Value<string>("element") == "move-right")
            {
                moveRight = data["data"].Value<bool>("pressed");
            }

            if (data.Value<string>("element") == "weapon-left")
            {
                weaponLeft = data["data"].Value<bool>("weapon-left");
            }
            if (data.Value<string>("element") == "weapon-right")
            {
                weaponRight = data["data"].Value<bool>("pressed");
            }
        }

        /// <summary>
        /// needs to be called to update the cooldown timers
        /// </summary>
        public void Update ()
        {
            //jumpCooldown -= Time.unscaledDeltaTime;
            //grabCooldown -= Time.unscaledDeltaTime;
            //shieldCooldown -= Time.unscaledDeltaTime;
            //startCooldown -= Time.unscaledDeltaTime;
            //selectCooldown -= Time.unscaledDeltaTime;
        }

        public void Reset(bool force = false)
        {
            //if (Game.gameState == GAMESTATE.PLAYING || force)
            //{
            //    // reset
            //    jump = false;
            //    grab = false;
            //    start = false;
            //    shield = false;
            //    select = false;

            //    if (Game.gameState != GAMESTATE.PLAYING)
            //    {
            //        x = 0f;
            //        y = 0f;
            //    }
            //}
        }
    }
}
