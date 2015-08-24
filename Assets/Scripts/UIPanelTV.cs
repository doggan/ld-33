﻿using UnityEngine;
using System.Collections;

public class UIPanelTV : Uzu.UiPanel {

	[SerializeField] private GameObject _MNM_logo;
	[SerializeField] private GameObject _zoomexit_target;
	[SerializeField] private GameObject _zoomexit_start;
	[SerializeField] private CameraFade _camera_fade;

	[SerializeField] private BobDirection _skele_talk_left,_skele_talk_right;

	[SerializeField] private ChatManager _chats;

	public override void OnInitialize() {
	}

	public override void OnEnter(Uzu.PanelEnterContext context) {
		gameObject.SetActive(true);
		Main.GameCamera.gameObject.SetActive(false);
		Main.Instance._tvCamera.gameObject.SetActive(true);
		_MNM_logo.SetActive(true);
		_current_mode = UIPanelTVMode.Idle;

		Main.Instance._tvCamera.transform.position = _zoomexit_start.transform.position;
		_camera_fade.set_alpha(0);
		_camera_fade.set_target_alpha(0);

		if (Main._current_repeat_reason == RepeatReason.None) {
			if (Main._current_level == GameLevel.Level1) {
				_chats.push_message("Welcome back to Monday Night Monsters!",2);
				_chats.push_message("In tonight's game, the away team...",2);
				_chats.push_message("the 2-6 Reds...",1);
				_chats.push_message("are facing off against the undefeated Blues!",2);
				_chats.push_message("(That's you, by the way...)",1);
				_chats.push_message("For all the first time viewers, let's talk controls!",2);
				_chats.push_message("(When you have the ball, click and hold to pass.)",1);
				_chats.push_message("(Hold space any time to enter time out.)",1);
				_chats.push_message("(Then, click and drag to tell your teammates what to do.)",1);
				_chats.push_message("(The goal is to get the ball in the opposing red goal.)",1);
				_chats.push_message("Kickoff's just about to begin.",2);
				_chats.push_message("Can the Blues score in the first quarter?",2);
				_chats.push_message("(You've got 5 minutes on the clock.)",1);
				_chats.push_message("Let's tune in and find out!",2);

			} else if (Main._current_level == GameLevel.Level2) {
				_chats.push_message("It's a close fought game, and we're nearing halftime.",2);
				_chats.push_message("The score's tied, with three minutes on the clock.",2);
				_chats.push_message("Can the Blues end the half on a strong note?",2);
				_chats.push_message("(You've got three minutes to score a goal.)",2);
				_chats.push_message("(You've got three minutes to score a goal.)",2);
				_chats.push_message("The players ",2);

			} else {

			}
		} else {
			if (Main._current_repeat_reason == RepeatReason.ScoredOn) {
				_chats.push_message("What a shock! The Reds broke away and scored last minute!",2);
				_chats.push_message("I can't believe it! Is this the beginning of the end for the Blues?",2);
				_chats.push_message("(As a wise man once said...)",1);
				_chats.push_message("(To score a touchdown, you've gotta move the ball to the endzone.)",1);
				_chats.push_message("(Let's try that one again.)",1);
				_chats.push_message("Let's watch an instant replay to see what just happened.",2);

			} else {
				_chats.push_message("In a absolutely SHOCKING turn of events...",2);
				_chats.push_message("The Reds defense held and allowed ZERO points!",2);
				_chats.push_message("Unbelieveable! The Red offence was first in the league in yards.",2);
				_chats.push_message("(As a wise man once said...)",1);
				_chats.push_message("(If a team doesn't put points on the board, I don't see how they can win.)",1);
				_chats.push_message("(Let's try that one again.)",1);
				_chats.push_message("Let's watch an instant replay to see what just happened.",2);
			}
		}
	}
	
	public override void OnExit(Uzu.PanelExitContext context) {
		gameObject.SetActive(false);
		Main.GameCamera.gameObject.SetActive(true);
		Main.Instance._tvCamera.gameObject.SetActive(false);
		_MNM_logo.SetActive(false);
	}

	enum UIPanelTVMode {
		Idle,
		ZoomExit
	}
	private UIPanelTVMode _current_mode;
	private float _anim_t;
	
	private void Update() {
		if (_current_mode == UIPanelTVMode.Idle) {
			if (Input.GetKey(KeyCode.Space) || (_chats._messages.Count == 0 && _chats._text_scroll.finished() && _chats._ct <= 0)) {
				_current_mode = UIPanelTVMode.ZoomExit;
				_anim_t = 0;
				_camera_fade.set_target_alpha(1);
			}
			_skele_talk_left.set_enabled(false);
			_skele_talk_right.set_enabled(false);
			if (!_chats._text_scroll.finished()) {
				if (_chats._current_id == 1) {
					_skele_talk_left.set_enabled(true);
				} else if (_chats._current_id == 2) {
					_skele_talk_right.set_enabled(true);
				}
			}

		} else if (_current_mode == UIPanelTVMode.ZoomExit) {
			_anim_t += Util.dt_scale * 0.05f;
			Main.Instance._tvCamera.transform.position = Vector3.Lerp(_zoomexit_start.transform.position,_zoomexit_target.transform.position,_anim_t);
			if (_anim_t >= 1) {
				Main.PanelManager.ChangeCurrentPanel(PanelIds.Game);
				Main.LevelController.CurrentDifficulty = Difficulty.Normal;
				Main.LevelController.StartLevel(LevelController.StartMode.Sequence);

			}
		}
	}
}