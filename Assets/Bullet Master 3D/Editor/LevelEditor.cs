using System.Linq;
using Bullet_Master_3D.Scripts;
using Bullet_Master_3D.Scripts.Editor;
using Bullet_Master_3D.Scripts.Game;
using Bullet_Master_3D.Scripts.Menu;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Bullet_Master_3D.Editor
{
    public class LevelEditor : EditorWindow
    {
        private LevelEditorSettings _settings;
        private LevelsSettings _levelsSettings;

        private GameManager _gameManager;
        private NavMeshSurface _navMeshSurface;
        private GameObject _playerSpawner;
        
        private Transform _environmentParent;
        private MeshRenderer _designMeshRenderer;

        private int _windowId;
        private int _designId;
        private bool _isLevelSettingsAdded;
        
        private const string ENVIRONMENT_OBJECTS_NAME = "EnvironmentObjects";
        private const string LEVEL_EDITOR_SETTINGS_RESOURCES_PATH = "Level Editor Settings";
        private const string DESIGN_NAME = "Design";

        [MenuItem("Level Editor/Open Editor")]
        private static void ShowWindow()
        {
            var window = GetWindow<LevelEditor>();
            window.titleContent = new GUIContent("Level Editor");
            window.maxSize = new Vector2(400,450);
            window.maxSize = new Vector2(400,450);
            window.Show();
        }
        
        [MenuItem("Level Editor/Clear all saves")]
        private static void ClearAllSaves()
        {
            SavesService.DeleteData();
        }

        private void OnEnable()
        {
            _settings = Resources.Load<LevelEditorSettings>(LEVEL_EDITOR_SETTINGS_RESOURCES_PATH);
            _levelsSettings = Resources.Load<LevelsSettings>(Constants.LEVELS_SETTINGS_RESOURCES_PATH);
        }
        
        private void OnDisable()
        {
            SetAllChangesDirty();
        }

        private void SetAllChangesDirty()
        {
            //Save all changes made in the scene
            if (_navMeshSurface != null)
            {
                EditorUtility.SetDirty(_navMeshSurface);
            }
            
            if (_designMeshRenderer != null)
            {
                EditorUtility.SetDirty(_designMeshRenderer);
            }
            
            if (_gameManager != null)
            {
                FindAllStickmen();
                EditorUtility.SetDirty(_gameManager);
                EditorSceneManager.MarkSceneDirty(_gameManager.gameObject.scene);
            }
        }

        private void OnGUI()
        {
            //There are six stages of level creation
            switch (_windowId)
            {
                case 0:
                    MainWindow();
                    break;
                case 1:
                    SelectDesignWindow();
                    break;
                case 2:
                    SelectDesignColorWindow();
                    break;
                case 3:
                    AddingEnvironmentWindow();
                    break;
                case 4:
                    SetLevelSettingsWindow();
                    break;
                case 5:
                    AddSceneToScenesInBuildWindow();
                    break;
            }
            
            if (GUI.changed)
            {
                SetAllChangesDirty();
            }
        }

        private void MainWindow()
        {
            GUI.Label(new Rect(125,10,200,20),"Welcome to level editor!");
            GUI.Label(new Rect(150,30,200,20),"Levels count: " + (SceneManager.sceneCountInBuildSettings - 1));

            GUI.Label(new Rect(100,50,200,350), _settings.Preview.texture);

            if (GUI.Button(new Rect(50,400,200,50),"Create new level"))
            {
                CreateNewLevel();
                SpawnDefaultLevelObjects();
                FindAllRequiredObjects();
                _windowId++;
            }

            if (GUI.Button(new Rect(250, 400, 100, 50), "Continue"))
            {
                FindAllRequiredObjects();
                _isLevelSettingsAdded = true;
                _windowId++;
            }
        }
        
        private void CreateNewLevel()
        {
            //You can change default scene name and default saving path
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Additive);
            var path = Constants.NEW_SCENE_SAVE_PATH + Constants.NEW_SCENE_NAME + SceneManager.sceneCountInBuildSettings + ".unity";
            EditorSceneManager.SaveScene(scene, path, false);
            EditorSceneManager.OpenScene(path);
        }
        
        private void SpawnDefaultLevelObjects()
        {
            foreach (var defaultSceneGameObject in _settings.DefaultSceneObjectsPrefabs)
            {
                PrefabUtility.InstantiatePrefab(defaultSceneGameObject);
            }
            _environmentParent = new GameObject(ENVIRONMENT_OBJECTS_NAME).transform;
        }

        private void FindAllRequiredObjects()
        {
            _gameManager = FindObjectOfType<GameManager>();
            _navMeshSurface = FindObjectOfType<NavMeshSurface>();
            _playerSpawner = FindObjectOfType<PlayerSpawnerService>().gameObject;
            _environmentParent = GameObject.Find(ENVIRONMENT_OBJECTS_NAME).transform;
            //Find design by name in list of all scene gameObjects
            for (var n = 0; n <= _environmentParent.childCount - 1; n++)
            {
                var child = _environmentParent.GetChild(n);
                if (child.name.Contains(DESIGN_NAME))
                {
                    _designMeshRenderer = child.GetComponent<MeshRenderer>();
                }
            }
        }

        private void SelectDesignWindow()
        {
            GUI.Label(new Rect(150,10,200,20),"Select level design:");
            GUI.Label( new Rect(100, 20, 200, 400),_settings.DesignsPreviews[_designId].texture);
            
            if (GUI.Button(new Rect(200, 400, 100, 50),"Select"))
            {
                //Destroy old design before spawn new
                if (_designMeshRenderer != null)
                {
                    DestroyImmediate(_designMeshRenderer.gameObject);
                }
                
                _designMeshRenderer = SpawnEnvironmentObject(_settings.DesignsPrefabs[_designId]).GetComponent<MeshRenderer>();
                _windowId++;
            }
            
            //If the list of designs is not finished yet, shows the next button
            if (_designId < _settings.DesignsPrefabs.Length - 1)
            {
                if (GUI.Button(new Rect(100, 400, 100, 50),"Next"))
                {
                    _designId++;
                }
            }
            
            //If not the first design is open, shows the button back
            if (_designId > 0)
            {
                if (GUI.Button(new Rect(0, 400, 100, 50),"Back"))
                {
                    _designId--;
                }
            }
            
            if (GUI.Button(new Rect(300, 400, 100, 50),"Continue"))
            {
                _windowId++;
            }
        }

        private void SelectDesignColorWindow()
        {
            //Show design color selection buttons
            foreach (var material in _settings.DesignsMaterials)
            {
                var style = new GUIStyle(GUI.skin.button) {normal = {textColor = material.color}};
                if (GUILayout.Button(material.name, style,GUILayout.Width(200),GUILayout.Height(32)))
                {
                    _designMeshRenderer.material = material;
                }
            }
            
            if (GUI.Button(new Rect(100, 400, 100, 50),"Next"))
            {
                _windowId++;
            }
            
            if (GUI.Button(new Rect(0, 400, 100, 50),"Back"))
            {
                _windowId--;
            }
        }

        private void AddingEnvironmentWindow()
        {
            //Select Player Spawner gameObject, for move & rotate
            if (GUILayout.Button("Select player spawner", GUILayout.Width(200), GUILayout.Height(32)))
            {
                Selection.activeObject = _playerSpawner;
            }
            
            //Spawn all environment gameObjects from the list
            foreach (var prefab in _settings.EnvironmentObjectsPrefabs)
            {
                if (GUILayout.Button("Spawn " + prefab.Prefab.name, GUILayout.Width(200),GUILayout.Height(32)))
                {
                    if (prefab.UsesDesignColor)
                    {
                        SpawnEnvironmentObject(prefab, _designMeshRenderer.sharedMaterial);
                    }
                    else
                    {
                        SpawnEnvironmentObject(prefab);
                    }

                    FindAllStickmen();
                }
            }

            if (GUI.Button(new Rect(0, 400, 100, 50),"Back"))
            {
                _windowId--;
            }
            
            if (GUI.Button(new Rect(100, 400, 100, 50),"Next"))
            {
                _windowId++;
                
                //Add a new level to level settings
                if (!_isLevelSettingsAdded)
                {
                    _levelsSettings.Levels.Add(new LevelsSettings.Level());
                    _isLevelSettingsAdded = true;
                }

                //When all environment objects added, creates a nav mesh map for navigated enemies
                _navMeshSurface.BuildNavMesh();
            }
        }

        private void FindAllStickmen()
        {
            _gameManager.AllEnemies = GameObject.FindGameObjectsWithTag(Constants.ENEMY_TAG).Select(gameObject => gameObject.GetComponent<Stickman>()).ToArray();
            _gameManager.Prisoners = GameObject.FindGameObjectsWithTag(Constants.PRISONER_TAG).Select(gameObject => gameObject.GetComponent<Stickman>()).ToArray();
        }

        private void SetLevelSettingsWindow()
        {
            if (GUI.Button(new Rect(100, 100, 200, 50),"Open level settings"))
            {
                Selection.activeObject = _levelsSettings;
            }
            
            if (GUI.Button(new Rect(0, 400, 100, 50),"Back"))
            {
                _windowId--;
            }
            
            if (GUI.Button(new Rect(100, 400, 100, 50),"Next"))
            {
                _windowId++;
            }
        }

        private void AddSceneToScenesInBuildWindow()
        {
            if (GUI.Button(new Rect(100, 150, 200, 50),"Open Scenes In Build Settings"))
            {
                GetWindow(System.Type.GetType("UnityEditor.BuildPlayerWindow,UnityEditor"));
            }
            
            if (GUI.Button(new Rect(100, 200, 200, 50),"Test it!"))
            {
                ClearAllSaves();
                OnDisable();
                TestIt();
                Close();
            }
            
            if (GUI.Button(new Rect(0, 400, 100, 50), "Back"))
            {
                _windowId--;
            }
        }

        private void TestIt()
        {
            var levelId = _gameManager.gameObject.scene.buildIndex;
            //Save new level scene
            var saveScenePath = Constants.NEW_SCENE_SAVE_PATH + Constants.NEW_SCENE_NAME + levelId + ".unity";
            EditorSceneManager.SaveScene(_gameManager.gameObject.scene, saveScenePath, false);
            //Open menu scene
            var openScenePath = Constants.NEW_SCENE_SAVE_PATH + Constants.MENU_SCENE_NAME + ".unity";
            EditorSceneManager.OpenScene(openScenePath);
            //Play game with new level
            var sceneService = FindObjectOfType<ScenesService>();
            sceneService.TestModeEnabled = true;
            sceneService.LevelForTestId = levelId;
            EditorApplication.isPlaying = true;
        }

        #region Instatiation
        private GameObject SpawnEnvironmentObject(LevelEditorSettings.PrefabSettings settings)
        {
            var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(settings.Prefab, _environmentParent);
            gameObject.transform.position = settings.DefaultLocalPosition;
            gameObject.transform.localEulerAngles = settings.DefaultLocalEulerAngles;
            Selection.activeObject = gameObject;
            return gameObject;
        }
        
        private GameObject SpawnEnvironmentObject(LevelEditorSettings.PrefabSettings settings, Material material)
        {
            var gameObject = (GameObject) PrefabUtility.InstantiatePrefab(settings.Prefab, _environmentParent);
            gameObject.transform.position = settings.DefaultLocalPosition;
            gameObject.transform.localEulerAngles = settings.DefaultLocalEulerAngles;
            if (material != null)
            {
                gameObject.GetComponent<MeshRenderer>().sharedMaterial = material;
            }
            Selection.activeObject = gameObject;
            return gameObject;
        }
        #endregion
    }
}