using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;



namespace TDFramework
{
	/// <summary>
	/// 日志管理器
	/// </summary>
	public class LoggerManager : ManagerBase, IDisposable
	{
		private List<string> m_LogArray;

		private string m_LogPath = null;
		private string m_ReporterPath = Application.streamingAssetsPath + "//Reporter";
		private int m_LogMaxCapacity = 500;
		private int m_CurrLogCount = 0;
		private int m_LogBufferMaxNumber = 10;

		internal override void Init()
		{
			m_LogArray = new List<string>();

			if (string.IsNullOrEmpty(m_LogPath))
			{
				m_LogPath = m_ReporterPath + "//" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + "-Start.txt";
			}
		}

		public void Write(string writeFileData, LogType type,DebugColoyType colorType=DebugColoyType.White,int size=13)
		{
			if (m_CurrLogCount >= m_LogMaxCapacity)
			{
				m_LogPath = m_ReporterPath + "//" + DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss") + ".txt";
				m_LogMaxCapacity = 0;
			}
			m_CurrLogCount++;

			if (!string.IsNullOrEmpty(writeFileData))
			{
				writeFileData = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss:fff") + "|" + type.ToString() + "|" + writeFileData + "\r\n";
				AppendDataToFile(writeFileData);
			}

#if UNITY_EDITOR
			if (type == LogType.Error || type == LogType.Exception)
				GameEntry.Debug.LogError(writeFileData, size);
			else if (type == LogType.Warning)
				GameEntry.Debug.LogWarning(writeFileData, size);
			else
				GameEntry.Debug.Log(writeFileData, colorType, size);
#endif
		}

		#region AppendDataToFile
		private void AppendDataToFile(string writeFileDate)
		{
			if (!string.IsNullOrEmpty(writeFileDate))
			{
				m_LogArray.Add(writeFileDate);
			}

			if (m_LogArray.Count % m_LogBufferMaxNumber == 0)
			{
				SyncLog();
			}
		}
		#endregion

		#region CreateFile
		private void CreateFile(string pathAndName, string info)
		{
			if (!Directory.Exists(m_ReporterPath)) Directory.CreateDirectory(m_ReporterPath);

			StreamWriter sw;
			FileInfo t = new FileInfo(pathAndName);
			if (!t.Exists)
			{
				sw = t.CreateText();
			}
			else
			{
				sw = t.AppendText();
			}

			sw.WriteLine(info);

			sw.Close();

			sw.Dispose();
		}
		#endregion

		#region ClearLogArray
		private void ClearLogArray()
		{
			if (m_LogArray != null)
			{
				m_LogArray.Clear();
			}
		}
		#endregion

		#region SyncLog
		public void SyncLog()
		{
			if (!string.IsNullOrEmpty(m_LogPath))
			{
				int len = m_LogArray.Count;
				for (int i = 0; i < len; i++)
				{
					CreateFile(m_LogPath, m_LogArray[i]);
				}
				ClearLogArray();
			}
		}
		#endregion

		public void Dispose()
		{
			m_LogArray.Clear();
		}
	}

}
