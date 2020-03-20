using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class WrappedInput : MonoBehaviour {

  public static bool GetButton( string buttonName ) {
    GamePadButton button = GamePadButtonData.GetButton( buttonName );
    if ( button != null && button.GetButton() ) return true;
    return Input.GetButton( KeyboardButtonName(buttonName) );
  }

  public static bool GetButtonDown( string buttonName ) {
    GamePadButton button = GamePadButtonData.GetButton( buttonName );
    // Debug.Log(buttonName);
    // Debug.Log(button);
    if ( button != null && button.GetButtonDown() ) return true;
    return Input.GetButtonDown( KeyboardButtonName(buttonName) );
  }

  public static float GetAxis( string axis ) {
    float amount;

    if ( axis == "Horizontal" || axis == "Vertical" ) {
      amount = Input.GetAxis( axis );
    } else {
      amount = 0;
    }

    String controllerInput = GamePadAxisData.GetInput( axis );
    int controllerSign = GamePadAxisData.GetSign( axis );

    if ( controllerInput != null ) {
      float controllerAmount = Input.GetAxis( controllerInput ) * controllerSign;

      if ( Mathf.Abs( controllerAmount ) > Mathf.Abs(amount) )
        amount = controllerAmount;
    }

    return amount;
  }

  static string KeyboardButtonName( string button ) {
    return "Kb+M " + button;
  }

  public static string ControllerPrefix() {
    if ( Input.GetJoystickNames().Length > 0 ) {
      return ControllerTypeData.GetType();
    } else {
      return null;
    }
  }

}
