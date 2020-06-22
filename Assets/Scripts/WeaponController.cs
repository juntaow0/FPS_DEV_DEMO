using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem _muzzleFlash;
    [SerializeField]
    private ParticleSystem _shell;
    [SerializeField]
    private Transform _firePoint;

    private WeaponStats _weaponStats;
    private Animator _animator;
    private ScopeIn _scopeFunction;
    private AimDownSight _ADSFunction;
    private WeaponRecoil _weaponRecoil;
    private Player _player;

    private bool isADS = false;

    private void Awake()
    {
        _weaponStats = GetComponent<WeaponStats>();
        if (_weaponStats == null)
        {
            Debug.LogError("No Weapon Stats!");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogError("No Animator!");
        }
        assignSecondaryFunction();
        _weaponRecoil = GetComponent<WeaponRecoil>();
    }
    public void reload()
    {
        if (canReload())
        {
            stopSecondaryFunction();
            _weaponStats.isReloading = true;
            _animator.SetBool("isReloading", true);
            StartCoroutine(ReloadRoutine());
        }
    }
    IEnumerator ReloadRoutine()
    {
        int feedAmount = _weaponStats.MagSize - _weaponStats.currentAmmo;
        bool emptyMag = MagEmpty(feedAmount);
        yield return new WaitForSeconds(_weaponStats.ReloadTime - 0.25f);

        _animator.SetBool("isScoped", false);
        _animator.SetBool("isAds", false);
        _animator.SetBool("isReloading", false);

        yield return new WaitForSeconds(0.25f);
        _weaponStats.isReloading = false;
        if (emptyMag)
        {
            if (_weaponStats.reserveAmmo >= _weaponStats.MagSize)
            {
                _weaponStats.currentAmmo = _weaponStats.MagSize;
                _weaponStats.reserveAmmo -= _weaponStats.MagSize;
            }
            else
            {
                _weaponStats.currentAmmo = _weaponStats.reserveAmmo;
                _weaponStats.reserveAmmo = 0;
            }
        }
        else
        {
            if (_weaponStats.reserveAmmo >= feedAmount)
            {
                _weaponStats.currentAmmo = _weaponStats.MagSize;
                _weaponStats.reserveAmmo -= feedAmount;
            }
            else
            {
                _weaponStats.currentAmmo += _weaponStats.reserveAmmo;
                _weaponStats.reserveAmmo = 0;
            }
        }
        UIManager.instance.updateCurrentAmmo(_weaponStats.currentAmmo);
        UIManager.instance.updateReserveAmmo(_weaponStats.reserveAmmo);
    }
    bool magNotFull()
    {
        return _weaponStats.currentAmmo < _weaponStats.MagSize;
    }
    bool hasAmmo()
    {
        return _weaponStats.reserveAmmo > 0;
    }
    bool canReload()
    {
        return magNotFull() && hasAmmo();
    }
    bool MagEmpty(int feedAmount)
    {
        return feedAmount == _weaponStats.MagSize;
    }
    bool canFire()
    {
        return (Time.time >= _weaponStats.nextFireTime) && (_weaponStats.currentAmmo > 0);
    }
    void FireFunction()
    {
        _muzzleFlash.Play();
        _shell.Play();
        _weaponStats.currentAmmo--;
        UIManager.instance.updateCurrentAmmo(_weaponStats.currentAmmo);
        _animator.SetTrigger("shoot");
        Vector3 offsetAngle = _weaponRecoil.getAngleOffset();
        Vector3 rayCastDir = Quaternion.AngleAxis(offsetAngle.x, _firePoint.right) * Quaternion.AngleAxis(offsetAngle.y, _firePoint.up) * _firePoint.forward;

        RaycastHit hit;
        if (Physics.Raycast(_firePoint.position, rayCastDir, out hit, _weaponStats.EffectiveRange))
        {
            // add interface for taking damange
            if (hit.transform.tag == "Enemy")
            {
                Enemy enemy = hit.transform.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.takeDamage(_weaponStats.Damage);
                }
            }
            else if (hit.transform.tag == "Player")
            {
                Player player = hit.transform.GetComponent<Player>();
                if (player != null)
                {
                    player.takeDamage(_weaponStats.Damage);
                }
            }
            else if (hit.transform.tag == "Target")
            {
                int score = hit.transform.GetComponent<TargetBoard>().getScore(hit.point);
                _player.addScore(score);

            }
            HitMarkerManager.instance.selectHitMarker(hit);
            GameObject bulletHole = ObjectPooler.instance.SpawnFromPool("bulletHole", hit.point+hit.normal*0.001f, Quaternion.FromToRotation(-Vector3.forward,hit.normal));
            bulletHole.transform.parent = hit.transform;

            if (hit.rigidbody != null)
            {
                Vector3 dir = _firePoint.forward;
                hit.rigidbody.AddForce(dir * 300f * Time.deltaTime, ForceMode.Impulse);
            }
        }
        _weaponStats.currentRecoil += _weaponStats.RecoilSpeed;
        _weaponStats.currentRecoil = Mathf.Clamp(_weaponStats.currentRecoil, 0, _weaponStats.MaxRecoil);
        offsetAngle = _weaponRecoil.getAngleOffset();
        _firePoint.localRotation = Quaternion.Euler(offsetAngle);
        _weaponStats.nextFireTime = Time.time + 60f/_weaponStats.FireRate;
    }
    public void Fire()
    {
        if (canFire())
        {
            FireFunction();
            if (!_weaponStats.IsADS && _scopeFunction.isUsing())
            {
                _scopeFunction.resetFunction();
                _scopeFunction.autoRescope();
            }
        }
    }
    public void secondaryFunction()
    {
        if (_weaponStats.IsADS)
        {
            _ADSFunction.useFunction();
        }
        else
        {
            _scopeFunction.useFunction();
        }
    }
    void assignSecondaryFunction()
    {
        if (_weaponStats.IsADS)
        {
            _ADSFunction = GetComponent<AimDownSight>();
            if (_ADSFunction == null)
            {
                Debug.LogError("No ADS script!");
            }
        }
        else
        {
            _scopeFunction = GetComponent<ScopeIn>();
            if (_scopeFunction == null)
            {
                Debug.LogError("No ScopeIn script!");
            }
        }
    }
    public void stopSecondaryFunction()
    {
        if (_weaponStats.IsADS)
        {
            if (_ADSFunction.isUsing())
            {
                _ADSFunction.resetFunction();
            }
        }
        else
        {
            if (_scopeFunction.isUsing())
            {
                _scopeFunction.resetFunction();
            }
        }
    }
    public void OnSwap()
    {
        _animator.enabled = true;
        _firePoint.localRotation = Quaternion.identity;
        if (!_weaponStats.IsADS)
        {
            UIManager.instance.setCrosshair(false);
        }
        else
        {
            UIManager.instance.setCrosshair(true);
        }
        UIManager.instance.updateCurrentAmmo(_weaponStats.currentAmmo);
        UIManager.instance.updateReserveAmmo(_weaponStats.reserveAmmo);
        StartCoroutine(SwapRoutine());
    }
    IEnumerator SwapRoutine()
    {
        _weaponStats.isSwapping = true;
        
        yield return new WaitForSeconds(0.4f);
        _weaponStats.isSwapping = false;
    }
    private void OnDisable()
    {
        _animator.SetBool("isReloading", false);
        _animator.enabled = false;
    }
    public void getAmmo()
    {
        if (!hasAmmo())
        {
            _weaponStats.reserveAmmo = _weaponStats.ReserveAmmoCapacity;
        }
        else
        {
            _weaponStats.reserveAmmo = _weaponStats.ReserveAmmoCapacity - _weaponStats.MagSize;
        }
        UIManager.instance.updateReserveAmmo(_weaponStats.reserveAmmo);
    }
    public void assignCamera(Camera FPSCam, Camera weaponCam)
    {
        _firePoint = FPSCam.transform;
        _weaponRecoil.assignCamera(_firePoint);
        if (_weaponStats.IsADS)
        {
            _ADSFunction.assignCameras(FPSCam);
        }
        else
        {
            _scopeFunction.assignCameras(FPSCam, weaponCam);
        }
    }
    public bool isReloading()
    {
        return _weaponStats.isReloading;
    }
    public bool isSwapping()
    {
        return _weaponStats.isSwapping;
    }
    public bool isAutomatic()
    {
        return _weaponStats.IsAutomatic;    
    }

    public bool isEmpty()
    {
        return (_weaponStats.currentAmmo == 0);
    }

    public void assignOwner(Player player)
    {
        if (_player != player || _player==null)
        {
            _player = player;
        }
    }
}