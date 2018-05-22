using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[HelpURL("https://www.google.be/url?sa=t&rct=j&q=&esrc=s&source=web&cd=2&cad=rja&uact=8&ved=0ahUKEwjI5pjQ9NvVAhXC2RoKHXorCtUQtwIIMzAB&url=https%3A%2F%2Fwww.youtube.com%2Fwatch%3Fv%3DdQw4w9WgXcQ&usg=AFQjCNEPGa2VKuL0GefK_nkQoh9csTD8OA")]
public class RotateWithMouse : MonoBehaviour
{


    [Header("Params")]
    [Tooltip("Affected Camera to Move")]
    [SerializeField]
    private Transform _target;

    public void SetRootToRotate(Transform transform)
    {
        _target = transform;
    }

    [Header("Rotation Speed by second")]
    [SerializeField]
    [Tooltip("In degree by second")]
    private float _horizontalSpeed = 360;

    [SerializeField]
    [Tooltip("In degree by second")]
    private float _verticalSpeed = 180;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="horizontal">In Degree by second</param>
    /// <param name="vertical">In Degree by second</param>
    public void SetRotationSpeed(float horizontal, float vertical)
    {
        horizontal = Mathf.Abs(horizontal);
        vertical = Mathf.Abs(vertical);

        _horizontalSpeed = horizontal;
        _verticalSpeed = vertical;
    }

    [Header("Clamp rotation")]
    [Tooltip("Best:90°  Max:180° - Allow to clamp the view verticaly.")]
    [Range(0, 180)]
    [SerializeField]
    private float _verticalClamp = 90;
    public void SetVerticalClamp(float value)
    {
        _verticalClamp = Mathf.Clamp(_verticalClamp, 0f, 180);
    }



    [Header("Where to use script")]
    [Tooltip("Is the script is allow to run in the editor?")]
    [SerializeField]
    private bool _allowToBeUsedInEditor = true;
    [Tooltip("Is the script is allow to run in the application?")]
    [SerializeField]
    private bool _allowToBeUsedOutOfEditor = false;

    [Header("Debug (Do not touch)")]
    [SerializeField]
    private Vector3 _rotateAround;



    #region INSPECTOR METHODE
    public void Reset()
    {
        SetTargetIfNull();

    }
    public void OnValidate()
    {
        SetVerticalClamp(_verticalClamp);

    }
    #endregion

    #region UNITY METHODE
    private void Awake()
    {
        if (_target == null)
        {
            Debug.LogError("The target must not be null !. Object will be destroy.", this.gameObject);
            Destroy(this);
        }
    }
    void Update()
    {

        if (IsAllowToRotate())
        {
            _rotateAround.x += Input.GetAxis("Mouse X") * _horizontalSpeed * Time.deltaTime;
            _rotateAround.y += Input.GetAxis("Mouse Y") * _verticalSpeed * Time.deltaTime;
            _rotateAround.y = Mathf.Clamp(_rotateAround.y, -_verticalClamp, _verticalClamp);

            _target.localRotation = Quaternion.Euler(new Vector3(-_rotateAround.y, _rotateAround.x, 0));
        }
    }
    #endregion

    #region PUBLIC METHODE
    public void ResetDirection()
    {
        _rotateAround = Vector3.zero;
    }

    public bool IsAllowToRotate()
    {
#if !UNITY_EDITOR
                    return  _allowToBeUsedOutOfEditor;
#endif
        return _allowToBeUsedInEditor;
    }
    public void SetTargetIfNull()
    {
        if (_target == null)
            _target = transform;

    }
    #endregion


}