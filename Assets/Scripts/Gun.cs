namespace PlayoVR {
    using UnityEngine;
    using VRTK;

    public class Gun : Photon.MonoBehaviour {
        public GameObject bulletPrefab;
        public Transform bulletSpawn;
        public AudioClip fireGunSound;
        public Animation fireAnimation;
        public int ammo = 0;
        public string gunType = "handgun";
        private bool fired;

        // Use this for initialization
        void Awake() {
            GetComponent<VRTK_InteractableObject>().InteractableObjectUsed += new InteractableObjectEventHandler(DoFireGun);
        }

        void DoFireGun(object sender, InteractableObjectEventArgs e) {
            if (ammo > 0)
            {
                fired = true;
            }
        }

        // Update is called once per frame
        void Update() {
            // Handle firing
            if (fired) {
                CmdFire();
                fired = false;
                ammo--;
            }
        }

        public void NewMagazine(int magazineAmmo)
        {
            ammo = magazineAmmo;
        }

        void CmdFire() {
            // Now create the bullet and play sound/animation locally and on all other clients
            photonView.RPC("NetFire", PhotonTargets.All, bulletSpawn.position, bulletSpawn.rotation);
        }

        [PunRPC]
        void NetFire(Vector3 position, Quaternion rotation) {
            // Create the Bullet from the Bullet Prefab
            var bullet = Instantiate(
                bulletPrefab,
                position,
                rotation);
            // Play sound of gun shooting
            AudioSource.PlayClipAtPoint(fireGunSound, transform.position, 1.0f);
            // Play animation of gun shooting
            fireAnimation.Play();
        }

        public void PhotonDestroy()
        {
            PhotonNetwork.Destroy(GetComponent<PhotonView>());
        }
    }
}
