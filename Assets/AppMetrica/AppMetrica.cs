/*
 * Version for Unity
 * © 2015-2020 YANDEX
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 * https://yandex.com/legal/appmetrica_sdk_agreement/
 */

using UnityEngine;

public class AppMetrica : MonoBehaviour
{
	public const string VERSION = "5.2.0";

	private static bool s_isInitialized;

	private static IYandexAppMetrica s_metrica;
	private static readonly object s_syncRoot = new Object();

	[SerializeField] private string ApiKey;

	[SerializeField] private bool ExceptionsReporting = true;

	[SerializeField] private uint SessionTimeoutSec = 10;

	[SerializeField] private bool LocationTracking = false;

	[SerializeField] private bool Logs = false;

	[SerializeField] private bool HandleFirstActivationAsUpdate = false;

	[SerializeField] private bool StatisticsSending = true;

	private bool _actualPauseStatus;

	public static IYandexAppMetrica Instance
	{
		get
		{
			if (s_metrica == null)
			{
				lock (s_syncRoot)
				{
#if UNITY_IPHONE || UNITY_IOS
                    if (s_metrica == null && Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        s_metrica = new YandexAppMetricaIOS();
                    }
#elif UNITY_ANDROID
					if (s_metrica == null && Application.platform == RuntimePlatform.Android)
					{
						s_metrica = new YandexAppMetricaAndroid();
					}
#endif
					if (s_metrica == null)
					{
						s_metrica = new YandexAppMetricaDummy();
					}
				}
			}

			return s_metrica;
		}
	}

	private void Awake()
	{
		if (!s_isInitialized)
		{
			s_isInitialized = true;
			SetupMetrica();
		}
		else
		{
			Destroy(gameObject);
		}
	}

	private void Start()
	{
		Instance.ResumeSession();
	}

	private void OnEnable()
	{
		if (ExceptionsReporting)
		{
#if UNITY_5 || UNITY_5_3_OR_NEWER
			Application.logMessageReceived += HandleLog;
#else
			Application.RegisterLogCallback(HandleLog);
#endif
		}
	}

	private void OnDisable()
	{
		if (ExceptionsReporting)
		{
#if UNITY_5 || UNITY_5_3_OR_NEWER
			Application.logMessageReceived -= HandleLog;
#else
			Application.RegisterLogCallback(null);
#endif
		}
	}

	private void OnApplicationPause(bool pauseStatus)
	{
		if (_actualPauseStatus != pauseStatus)
		{
			_actualPauseStatus = pauseStatus;
			if (pauseStatus)
			{
				Instance.PauseSession();
			}
			else
			{
				Instance.ResumeSession();
			}
		}
	}

	private void SetupMetrica()
	{
		YandexAppMetricaConfig configuration = new YandexAppMetricaConfig(ApiKey)
		{
			SessionTimeout = (int)SessionTimeoutSec,
			Logs = Logs,
			HandleFirstActivationAsUpdate = HandleFirstActivationAsUpdate,
			StatisticsSending = StatisticsSending,
			LocationTracking = LocationTracking
		};

		Instance.ActivateWithConfiguration(configuration);
	}

	private static void HandleLog(string condition, string stackTrace, LogType type)
	{
		if (type == LogType.Exception)
		{
			Instance.ReportErrorFromLogCallback(condition, stackTrace);
		}
	}
}