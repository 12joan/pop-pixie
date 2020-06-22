﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour, IDialoguePageEventHandler {

  public DialogueBoxController DialogueBox;
  public SoundController SoundController;
  public float InterruptCooldown;
  public float SkipCooldown;

  private DialogueSequence Sequence;
  private int SequenceProgress;
  private bool DialogueBoxInProgress;
  private IDialogueSequenceEventHandler EventHandler;

  IntervalTimer SkipCooldownTimer; 
  bool ButtonReleased;

  void ReadPage (DialoguePage page) {
    DialogueBoxInProgress = true;
    DialogueBox.Write( page.Text, this );
    DialogueBox.SetFace( page.Face() );

    if ( page.HasVoiceLine() ) {
      SoundController.Play( page.VoiceLine() );
    } else {
      SoundController.Stop();
    }
  }

  public void PageFinished () {
    DialogueBoxInProgress = false;
  }

  void ReadNextPage () {
    if ( SequenceProgress < Sequence.Pages.Length ) {
      var page = Sequence.Pages[ SequenceProgress ];
      ReadPage(page);

      SequenceProgress += 1;
    } else {
      Exit();
    }
  }

  void Exit () {
    DialogueBox.Hide();
    StateManager.SetState( State.Playing );
    EventHandler.SequenceFinished();
    SoundController.Stop();
  }

	public void Play (string sequence_name, IDialogueSequenceEventHandler event_handler) {
    ButtonReleased = false;
    DialogueBoxInProgress = false;
    DialogueBox.Show();
    StateManager.SetState( State.Dialogue );

    string json = Resources.Load<TextAsset>(sequence_name).text;
    Sequence = DialogueSequence.ParseJSON(json);
    SequenceProgress = 0;

    EventHandler = event_handler;

    ReadNextPage();
	}

  void Awake () {
    DialogueBox.Hide();
  }

  void Start () {
    SkipCooldownTimer = new IntervalTimer() {
      Interval = SkipCooldown
    };

    SkipCooldownTimer.Start();
  }
	
	// Update is called once per frame
	void Update () {
    if ( StateManager.Isnt( State.Dialogue ) )
      return;

    if ( ButtonReleased && WrappedInput.GetButton("Confirm") ) {
      ButtonReleased = false;

      if (DialogueBoxInProgress) {
        DialogueBox.FinishPage();
      } else {
        ReadNextPage();
      }
    }

    ButtonReleased = ! WrappedInput.GetButton("Confirm");

    if ( WrappedInput.GetButton("Cancel") && !DialogueBoxInProgress ) {
      ReadNextPage();
    }

    if ( WrappedInput.GetButton("Skip") && SkipCooldownTimer.Elapsed() ) {
      SkipCooldownTimer.Reset();
      ReadNextPage();
      DialogueBox.FinishPage();
    }
	}
}
