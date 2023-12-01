using UnityEngine;

public class HandgunWeapon : Weapon
{
    [SerializeField]
    private Bullet bulletPrefab;

    [SerializeField]
    private Transform canon;
    
    private bool _readyToShoot;
    public bool ReadyToShoot => _readyToShoot;

    private bool allowInvoke;
    private float timeBetweenShooting;
    
    private void Start()
    {
        _readyToShoot = true;
        allowInvoke = true;
        timeBetweenShooting = 1.2f;
    }
    
    public void Shoot(Vector3 mouseWorldPosition)
    {
        if (_readyToShoot)
        {
            _readyToShoot = false;
            
            Vector3 aimDirection = (mouseWorldPosition - canon.position).normalized;
            var bullet = Instantiate(bulletPrefab, canon.position, Quaternion.LookRotation(aimDirection, Vector3.up));
            bullet.SetDamage(Damage);

            if (allowInvoke)
            {
                Invoke("ResetShot", timeBetweenShooting);
                allowInvoke = false;
            }
        }
    }

    private void ResetShot()
    {
        _readyToShoot = true;
        allowInvoke = true;
    }
    
    public override void Equip(Character character)
    {
        if (character.HandgunSocket)
        {
            transform.parent = character.HandgunSocket;
            transform.position = character.HandgunSocket.position;
            transform.rotation = character.HandgunSocket.rotation;
            character.CharacterState = CharacterState.Equipped_HandGun;
        }
    }
}
