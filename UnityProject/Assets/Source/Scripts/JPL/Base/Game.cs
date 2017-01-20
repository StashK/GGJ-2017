using UnityEngine;
using System.Collections;

namespace JPL.Base
{
    public class Game : Mono
    {
        private GAMESTATE state = GAMESTATE.PLAYING;

        /* USED IN PAUSE/UNPAUSE */
        /// The orriginal timescale before the game got paused
        private float _timeScale;


        public bool IsPlaying
        {
            get { return state == GAMESTATE.PLAYING; }
        }

        public bool IsPaused
        {
            get { return state == GAMESTATE.PAUSED; }
        }

        public void Pause()
        {
            if (state != GAMESTATE.PAUSED) {
                // save the current timescale
                _timeScale = Time.timeScale;
                // set the state
                state = GAMESTATE.PAUSED;
                // set the timescale
                Time.timeScale = 0f;
            }
        }

        public void Unpause()
        {
            if (state != GAMESTATE.PLAYING)
            {
                // set the timescale
                Time.timeScale = _timeScale;
                // set the state
                state = GAMESTATE.PLAYING;
            }
        }
    }
}
