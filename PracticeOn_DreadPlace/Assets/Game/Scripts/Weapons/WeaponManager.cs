using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public InputActionReference Fire;
    public InputActionReference Reload;
    public InputActionReference NextWeapon;
    public InputActionReference PreviousWeapon;
    public InputActionReference Scroll;

    public List<WeaponController> Weapons;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        SelectWeapon(0);
    }

    void Update()
    {
        HandleFire();
        HandleReload();
        WeaponNext();
        WeaponPrevious();
    }

    void HandleFire()
    {
        if (Fire.action.triggered)
            Weapons[_currentWeaponIndex].TryShoot();
    }

    void HandleReload()
    {
        if (Reload.action.triggered)
            Weapons[_currentWeaponIndex].Reload();
    }

    private void WeaponNext()
    {
        if(NextWeapon.action.triggered || Scroll.action.ReadValue<Vector2>().y > 0)
            SwitchWeapon(1);
    }

    private void WeaponPrevious()
    {
        if (PreviousWeapon.action.triggered || Scroll.action.ReadValue<Vector2>().y < 0)
            SwitchWeapon(-1);
    }

    public void SelectWeapon(int index)
    {
        for (int i = 0; i < Weapons.Count; i++)
                Weapons[i].gameObject.SetActive(i == index);
    }

    void SwitchWeapon(int direction)
    {
        _currentWeaponIndex += direction;

        if (_currentWeaponIndex < 0)
            _currentWeaponIndex = Weapons.Count - 1;

        if (_currentWeaponIndex >= Weapons.Count)
            _currentWeaponIndex = 0;

        SelectWeapon(_currentWeaponIndex);
    }
}
