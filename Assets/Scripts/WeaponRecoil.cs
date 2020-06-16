using UnityEngine;
using UnityEngine.InputSystem;
public class WeaponRecoil : MonoBehaviour
{
    private WeaponStats _weaponStats;
    private Transform _camera;

    // Start is called before the first frame update
    private void Awake()
    {
        _weaponStats = GetComponent<WeaponStats>();
    }

    float calculateOffset()
    {
        return (0.5f * Mathf.Cos((_weaponStats.currentRecoil + 1f) * Mathf.PI) + 0.5f)*_weaponStats.RecoilScale;
    }

    Vector3 offsetBySpread()
    {
        float spread = _weaponStats.BulletSpread*_weaponStats.currentRecoil;
        Vector2 randomOffsets = new Vector3(Random.Range(-spread, spread), Random.Range(-spread, spread),0);
        return randomOffsets;
    }

    public Vector3 getAngleOffset()
    {
        Vector3 angleOffset = offsetBySpread();
        angleOffset.x -= calculateOffset();
        angleOffset *= 0.5f;
        return angleOffset;
    }

    public void assignCamera(Transform camera)
    {
        _camera = camera;
    }
    // Update is called once per frame
    void Update()
    {
        if (_weaponStats.currentRecoil > 0)
        {
            if ((!Mouse.current.leftButton.isPressed && _weaponStats.IsAutomatic)||_weaponStats.currentAmmo==0 || (!Mouse.current.leftButton.wasPressedThisFrame && !_weaponStats.IsAutomatic))
            {
                _camera.localRotation = Quaternion.Slerp(_camera.localRotation, Quaternion.identity, 1-_weaponStats.currentRecoil);
                _weaponStats.currentRecoil -= _weaponStats.RecoilDecay * Time.deltaTime;
            }
            
            if (_weaponStats.currentRecoil < 0)
            {
                _weaponStats.currentRecoil = 0;
            }
        }
    }
}
