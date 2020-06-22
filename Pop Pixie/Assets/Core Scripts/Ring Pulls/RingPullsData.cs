using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RingPullsData {

  public static bool ShouldPulse = false;

  public static int Amount() {
    return GameData.Current.Fetch(
      "ring-pulls", 
      orSetEqualTo: 0
    );
  }

  public static void SetAmount(int amount) {
    GameData.Current.Set("ring-pulls", amount);
  }

  public static void Increment() {
    SetAmount( Amount() + 1 );
  }

}
