﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueBoxController : MonoBehaviour {

  public Text TextBox;
  public float InitialDelay;
  public float WriteDelay;
  public IDialogueEventHandler EventHandler;

  private string FullText;
  private int WriteProgress;

  public void Write (string text) {
    FullText = text;
    WriteProgress = 0;
    DirectWrite("");

    Interrupt();
    InvokeRepeating(
      "WriteNextLetter",
      InitialDelay,
      WriteDelay
    );
  }

  void WriteNextLetter () {
    if ( WriteProgress == FullText.Length ) {
      Interrupt();
      EventHandler.PageFinished();
      return;
    }

    WriteProgress += 1;

    DirectWrite(
      FullText.Substring(
        0,
        WriteProgress
      )
    );
  }

  void DirectWrite (string text) {
    TextBox.text = text;
  }

  public void Interrupt () {
    CancelInvoke();
  }

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	}
}
