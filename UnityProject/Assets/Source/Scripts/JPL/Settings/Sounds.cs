using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace JPL
{
    public class Sounds : Base.Sounds
    {
        //public List<AudioClip> damageSounds;
        //public List<AudioClip> dieSounds;
        //public List<AudioClip> jumpSounds;
        //public List<AudioClip> winSounds;
        //public List<AudioClip> dashSounds;
        //public List<AudioClip> hitSounds;
        //public List<AudioClip> godBiteSounds;
        //public List<AudioClip> godGrowlSounds;
        //public AudioClip background;
        //public AudioClip explosion;
        //public AudioClip door;

        //public AudioClip countdown;
        //public AudioClip countdownStart;

        public List<AudioClip> sfx_character_jump = new List<AudioClip>();  //
        public List<AudioClip> sfx_character_dash = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_impact = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_spawn = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_grab = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_throw = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_shield = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_recover = new List<AudioClip>(); //
        public List<AudioClip> sfx_character_land = new List<AudioClip>(); //

        public List<AudioClip> sfx_behemoth_growl = new List<AudioClip>(); //
        public List<AudioClip> sfx_behemoth_eat = new List<AudioClip>(); //

        public List<AudioClip> sfx_object_throw = new List<AudioClip>(); //
        public List<AudioClip> sfx_object_impact_hard = new List<AudioClip>(); //
        public List<AudioClip> sfx_object_impact_soft = new List<AudioClip>(); //

        public List<AudioClip> sfx_env_sc_geyser = new List<AudioClip>();
        public List<AudioClip> sfx_env_sc_geyser_eruption = new List<AudioClip>();
        public List<AudioClip> sfx_env_sc_geyser_rumble = new List<AudioClip>();
        public List<AudioClip> sfx_env_sc_glass_impact = new List<AudioClip>();
        public List<AudioClip> sfx_env_yd_statue_slide = new List<AudioClip>();
        public List<AudioClip> sfx_env_yd_statue_start = new List<AudioClip>();
        public List<AudioClip> sfx_env_yd_statue_bounce = new List<AudioClip>();
        public List<AudioClip> sfx_env_fb_flower_growth = new List<AudioClip>(); //
        public List<AudioClip> sfx_env_fb_flower_explode = new List<AudioClip>(); //
        public List<AudioClip> sfx_env_zt_zombie_growl = new List<AudioClip>();
        public List<AudioClip> sfx_env_zt_zombie_death = new List<AudioClip>();
        public List<AudioClip> sfx_env_gl_jelly_bounce = new List<AudioClip>();
        public List<AudioClip> sfx_env_gl_switch_slide = new List<AudioClip>();
        public List<AudioClip> sfx_env_gs_pipe_suction = new List<AudioClip>(); //
        public List<AudioClip> sfx_env_gs_pipe_vomit = new List<AudioClip>(); //

        public List<AudioClip> sfx_ui_btn_switch = new List<AudioClip>();   //
        public List<AudioClip> sfx_ui_btn_confirm = new List<AudioClip>();  //
        public List<AudioClip> sfx_ui_btn_cancel = new List<AudioClip>();   //
        public List<AudioClip> sfx_ui_btn_select = new List<AudioClip>();   //
        public List<AudioClip> sfx_ui_slotmachine_tick = new List<AudioClip>();
        public List<AudioClip> sfx_ui_slotmachine_select = new List<AudioClip>();

        public List<AudioClip> sfx_gameplay_countdown_start = new List<AudioClip>();
        public List<AudioClip> sfx_gameplay_countdown_end = new List<AudioClip>();

        public AudioClip music_soundtrack_intro; //
        public AudioClip music_soundtrack_loop;
        public AudioClip music_background_base;

        void Start ()
        {
            LoadSounds(OnClipLoad);

            const BindingFlags flags = /*BindingFlags.NonPublic | */BindingFlags.Public |
             BindingFlags.Instance | BindingFlags.Static;
            FieldInfo[] fields = this.GetType().GetFields(flags);
            foreach (FieldInfo fieldInfo in fields)
            {
                Debug.Log("Obj: " + this.name + ", Field: " + fieldInfo.Name);
            }
            PropertyInfo[] properties = this.GetType().GetProperties(flags);
            foreach (PropertyInfo propertyInfo in properties)
            {
                Debug.Log("Obj: " + this.name + ", Property: " + propertyInfo.Name);
            }
        }

        void OnClipLoad (AudioClip clip)
        {

            FieldInfo field = this.GetType().GetField(VariableName(clip.name));
            if (field != null)
            {
                if (clip.name.Contains("sfx"))
                {
                    List<AudioClip> temp = (List<AudioClip>)field.GetValue((object)this);
                    temp.Add(clip);
                    field.SetValue(this, temp);
                }
                else  if (clip.name.Contains("music"))
                {
                    field.SetValue(this, clip);
                }
            }
            else
            {
                //Debug.Log("Didn't find " + VariableName(clip.name));
            }
        }

        string VariableName (string s)
        {
            if (s.Any(char.IsDigit))
            {
                string t = s.Remove(s.LastIndexOf('_') + 1);
                return t.TrimEnd('_');
            }
            else
            {
                string t = s.Remove(s.LastIndexOf('.') + 1);
                return t.TrimEnd('.');
            }
        }
    }
}
