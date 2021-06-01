#if !No_Reporter
using UnityEngine;
using System.Collections;


namespace TDFramework
{
	public class ReporterGUI : MonoBehaviour
	{

		Reporter reporter;
		void Awake()
		{
			reporter = gameObject.GetComponent<Reporter>();
		}

		void OnGUI()
		{
			if (reporter != null)
				reporter.OnGUIDraw();
		}
	}
}

#endif