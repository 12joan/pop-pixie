﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level2Started : MonoBehaviour, IDialogueSequenceEventHandler {

  public DialogueManager Dialogue;
  public ScreenFade Fader;
  public Camera MainCamera, CutsceneCamera;
  public MentoeRunToElevator MentoeAnimation;

  private int DialogueCount;

	// Use this for initialization
	void Start () {
    Fader.Fade("from black", 2.0f);
    StateManager.SetState( State.Playing );

    GDCall.UnlessLoad( StartCutscene );
    GDCall.IfLoad( MentoeAnimation.Skip );
  }

  public void StartCutscene() {
    MainCamera.enabled = false;
    CutsceneCamera.enabled = true;

    DialogueCount = 0;
    Dialogue.Play("Dialogue/l2d1", this);
	}

  public void SequenceFinished () {
    switch (DialogueCount) {
      case 0:
        StateManager.SetState( State.Cutscene );
        MentoeAnimation.Run();
        Invoke("PlaySecondDialogue", 3.0f);
        break;

      case 1:
        Fader.Fade("to black", 0.5f);
        Invoke("SwitchToMainCamera", 0.5f);
        break;
    }
  }

  void PlaySecondDialogue () {
    DialogueCount = 1;
    Dialogue.Play("Dialogue/l2d2", this);
  }

  void SwitchToMainCamera () {
    MainCamera.enabled = true;
    CutsceneCamera.enabled = false;

    Fader.Fade("from black", 0.5f);
  }
}
