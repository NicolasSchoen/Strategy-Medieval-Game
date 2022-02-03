using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;
using UnityEngine.UI;

public class menuScript : MonoBehaviour
{
    public GameObject menuCamera;
    public int loadRotationDirection = 1;
    private string[] saveNames;
    public GameObject savegameScrollView;
    public GameObject savegameButton;
    public Animator camerAnimation;

    [Header("Settings Elements")]
    public Slider soundVolumeSlider;
    private float savedVolume;
    public Toggle peacufulmodeToggle;
    public Toggle godModeToggle;
    public Slider mapSizeSlider;
    public Slider difficultySlider;
    public Slider rainSlider;

    public Canvas loadingScreen;

    private AsyncOperation asyncLoad;


    private void Start()
    {
        if (!Directory.Exists(Application.dataPath + "/saves")) Directory.CreateDirectory(Application.dataPath + "/saves");
        saveNames = Directory.GetDirectories(Application.dataPath + "/saves");
        foreach (string itsavegame in saveNames)
        {
            Debug.Log(Path.GetFileName(itsavegame));
            string levelName = Path.GetFileName(itsavegame);
            GameObject currentSaveGame = Instantiate(savegameButton) as GameObject;

            currentSaveGame.GetComponent<Button>().onClick.AddListener(delegate { loadLoadDemo(levelName); });
            currentSaveGame.transform.SetParent(savegameScrollView.transform.GetChild(0).GetChild(0), false);
            currentSaveGame.name = levelName;

            currentSaveGame.GetComponentInChildren<TextMeshProUGUI>().text = Path.GetFileName(itsavegame);

            //savegameScrollView.GetComponent<ScrollView>().Add(savegameButton.GetComponent<Button
        }
        savedVolume = AudioListener.volume;
        initializeSettingsElements();

    }

    private void initializeSettingsElements()
    {
        soundVolumeSlider.value = AudioListener.volume;
        peacufulmodeToggle.isOn = globalVariables.isPeaceful;
        godModeToggle.isOn = globalVariables.GodMode;
        mapSizeSlider.value = globalVariables.mapSize;
        mapSizeSlider.GetComponentInChildren<TextMeshProUGUI>().text = "Map Size: " + mapSizeSlider.value.ToString();
        difficultySlider.value = globalVariables.difficulty;
        string difficultytext = "";
        switch (difficultySlider.value)
        {
            case 1:
                {
                    difficultytext = "Easy";
                    break;
                }
            case 2:
                {
                    difficultytext = "Normal";
                    break;
                }
            case 3:
                {
                    difficultytext = "Hard";
                    break;
                }
        }
        difficultySlider.GetComponentInChildren<TextMeshProUGUI>().text = "Difficulty: " + difficultytext;

        rainSlider.value = globalVariables.possibilityForRain;
        rainSlider.GetComponentInChildren<TextMeshProUGUI>().text = "Rain Probability: " + rainSlider.value.ToString() + "%";
    }

    public void loadGameSystem()
    {
        loadingScreen.enabled = true;
        StartCoroutine(loadScene("gameSystem"));
    }

    public void loadWorldGenerator()
    {
        StartCoroutine(loadScene("world_generator"));
    }

    public void loadLoadDemo(string levelname)
    {
        globalVariables.loadedSaveName = levelname;
        globalVariables.loadSaveGame = true;

        loadingScreen.enabled = true;
        StartCoroutine(loadScene("buildingGenerator"));
    }

    public void loadLoadDemo()
    {
        globalVariables.loadedSaveName = "demoWorld";
        globalVariables.loadSaveGame = true;

        loadingScreen.enabled = true;
        StartCoroutine(loadScene("buildingGenerator"));
    }

    public void loadBuildingGenerator()
    {
        globalVariables.loadedSaveName = "";
        globalVariables.loadSaveGame = false;

        //globalVariables.prepareWorld();

        loadingScreen.enabled = true;
        StartCoroutine(loadScene("buildingGenerator"));
    }

    public void loadLoadWorld()
    {
        if (camerAnimation.GetBool("isLoad"))
        {
            camerAnimation.SetBool("isLoad", false);
        }
        else
        {
            camerAnimation.SetBool("isLoad", true);
        }
    }

    public void loadSettings()
    {
        if (camerAnimation.GetBool("isOptions"))
        {
            AudioListener.volume = savedVolume;
            initializeSettingsElements();
            camerAnimation.SetBool("isOptions", false);
        }
        else
        {
            camerAnimation.SetBool("isOptions", true);
        }
    }

    public void saveSettings()
    {
        globalVariables.isPeaceful = peacufulmodeToggle.isOn;
        globalVariables.GodMode = godModeToggle.isOn;
        AudioListener.volume = soundVolumeSlider.value;
        globalVariables.mapSize = (int)mapSizeSlider.value;
        globalVariables.difficulty = (int)difficultySlider.value;
        globalVariables.possibilityForRain = (int)rainSlider.value;
        savedVolume = AudioListener.volume;
        loadSettings();
    }

    public void experienceVolumeChange()
    {
        AudioListener.volume = soundVolumeSlider.value;
    }

    public void moveMapsizeSlider()
    {
        mapSizeSlider.GetComponentInChildren<TextMeshProUGUI>().text = "Map Size: " + mapSizeSlider.value.ToString();
    }

    public void moveDifficultySlider()
    {
        string difficultytext = "";
        switch (difficultySlider.value)
        {
            case 1:
                {
                    difficultytext = "Easy";
                    break;
                }
            case 2:
                {
                    difficultytext = "Normal";
                    break;
                }
            case 3:
                {
                    difficultytext = "Hard";
                    break;
                }
        }
        difficultySlider.GetComponentInChildren<TextMeshProUGUI>().text = "Difficulty: " + difficultytext;
    }

    public void moveRainSlider()
    {
        rainSlider.GetComponentInChildren<TextMeshProUGUI>().text = "Rain Probability: " + rainSlider.value.ToString() + "%";
    }

    public void quitGame()
    {
        Application.Quit();
    }

    IEnumerator loadScene(string sceneName)
    {
        asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        /*asyncLoad.allowSceneActivation = false;

        while (asyncLoad.progress <= .89f) 
        {
            yield return null;
        } 
        asyncLoad.allowSceneActivation = true;*/
        yield return asyncLoad;
    }
}
