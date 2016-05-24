using UnityEngine;
using System.Collections;

public class App : MonoBehaviour {

	GameObject m_ColorChangeObj = null;
	public Material m_SphereMaterial;
	public Material m_SelectMaterial;
	public Material m_CubeMaterial;
	public GameObject m_CubeObj;

	bool m_LayerMask = true;

	// Use this for initialization
	void Start () {

		int num = 10;
		for (int x = -num; x < num; ++x) {
			for (int y = -num; y < num; ++y) {
				Vector3 pos = new Vector3 (0.5f + (float)x, 0.5f + (float)y, 4.0f);
				Instantiate (m_CubeObj,
					pos,
					Quaternion.identity);
			}
		}

	}

	void ReturnMaterial()
	{
		if (m_ColorChangeObj) {
			if (m_ColorChangeObj.name == "Sphere") {
				m_ColorChangeObj.GetComponent<Renderer> ().material = m_SphereMaterial;
			} else {
				m_ColorChangeObj.GetComponent<Renderer> ().material = m_CubeMaterial;
			}
		}
		m_ColorChangeObj = null;

	}

	void OnGUI () {
		if (m_LayerMask) {
			GUI.TextArea (new Rect (10, 10, 250, 100), "Push Space Key To OFF LayerMask.\nON Now.");
		} else {
			GUI.TextArea(new Rect (10, 10, 250, 100), "Push Space Key To ON LayerMask.\nOFF Now.");
		}
	}

	// Update is called once per frame
	void Update () {
	
		if (Input.GetKeyDown (KeyCode.Space)) {
			m_LayerMask = m_LayerMask ? false : true;
		}


		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		// http://docs.unity3d.com/ja/current/Manual/Layers.html
		// ここを参考に

		// Bit shift the index of the layer (8) and layer (9) to get a bit mask
		//int layerMask = 1 << 9 | 1 << 8;

		// Bit shift the index of the layer (9) to get a bit mask
		int layerMask = 1 << 9;

		// This would cast rays only against colliders in layer 9.
		// But instead we want to collide against everything except layer 9. The ~ operator does this, it inverts a bitmask.
		layerMask = ~layerMask;

		if (!m_LayerMask) {
			layerMask = 0xfffffff;
		}

		if (!Physics.Raycast (ray, out hit, Mathf.Infinity, layerMask)) {
			ReturnMaterial ();
			return;
		}

		if (m_ColorChangeObj != hit.collider.gameObject) {
			ReturnMaterial ();
		}

		m_ColorChangeObj = hit.collider.gameObject;
		hit.collider.gameObject.GetComponent<Renderer> ().material = m_SelectMaterial;


	}
}
