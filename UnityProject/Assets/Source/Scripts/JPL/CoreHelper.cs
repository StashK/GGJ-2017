using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace JPL
{
    public class CoreHelper : Mono
    {
        /****** VARIABLES ******/


        // Use this for initialization

        public void LoadScene(int i)
        {
            SceneManager.LoadScene(i);
        }

        public void LoadScene(int i, float time)
        {
            StartCoroutine(_LoadScene(i, time));
        }

        private IEnumerator _LoadScene(int i, float time)
        {
            yield return new WaitForSeconds(time);

            LoadScene(i);
        }

        public void DisableForSeconds(Transform t, float time)
        {
            // disable
            t.gameObject.SetActive(false);
            // start coroutine
            StartCoroutine(_DisableForSeconds(t, time));
        }

        private IEnumerator _DisableForSeconds(Transform t, float time)
        {
            // yield
            yield return new WaitForSeconds(time);
            // activate
            t.gameObject.SetActive(t);
        }
    }
}