using UnityEditor;
using UnityEngine;

public class MyUtility : EditorWindow
{
    Vector2 scrollPosition = Vector2.zero;
    string myString = "Hello World";
    bool groupEnabled;

    float myFloat = 1.23f;

    bool update_selection_info = false;

    //Collider collider;
    //Mesh meshfilter;
    GameObject selectingGameObject, previous_selectingGameObject;
    Vector3 obj_pos,obj_localpos;
    float dist_to_ground;
    Bounds obj_filterbounds, obj_renderbounds, obj_colliderbounds,parent_bounds;
    int childCount;

    Bounds init_bounds = new Bounds(new Vector3(0, 0, 0), new Vector3(0,0,0));

    struct ObjPosSize
    {
        public GameObject obj;
        public string name;
        public Vector3 obj_pos, obj_localpos, obj_size;
        public Bounds obj_colliderbounds;
        public bool showPosition;

        public ObjPosSize(GameObject obj)
        {
            this.obj = obj;
            this.name = obj.name;
            this.obj_pos = obj.transform.position;
            this.obj_localpos = obj.transform.localPosition;
            this.showPosition = true;

            if (obj.TryGetComponent(out Collider collider))
            {
                this.obj_colliderbounds = collider.bounds;
                this.obj_size = collider.bounds.size;
            }
            else
            {
                this.obj_colliderbounds = new Bounds(new Vector3(0, 0, 0), new Vector3(0, 0, 0));
                this.obj_size = Vector3.zero;
            }
        }
    }

    ObjPosSize currentselectingGameObject;
    ObjPosSize[] childrenofselectingGameObject = new ObjPosSize[10];




    // Add menu named "My Window" to the Window menu
    [MenuItem("My Tools/My Utility")]
    static void Init()
    {
        

        // Get existing open window or if none, make a new one:
        MyUtility window = (MyUtility)EditorWindow.GetWindow(typeof(MyUtility));
        window.Show();


    }
    void OnSelectionChange()
    {
        Debug.Log("activeGameObject " + Selection.activeGameObject);
        childCount = 0;
        dist_to_ground = 0;
        //obj_filterbounds = init_bounds;
        //obj_renderbounds = init_bounds;
        obj_colliderbounds = init_bounds;
        //parent_bounds = init_bounds;
        if (Selection.activeGameObject == null)
        {
            selectingGameObject = null;

        }
        else
        {
            selectingGameObject = Selection.activeGameObject;
            currentselectingGameObject = new ObjPosSize(selectingGameObject);

            childCount = selectingGameObject.transform.childCount;
            //childrenofselectingGameObject = new ObjPosSize[childCount];
            for (int i = 0; i < childCount; i++)
            {
                //Debug.Log(i.ToString());
                GameObject childObject = selectingGameObject.transform.GetChild(i).gameObject;
                Debug.Log(i+" "+childObject.name);
                
                childrenofselectingGameObject[i] = new ObjPosSize(childObject);
                //Debug.Log(childrenofselectingGameObject[i].name + " " + childrenofselectingGameObject[i].obj_pos);
            }

        }


    }
    float DistToGround(GameObject obj)
    {
        
        Transform transform = obj.transform;

        //For checking
        var hits = Physics.RaycastAll(transform.position + Vector3.up, Vector3.down, Mathf.Infinity);
        Debug.Log(transform.gameObject.name + " ground hits number:" + hits.Length);
        for (int i=0;i<hits.Length;i++)
        {
            Debug.Log(i + " hit at " + hits[i].point);
        }

        //Real caluculation
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(transform.position, Vector3.down, out hit, Mathf.Infinity))
        {
            Debug.Log(transform.gameObject.name + " ground hit at " + hit.point);

            return transform.position.y - hit.point.y;
        }
        else
        {
            Debug.Log(transform.gameObject.name + " not hitting.");
            return 0f;
        }
            


        
    }

    void FallToGround(GameObject obj, float distance)
    {
        Transform transform = obj.transform;

        transform.position = new Vector3(transform.position.x, transform.position.y- distance, transform.position.z);

    }

    // Need to draw GUI everframe
    void OnGUI()
    {
        EditorGUIUtility.wideMode = true;


        //Debug.Log("selectingGameObject " + selectingGameObject);
        if (Selection.activeGameObject == null)
        {
            return;
        }

        //EditorGUILayout.BeginHorizontal();
        //EditorGUILayout.EndHorizontal();       
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);

        //EditorGUILayout.LabelField(selectingGameObject.name);
        EditorGUILayout.TextField("GameObject", currentselectingGameObject.name);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Distant To Ground"))
        {
            dist_to_ground = DistToGround(currentselectingGameObject.obj);
        }
        if (GUILayout.Button("Fall to Ground"))
        {
            FallToGround(currentselectingGameObject.obj, dist_to_ground);
        }
        EditorGUILayout.EndHorizontal();
        dist_to_ground = EditorGUILayout.FloatField("Distant to ground:", dist_to_ground);
        EditorGUILayout.Vector3Field("Local Position:", currentselectingGameObject.obj_localpos);
        EditorGUILayout.Vector3Field("Global Position:", currentselectingGameObject.obj_pos);
        EditorGUILayout.Vector3Field("Collider Size:", currentselectingGameObject.obj_size);
        EditorGUILayout.BoundsField("Collider Bound:", currentselectingGameObject.obj_colliderbounds);

        if (childCount > 0)
        {
            EditorGUILayout.LabelField(" # Children");
            for (int i = 0; i < childCount; i++)
            {

                //GameObject childObject = selectingGameObject.transform.GetChild(i).gameObject;
                ObjPosSize ops = childrenofselectingGameObject[i];
                //EditorGUILayout.TextField(i.ToString(), childObject.name);
                childrenofselectingGameObject[i].showPosition = EditorGUILayout.Foldout(childrenofselectingGameObject[i].showPosition, ops.name);
                if (childrenofselectingGameObject[i].showPosition)
                {

                    EditorGUILayout.Vector3Field("Local Position:", ops.obj_localpos);
                    EditorGUILayout.Vector3Field("Global Position:", ops.obj_pos);
                    EditorGUILayout.Vector3Field("Collider Size:", ops.obj_size);
                    EditorGUILayout.BoundsField("Collider Bound:", ops.obj_colliderbounds);
                }


                EditorGUILayout.LabelField(" ");

            }
        }

        GUILayout.EndScrollView();
        

    }
    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

}