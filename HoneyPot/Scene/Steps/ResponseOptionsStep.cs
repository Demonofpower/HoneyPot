using System.Collections.Generic;
using HoneyPot.Debug;
using HoneyPot.Scene.Helper;

namespace HoneyPot.Scene.Steps
{
    class ResponseOptionsStep : IStep
    {
        public event StepFinishedEventHandler StepFinished;

        private List<Response> responseOptions;

        public ResponseOptionsStep(List<Response> responseOptions)
        {
            this.responseOptions = responseOptions;
        }
        
        public void InvokeStep()
        {
            var sceneResponseOptions = new List<DialogSceneResponseOption>();
            foreach (var responseOption in responseOptions)
            {
                var dialogSceneResponseOption = new DialogSceneResponseOption();
                dialogSceneResponseOption.steps = new List<DialogSceneStep>();
                dialogSceneResponseOption.text = responseOption.text;
                sceneResponseOptions.Add(dialogSceneResponseOption);
            }
            
            GameManager.Stage.uiWindows.forceResponseOptions = sceneResponseOptions;
            
            TalkWindow talkWindow = GameManager.Stage.uiWindows.SetWindow(GameManager.Stage.uiWindows.talkWindow, true, false) as TalkWindow;
            talkWindow.ResponseSelectedEvent += this.OnResponseOptionSelected;
        }

        private void OnResponseOptionSelected(TalkWindow talkwindow, DialogSceneResponseOption returnresponseoption)
        {
            GameManager.Stage.uiWindows.HideActiveWindow();

            Response usedResponse = null;
            foreach (var responseOption in responseOptions)
            {
                if (responseOption.text == returnresponseoption.text)
                {
                    usedResponse = responseOption;
                }
            }

            ScenePlayer.Instance.AddScenesAtFirst(usedResponse.steps);

            StepFinished?.Invoke();
        }
    }
}
