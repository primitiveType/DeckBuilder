using System.Collections;
using App.Utility;
using UnityEngine;

namespace App
{
    public class CoroutineManager : MonoBehaviourSingleton<CoroutineManager>
    {
        public Coroutine RunCoroutine(IEnumerator cr)
        {
            return StartCoroutine(cr);
        }

        public void EndCoroutine(Coroutine cr)
        {
            if (cr != null)
            {
                StopCoroutine(cr);
            }
        }
        
    }
}