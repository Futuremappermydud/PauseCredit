using HarmonyLib;
using IPA;
using System;
using TMPro;
using static IPA.Logging.Logger;
using IPALogger = IPA.Logging.Logger;

namespace PauseCredit
{
	[Plugin(RuntimeOptions.SingleStartInit)]
	public class Plugin
	{
		internal static Plugin Instance { get; private set; }
		internal static Harmony harmony { get; private set; } = new Harmony("com.FutureMapper.PauseCredit");
		internal static IPALogger Log { get; private set; }

		[Init]
		public Plugin(IPALogger logger)
		{
			Instance = this;
			Log = logger;
			harmony.PatchAll();
		}

		[OnExit]
		public void OnApplicationExit()
		{
			harmony.UnpatchSelf();
		}
		[HarmonyPatch(typeof(LevelBar))]
		[HarmonyPatch("Setup", new Type[] { typeof(IPreviewBeatmapLevel), typeof(BeatmapCharacteristicSO), typeof(BeatmapDifficulty) })]
		public class Patch
		{
			public static void Postfix(TextMeshProUGUI ____multiLineAuthorNameText, TextMeshProUGUI ____authorNameText, bool ____showSongSubName, IPreviewBeatmapLevel previewBeatmapLevel)
			{
				if (____showSongSubName && previewBeatmapLevel.songSubName.Length > 0)
				{
					____multiLineAuthorNameText.richText = true;
					____multiLineAuthorNameText.text = $"<size=80%>{previewBeatmapLevel.songAuthorName}</size> <size=90%>[<color=#89ff89>{previewBeatmapLevel.levelAuthorName.Replace(@"<", "<\u200B").Replace(@">", ">\u200B")}</color>]</size>";
				}
				else
				{
					____authorNameText.richText = true;
					____authorNameText.text = $"<size=80%>{previewBeatmapLevel.songAuthorName}</size> <size=90%>[<color=#89ff89>{previewBeatmapLevel.levelAuthorName.Replace(@"<", "<\u200B").Replace(@">", ">\u200B")}</color>]</size>";
				}
			}
		}
	}
}
