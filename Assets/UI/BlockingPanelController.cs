using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
	public class BlockingPanelController : MonoBehaviour
	{
		public TextMeshProUGUI tmpro;
		public Button closeButton;
		public TextMeshProUGUI primaryTaskTmpro;
		public TextMeshProUGUI taskExplanationTmpro;

		private const string LoadingText = "The trial will start in ";

		private const string ClosingText = "The Experiment is over, \n Thank you for participating! \n";


		public void LoadingView(float duration)
		{
			var primaryTaskName = "";
			if (GameManager.Instance.settingsDatabase.reroutingPanelSettings.headerText == "Primary Task")
				primaryTaskName = "Rerouting";
			else
			{
				primaryTaskName = "Flare Detection";
			}
			primaryTaskTmpro.text = "Primary Task: " + primaryTaskName;
			// make the panel visible by setting scale to 1
			transform.localScale = Vector3.one;
		
			//start a coroutine to animate the text every 0.5 seconds
			StartCoroutine(AnimateText(LoadingText,duration));
		
		
		}

		private IEnumerator AnimateText(string loadingText, float simulationStartTime)
		{ 
		
			tmpro.text = loadingText;
			while(Time.time < simulationStartTime)
			{
				tmpro.text = loadingText + "  "+ (simulationStartTime - Time.time).ToString("F0");
				yield return new WaitForSeconds(0.1f);
			}
		
			// hide the panel by setting scale to 0
			transform.localScale = Vector3.zero;
		}
	
		public void ClosingView()
		{
			primaryTaskTmpro.text = "";
			taskExplanationTmpro.text = " ";
			// make the panel visible by setting scale to 1
			transform.localScale = Vector3.one;
			tmpro.text = ClosingText;
		
			//make the close button visible
			closeButton.transform.localScale = Vector3.one;
			//wire up the close button
			closeButton.onClick.AddListener(() => Application.Quit());
		}
	}
}
