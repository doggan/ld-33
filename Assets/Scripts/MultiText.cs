﻿using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class MultiText : MonoBehaviour {

	[SerializeField] public List<Text> _texts;

	public void set_string(string text) {
		for (int i = 0; i < _texts.Count; i++) {
			_texts[i].text = text;
		}
	}

	public string get_string() {
		return _texts[0].text;
	}

}
