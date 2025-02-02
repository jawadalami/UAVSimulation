using System;
using System.Collections;
using Databases.ScriptableObjects;
using HelperScripts;
using Modules.Prompts;
using ScriptableObjects.EventChannels;
using TMPro;
using UI.Console.Channels.ScriptableObjects;
using Unity.VisualScripting;
using UnityEngine;

namespace UI.Console
{
	public class ConsoleTextHandler : MonoBehaviour
	{
		[SerializeField] private bool animateAllAtStart = true;
		
		[SerializeField] private TextMeshProUGUI consoleTextMeshProUGUI;

		[SerializeField] private ConsoleMessageEventChannelSO writeMessageToConsoleChannel;
		[SerializeField] public SettingsDatabaseSO settingsDatabase;
		[NonSerialized] private TypeWriterEffect _typeWriterEffect;
		[NonSerialized] private QueueManager _animationQueue;
		

		private void OnValidate()
		{
			AssertionHelper.CheckIfReferenceExistsOrComponentExistsInGameObject(consoleTextMeshProUGUI, this, this.gameObject);
		}

		private void Awake()
		{
			
			InitializeAnimationQueueAndSettings();
			if (writeMessageToConsoleChannel == null)
				writeMessageToConsoleChannel = GameManager.Instance.channelsDatabase.newConsoleMessageChannel;
			
			if (writeMessageToConsoleChannel != null)
				writeMessageToConsoleChannel.Subscribe(AddTextToConsole);
		}
		
		private void InitializeAnimationQueueAndSettings()
		{
			if (settingsDatabase == null)
				settingsDatabase = GameManager.Instance.settingsDatabase;
			
			if (!TryGetComponent(out _typeWriterEffect))

			{
				_typeWriterEffect = gameObject.AddComponent<TypeWriterEffect>();
				_typeWriterEffect.settingsDatabase = settingsDatabase;
			}
			
			_typeWriterEffect.mTextMeshPro = consoleTextMeshProUGUI;
			
			consoleTextMeshProUGUI.fontSize = settingsDatabase.promptSettings.textFontSize;
			if (!TryGetComponent(out _animationQueue))
				_animationQueue = gameObject.AddComponent<QueueManager>();
			
			if (animateAllAtStart)
				_animationQueue.AddToQueue(_typeWriterEffect.AnimateAll());
			
		}
		
		private void AddTextToConsole(string prefix, ConsoleMessage message)
		{
			var textToAdd = Environment.NewLine+ prefix + message.text;
			if(message.color != null)
				textToAdd=  TextManipulation.AddColorToText(textToAdd, message.color);
			
			var animate= message.doAnimate;
			_animationQueue.AddToQueue(animate ? _typeWriterEffect.AddAnimatedText(textToAdd) : AddTextWithoutAnimation(textToAdd));
	
		}
		
		private IEnumerator AddTextWithoutAnimation(string textToAdd)
		{
			consoleTextMeshProUGUI.text += textToAdd;
			consoleTextMeshProUGUI.maxVisibleCharacters = consoleTextMeshProUGUI.textInfo.characterCount;
			yield return null;
		}
		
		
		private void OnDisable()
		{
			if (writeMessageToConsoleChannel != null)
				writeMessageToConsoleChannel.Unsubscribe( AddTextToConsole);
			_animationQueue.ClearQueue();
		}
	}
}
