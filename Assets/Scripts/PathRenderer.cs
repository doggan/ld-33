﻿using UnityEngine;
using System.Collections.Generic;

public class MultiList<TKey, TValue> {
	public Dictionary<TKey,List<TValue>> _key_to_list = new Dictionary<TKey, List<TValue>>();
	public int count_of(TKey key) {
		if (!_key_to_list.ContainsKey(key)) _key_to_list[key] = new List<TValue>();
		return _key_to_list[key].Count;
	}
	public void add(TKey key, TValue val) {
		if (!_key_to_list.ContainsKey(key)) _key_to_list[key] = new List<TValue>();
		_key_to_list[key].Add(val);
	}
	public void clear(TKey key) {
		if (!_key_to_list.ContainsKey(key)) _key_to_list[key] = new List<TValue>();
		_key_to_list[key].Clear();
	}
	public List<TValue> list(TKey key) {
		if (!_key_to_list.ContainsKey(key)) _key_to_list[key] = new List<TValue>();
		return _key_to_list[key];
	}
	public List<TKey> keys() {
		return new List<TKey>(_key_to_list.Keys);
	}
}

public class Util {
	public static System.Random rand = new System.Random();
	
	public static float rand_range(float min, float max) {
		float r = (float)rand.NextDouble();
		return (max-min)*r + min;
	}
	
	public static GameObject proto_clone(GameObject proto) {
		GameObject rtv = ((GameObject)UnityEngine.Object.Instantiate(proto));
		rtv.transform.parent = proto.transform.parent;
		rtv.transform.localScale = proto.transform.localScale;
		rtv.transform.localPosition = proto.transform.localPosition;
		rtv.transform.localRotation = proto.transform.localRotation;
		rtv.SetActive(true);
		return rtv;
	}
	
	public static string vec_to_s(Vector3 v) {
		return string.Format("({0},{1},{2})",v.x,v.y,v.z);
	}
	
	public static Vector3 valv(float x) {
		return new Vector3(x,x,x);
	}
	
	
	public static Vector3 vec_scale(Vector3 v,float f) {
		v.x *= f;
		v.y *= f;
		v.z *= f;
		return v;
	}
	
	public static float rad2deg = 57.29f;
	public static float deg2rad = 0.017f;
	
	public static void transform_set_euler_world(Transform t,Vector3 tar) {
		Quaternion q = t.rotation;
		q.eulerAngles = tar;
		t.rotation = q;
	}
	
	public static Vector3 vec_add(Vector3 a, Vector3 b) {
		Vector3 v = new Vector3();
		v.x = a.x + b.x;
		v.y = a.y + b.y;
		v.z = a.z + b.z;
		return v;
	}
	
	public static Vector3 vec_sub(Vector3 a, Vector3 b) {
		return new Vector3(a.x-b.x,a.y-b.y,a.z-b.z);
	}
	
}


public class PathRenderer : MonoBehaviour {
	
	[SerializeField] private GameObject _arrow_dot_proto;
	[SerializeField] private GameObject _arrow_head_proto;
	[SerializeField] private GameObject _path_renderer_root;
	
	private void Start () {
		_arrow_dot_proto.SetActive(false);
		_arrow_head_proto.SetActive(false);
	}
	
	//TODO -- pool me
	private MultiList<int,GameObject> _id_to_objs = new MultiList<int, GameObject>();
	public void id_draw_path(int id, Vector3 position, Vector3[] points) {
		if (_id_to_theta.ContainsKey(id)) _id_to_theta[id] = 0.0f;
		float dist_per = 15.0f;
		Vector3 last = position;
		float last_remainder = 0;
		for (int i = 0; i < points.Length; i++) {
			Vector3 itr = points[i];
			float itr_dist_total = Vector3.Distance(last,itr); 
			float itr_dist = 0;
			if (i == 1) itr_dist = dist_per;
			while (itr_dist < itr_dist_total) {
				Vector3 neu_obj_pos = Vector3.Lerp(last,itr,itr_dist/itr_dist_total);
				
				GameObject neu_obj = Util.proto_clone(_arrow_dot_proto);
				neu_obj.transform.parent = _path_renderer_root.transform;
				neu_obj.transform.position = new Vector3(neu_obj_pos.x,neu_obj_pos.y,neu_obj_pos.z);
				_id_to_objs.add(id,neu_obj);
				
				itr_dist += dist_per;
			}
			
			if (i == points.Length-1) {
				GameObject neu_obj2 = Util.proto_clone(_arrow_head_proto);
				neu_obj2.transform.parent = _path_renderer_root.transform;
				neu_obj2.transform.position = new Vector3(itr.x,itr.y,itr.z);
				_id_to_objs.add(id,neu_obj2);
				
			}
			
			last_remainder = (itr_dist_total - itr_dist);
			last = itr;
		}
	}
	
	public void clear_path(int id) {
		foreach(GameObject itr in _id_to_objs.list(id)) {
			Destroy(itr);
		}
		_id_to_objs.clear(id);
	}
	
	private Dictionary<int, float> _id_to_theta = new Dictionary<int, float>();
	public void Update() {
		foreach(int id in _id_to_objs.keys()) {
			if (_id_to_objs.count_of(id) > 0) {
				if (!_id_to_theta.ContainsKey(id)) _id_to_theta[id] = 0.0f;
				float val = _id_to_theta[id];
				List<GameObject> list = _id_to_objs.list(id);
				
				val += 0.5f * list.Count * 0.02f;
				if (val > list.Count*1.35f) val = -list.Count*0.35f;
				
				for (int i_list = 0; i_list < list.Count; i_list++) {
					SpriteRenderer itr_list = list[i_list].GetComponent<SpriteRenderer>();
					if (itr_list.sprite.name != "move_arrow_cross") {
						Color itr_list_color = itr_list.color;
						float aval = Mathf.Pow(1-(Mathf.Abs(i_list-val))/list.Count,4.0f);
						itr_list_color.a = Mathf.Max(aval,0.25f);
						itr_list.color = itr_list_color;
						itr_list.transform.localScale = Util.valv((0.75f + aval * 0.5f)*20.0f);
					}
				}
				
				_id_to_theta[id] = val;
			}
		}
	}
}