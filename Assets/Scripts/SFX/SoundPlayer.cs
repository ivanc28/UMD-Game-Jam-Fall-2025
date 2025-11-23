using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static IEnumerator PlaySound(GameObject soundObj, Vector3 pos, float soundLength)
    {
        if (soundObj == null)
            yield break;
        GameObject sound = Instantiate(soundObj, pos, Quaternion.identity);
        yield return new WaitForSeconds(soundLength);
        Destroy(sound);
    }

}
