using UnityEngine;

/**
 * Responsible for controlling the AI of a single bot.
 */
[RequireComponent (typeof (Steering))]
public class BotBase : MonoBehaviour {
	private FiniteStateMachine<BotBase> _FSM;
	private Steering _steering;
	
	public FiniteStateMachine<BotBase> DBG_FSM {
		get { return _FSM; }
	}
	
	public TeamBase Team {
		get; set;
	}
	
	public Steering Steering {
		get { return _steering; }
	}
	
	public void ChangeState(FSMState<BotBase> e) {
		_FSM.ChangeState(e);		
	}
	
	public Vector3 HomePosition {
		get; set;
	}
	
	public void GoToRegion(int region) {
		HomePosition = Main.FieldController.GetRegionPosition(region);
		ChangeState(BotState_GoHome.Instance);
	}
	
	public bool IsAtHomePosition() {
		return true;
	}
	
	public void GetGoalpostPositions(out Vector3 top, out Vector3 bottom) {
		top = Vector3.zero;
		bottom = Vector3.zero;
		// top = topPost.position;
		// bottom = bottomPost.position;
	}
	
	public Vector3 GetBallPosition() {
		return GameObject.Find("Mover").transform.position;
	}
	
	public void Awake() {
		_FSM = new FiniteStateMachine<BotBase>();
		_steering = GetComponent<Steering>();
		_FSM.Configure(this, BotState_Idle.Instance);
	}
 
	public void Update() {
		_FSM.Update();
	}
}


