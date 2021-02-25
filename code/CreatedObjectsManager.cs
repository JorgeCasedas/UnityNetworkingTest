using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatedObjectsManager : MonoBehaviour
{

    public static CreatedObjectsManager Instance;

    public GameObject bulletPrefab;

    List<string> newBulletsPos;
    List<string> newBulletsRot;

    private void Awake ()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy (this);
    }
    // Start is called before the first frame update
    void Start()
    {
        newBulletsPos = new List<string> ();
        newBulletsRot = new List<string> ();
    }

    // Update is called once per frame
    void Update()
    {
        if (newBulletsPos.Count > 0)
        {
            int i = 0;
            while (i < newBulletsPos.Count)
            {
                GameObject bullet = Instantiate (bulletPrefab, JsonUtility.FromJson<Vector3> (newBulletsPos[i]), Quaternion.Euler (JsonUtility.FromJson<Vector3> (newBulletsRot[i])));
                i++;
            }
            newBulletsPos.Clear ();
            newBulletsRot.Clear ();
        }
    }

    public void CreateBullet (string pos, string rot)
    {
        newBulletsPos.Add (pos);
        newBulletsRot.Add (rot);
        //JsonUtility.FromJson<Vector3> (pos), JsonUtility.FromJson<Vector3> (rot)
        //GameObject bullet = Instantiate (bulletPrefab, pos, Quaternion.Euler(rot));
    }

}
