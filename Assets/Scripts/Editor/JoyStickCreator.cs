using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
[CreateAssetMenu(fileName = "JoystickCreator")]
public class JoystickCreator : ScriptableObject
{
    [Header("General")]
    [SerializeField]
    private int _numJoysticks = 4;
    [SerializeField]
    private int _ignoreCount = 9;

    [Header("Thumbstick")]
    [SerializeField]
    private float _axisSensitivity;
    [SerializeField]
    private float _axisGravity;
    [SerializeField]
    private float _axisDead;
    [SerializeField]
    private bool _invertY;
    [SerializeField]
    private bool _invertX;

    [Header("Buttons")]
    [SerializeField]
    private string _moveAxisXName;
    [SerializeField]
    private string _moveAxisYName;
    [SerializeField]
    private string[] _buttonNames;
    public string[] GetButtonNames { get { return _buttonNames; } }

    [Header("Keyboard")]
    [SerializeField]
    private string[] _keyboardButtons;
    [SerializeField]
    private string _keyHorizontalPos;
    [SerializeField]
    private string _keyHorizontalNeg;
    [SerializeField]
    private string _keyVerticalPos;
    [SerializeField]
    private string _keyVerticalNeg;


    [ContextMenu("UpdateInput")]
    public void Init()
    {
        if (_buttonNames.Length == _keyboardButtons.Length)
        {
            List<Axis> allAxises = new List<Axis>();

            CreateJoystickAxises(allAxises);
            CreateKeyboardAxises(allAxises);

#if UNITY_EDITOR
            UnityInputChanger.WriteInput(_ignoreCount, allAxises);
#endif
        }

        else
        {
            Debug.LogError("The number of KeyboardButtons must match the number of ButtonNames!");
        }

    }

    private void CreateKeyboardAxises(List<Axis> allAxises)
    {
        allAxises.Add(Axis.CreateKeyboardAxis(_moveAxisXName + _numJoysticks, _keyHorizontalPos, _keyHorizontalNeg, _axisGravity, _axisDead, _axisSensitivity));
        allAxises.Add(Axis.CreateKeyboardAxis(_moveAxisYName + _numJoysticks, _keyVerticalPos, _keyVerticalNeg, _axisGravity, _axisDead, _axisSensitivity));

        for (int i = 0; i < _buttonNames.Length; i++)
            allAxises.Add(Axis.CreateButton(_buttonNames[i] + _numJoysticks, _keyboardButtons[i]));
    }

    private void CreateJoystickAxises(List<Axis> axises)
    {
        for (int i = 0; i < _numJoysticks; i++)
        {
            const int X_AXIS = 1;
            string name = _moveAxisXName + i;
            axises.Add(Axis.CreateJoystickStick(name, X_AXIS, i + 1, _axisGravity, _axisDead, _axisSensitivity, _invertX));
        }

        for (int i = 0; i < _numJoysticks; i++)
        {
            const int Y_AXIS = 2;
            string name = _moveAxisYName + i;
            axises.Add(Axis.CreateJoystickStick(name, Y_AXIS, i + 1, _axisGravity, _axisDead, _axisSensitivity, _invertY));
        }

        for (int i = 0; i < _buttonNames.Length * _numJoysticks; i++)
        {
            int buttonId = i % _buttonNames.Length;
            int joyId = i / _buttonNames.Length;
            string name = string.Format("joystick {0} button {1}", joyId + 1, buttonId);
            axises.Add(Axis.CreateButton(_buttonNames[buttonId] + joyId, name));
        }
    }

    [System.Serializable]
    public class Axis
    {
        public enum Type
        {
            KeyOrMouseButton = 0,
            MouseMovement = 1,
            JoystickAxis = 2
        };

        public string name = "";
        public string descriptiveName = "";
        public string descriptiveNegativeName = "";
        public string negativeButton = "";
        public string positiveButton = "";
        public string altNegativeButton = "";
        public string altPositiveButton = "";

        public float gravity = 0.0f;
        public float dead = 0.0f;
        public float sensitivity = 0.1f;

        public bool snap = false;
        public bool invert = false;

        public Type axisType = Type.KeyOrMouseButton;

        public int axis = 0;
        public int joyNum = 0;

        public static Axis CreateKeyboardAxis(string name, string posButton, string negButton, float gravity, float dead, float sensitivity)
        {
            Axis result = new Axis();

            result.name = name;
            result.positiveButton = posButton;
            result.negativeButton = negButton;
            result.axisType = Type.KeyOrMouseButton;
            result.gravity = gravity;
            result.dead = dead;
            result.sensitivity = sensitivity;

            return result;
        }

        public static Axis CreateJoystickStick(string name, int axis, int joyNum, float gravity, float dead, float sensitivity, bool invert)
        {
            Axis result = new Axis();

            result.name = name;
            result.axisType = Type.JoystickAxis;
            result.axis = axis;
            result.joyNum = joyNum;
            result.gravity = gravity;
            result.dead = dead;
            result.sensitivity = sensitivity;
            result.invert = invert;

            return result;
        }

        public static Axis CreateButton(string name, string posPutton)
        {
            Axis result = new Axis();
            result.name = name;
            result.positiveButton = posPutton;
            return result;
        }
    }

}


//public bool JoystickButtonClick(JoystickButton button, int joyId)
//{
//    return JoystickButtonClick((int)button, joyId);
//}

//public bool JoystickButtonPress(JoystickButton button, int joyId)
//{
//    return JoystickButtonPress((int)button, joyId);
//}

//public bool JoystickButtonRelease(JoystickButton button, int joyId)
//{
//    return JoystickButtonRelease((int)button, joyId);
//}

///// <summary> Button and joyId start at 0! </summary>
//public bool JoystickButtonClick(int button, int joyId)
//{
//    return Input.GetKeyDown(_mappings.GetButtonName(this, joyId, button));
//}

///// <summary> Button and joyId start at 0! </summary>
//public bool JoystickButtonPress(int button, int joyId)
//{
//    return Input.GetKey(_mappings.GetButtonName(this, joyId, button));
//}

///// <summary> Button and joyId start at 0! </summary>
//public bool JoystickButtonRelease(int button, int joyId)
//{
//    return Input.GetKeyUp(_mappings.GetButtonName(this, joyId, button));
//}

///// <summary> The id starts at 0! </summary>
//public Vector2 GetJoystickAxis(int id)
//{
//    Vector2 axis = new Vector2(  Input.GetAxis(_mappings._xAxis[id]), 
//                                -Input.GetAxis(_mappings._yAxis[id]));

//    float squared = axis.sqrMagnitude;

//    if (squared <= _joystickDeadzone * _joystickDeadzone)
//    {
//        axis.x = 0.0f;
//        axis.y = 0.0f;
//    }


//    else if (squared > 1.0f)
//        axis.Normalize();


//    return axis;
//}
