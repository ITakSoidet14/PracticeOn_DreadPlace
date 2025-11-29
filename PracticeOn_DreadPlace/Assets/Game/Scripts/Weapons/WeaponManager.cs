using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponManager : MonoBehaviour
{
    public GameObject WeaponHolder;
    public Camera MainCamera;
    public Camera WeaponCamera;

    public InputActionReference Fire;
    public InputActionReference Reload;
    public InputActionReference NextWeapon;
    public InputActionReference PreviousWeapon;
    public InputActionReference Scroll;
    public InputActionReference Aiming;

    public List<WeaponControllerBase> Weapons;
    private int _currentWeaponIndex = 0;

    private void Start()
    {
        SelectWeapon(0);
    }

    void Update()
    {
        HandleFire();
        HandleReload();
        HandleAiming();
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

    private void HandleAiming()
    {
        if (Aiming.action.IsPressed())
        {
            WeaponHolderMoveX(0, 0.1f);

            ChangeCameraFov(35, 0.2f);
        }
        else
        {
            WeaponHolderMoveX(0.4f, 0.1f);

            ChangeCameraFov(60, 0.2f);
        }
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

    private void SwitchWeapon(int direction)
    {
        _currentWeaponIndex += direction;

        if (_currentWeaponIndex < 0)
            _currentWeaponIndex = Weapons.Count - 1;

        if (_currentWeaponIndex >= Weapons.Count)
            _currentWeaponIndex = 0;

        SelectWeapon(_currentWeaponIndex);
    }

    private void WeaponHolderMoveX(float endValue, float duration) => 
        WeaponHolder.transform.DOLocalMoveX(endValue, duration).SetEase(Ease.Linear);

    private void ChangeCameraFov(float endValue, float duration)
    {
        MainCamera.DOFieldOfView(endValue, duration).SetEase(Ease.Linear);
        WeaponCamera.DOFieldOfView(endValue, duration).SetEase(Ease.Linear);
    }
}
