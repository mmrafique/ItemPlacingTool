using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ItemPlacerWindow : EditorWindow
{
    // Para la biblioteca de prefabs
    [SerializeField] private List<GameObject> prefabLibrary = new List<GameObject>();
    [SerializeField] private int activePrefabIndex = -1;

    //Esto hace que los objetos se instancien dentro de objeto padre como hijos
    [SerializeField] private Transform spawnParent;

    //Para almacenar la ultima instancia y la rotacion
    private GameObject lastInstance;
    private bool isRotating;


    [MenuItem("Tools/Item Placer Tool")]
    public static void ShowWindow()
    {
        GetWindow<ItemPlacerWindow>("Item Placer Tool");
    }
    
    //Sirve para dibujar cuando la ventana esta abierta
    private void OnEnable()
    {
        SceneView.duringSceneGui += OnSceneGUI;
    }

    //Sirve para que deje de dibujar cuando la ventana esta cerrada
    private void OnDisable()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnGUI()
    {
        //Pone el titulo de la ventana
        EditorGUILayout.LabelField("Prefab Library", EditorStyles.boldLabel);

        //Mostrar la lista que hay de prefabs que se pueden usar
        for (int i = 0; i < prefabLibrary.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();

            //Se puede cambiar del prefab de la llista
            prefabLibrary[i] = (GameObject)EditorGUILayout.ObjectField(prefabLibrary[i], typeof(GameObject), false);

            //Aqui se elimina el prefab de la lista
            if (GUILayout.Button("X", GUILayout.Width(20)))
            {
                prefabLibrary.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        //Este boton sirve para añadir prefab seleccionado a la lista de prefabs
        if (GUILayout.Button("Añadir prefab Seleccionado"))
        {
            if (Selection.activeGameObject != null)
            {
                //Comprobar cual objeto esta seleccionado que peretenece a la lista de prefabs
                string path = AssetDatabase.GetAssetPath(Selection.activeGameObject);
                if (!string.IsNullOrEmpty(path))
                {
                    prefabLibrary.Add(Selection.activeGameObject);
                }
            }
        }

        EditorGUILayout.Space();

        //Seklecciona cual sera prefab se activo
        activePrefabIndex = EditorGUILayout.Popup("Prefab activo", activePrefabIndex, GetLabels());

        // Objeto padre donde se crean los prefabs que van a instanciarse
        spawnParent = (Transform)EditorGUILayout.ObjectField("Padre", spawnParent, typeof(Transform), true);

        //usar el objeto seleccionado de los objetos como padre 
        if (GUILayout.Button("Usar seleccionado como padre"))
        {
            spawnParent = Selection.activeTransform;
        }
    }

    private string[] GetLabels()
    {
        //si no hay ningun prefabs devolvera un texto vacio que dira sin prefabs
        if (prefabLibrary.Count == 0) return new[] { "Sin prefabs" };
        //aqui cra un array con los numeros de los prefabs que hay en la lista o si no hay nignuno
        string[] labels = new string[prefabLibrary.Count];
        //aqui recorre la lista de los prefabs y guardara el nombre de cada uno de los prefabs en la array de lables que hay para desplegable
        for (int i = 0; i < prefabLibrary.Count; i++)
        {
            //si el prefab exite usara su nombre sino existe exrbira el texto falta
            labels[i] = prefabLibrary[i] != null ? prefabLibrary[i].name : "Falta";
        }
        //aqui devuelve la lista de los nombres en el dropdown de la flecha
        return labels;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        //si no hay ningun prefab activo no hara nada
        if (activePrefabIndex < 0 || activePrefabIndex >= prefabLibrary.Count) return;

        //guaradara el evento actual del raotn para colocar el prefab y moverlo
        Event e = Event.current;

        //si clicas el click izquierdo de raton en la scene view coloca el objeto de prefab
        if (e.type == EventType.MouseDown && e.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceInstance(hit.point);
                isRotating = true;
                e.Use();
            }
        }

        // DRAG PARA ROTAR
        if (e.type == EventType.MouseDrag && e.button == 0 && lastInstance != null && isRotating)
        {
            Undo.RecordObject(lastInstance.transform, "Rotar");
            lastInstance.transform.Rotate(Vector3.up, -e.delta.x * 0.5f);
            e.Use();
        }

        // SOLTAR
        if (e.type == EventType.MouseUp && e.button == 0)
        {
            isRotating = false;
        }
    }

    private void PlaceInstance(Vector3 position)
    {
        GameObject prefab = prefabLibrary[activePrefabIndex];
        if (prefab == null) return;

        // INSTANCIAR
        GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        
        // UNDO
        Undo.RegisterCreatedObjectUndo(instance, "Colocar objeto");

        // PADRE
        if (spawnParent != null)
        {
            Undo.SetTransformParent(instance.transform, spawnParent, "Colocar objeto");
        }

        // POSICIÓN
        instance.transform.position = position;

        lastInstance = instance;
    }
}