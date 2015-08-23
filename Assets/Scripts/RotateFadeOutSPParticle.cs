﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class RotateFadeOutSPParticle  : MonoBehaviour, SPParticle {

	public static RotateFadeOutSPParticle cons(GameObject proto) {
		GameObject neu_obj = Util.proto_clone(proto);
		RotateFadeOutSPParticle rtv = neu_obj.GetComponent<RotateFadeOutSPParticle>();
		return rtv.i_cons();
	}

	private float _ct, _ctmax;
	public float _vr;
	public float _scmin, _scmax;
	public Vector2 _alpha;
	public Vector2 _velocity;
	public float _gravity;

	private float _self_rotation;

	private SpriteRenderer _renderer;

	private RotateFadeOutSPParticle i_cons() {
		_scmin = 1;
		_scmax = 1;
		_velocity = Vector2.zero;
		_gravity = 0;
		_alpha = new Vector2(1,0);

		this.set_ctmax(50);
		_renderer = this.GetComponent<SpriteRenderer>();
		return this;
	}

	public void set_ctmax(float val) {
		_ct = _ctmax = val;
	}

	public void i_update(Object context) {
		float pct = _ct/_ctmax;
		_ct -= Util.dt_scale;
		this.set_opacity(Util.lerp(_alpha.x,_alpha.y,1-pct));
		_self_rotation +=  _vr * Util.dt_scale;
		this.set_rotation(_self_rotation);
		this.set_scale(Util.lerp(_scmin,_scmax,pct));
		_velocity.y -= _gravity * Util.dt_scale;
		this.set_position(
			transform.localPosition.x + _velocity.x * Util.dt_scale,
			transform.localPosition.y + _velocity.y * Util.dt_scale
		);
	}

	private void set_opacity(float val) {
		Color c = _renderer.color;
		c.a = val;
		_renderer.color = c;

	}
	private void set_rotation(float val) {
		transform.localEulerAngles = new Vector3(0,0,val);
	}
	private void set_scale(float val) {
		transform.localScale = Util.valv(val);
	}
	private void set_position(float x, float y) {
		transform.localPosition = new Vector3(x,y,transform.localPosition.z);
	}

	public bool should_remove(Object context) {
		return _ct <= 0;
	}
	
	public void do_remove(Object context) {
		Destroy(this.gameObject);
	}

}
/*
	RotateFadeOutParticle *particle = [RotateFadeOutParticle cons_tex:[Resource get_tex:TEX_GAMEPLAY_ELEMENTS] rect:[FileCache get_cgrect_from_plist:TEX_GAMEPLAY_ELEMENTS idname:@"vfx_blood.png"]];

[g add_particle:particle];
*/
