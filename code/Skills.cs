using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class made for testing purposes, not being used in the actual gameplay
/// </summary>
public class Skills : MonoBehaviour
{
    public float hp;
    //ProgrammerAnims prAnims;
    public GameObject bulletPrefab;
    public Transform origin;
    // Start is called before the first frame update
    void Start()
    {
        hp = 100;
        //prAnims = GetComponent<ProgrammerAnims>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) /*&& prAnims.pcOn*/)
        {
            GameObject bullet = Instantiate(bulletPrefab, origin.position, bulletPrefab.transform.rotation);
            bullet.transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + bullet.transform.rotation.eulerAngles);
            InfoSender.Instance.SpawnBullet (origin.position, bullet.transform.rotation.eulerAngles);
        }
    }
    private void OnTriggerEnter (Collider other)
    {
        if (other.tag == "projectile")
        {
            hp -= 13;
            ChangeHp ();
        }
    }

    void ChangeHp ()
    {
        InfoManager.Instance.ChangeMyHp (hp);
        InfoSender.Instance.SendMyHp (hp);
    }

}
