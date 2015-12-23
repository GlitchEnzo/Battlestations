using System.Linq;

namespace Battlestations
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(Ship))]
    public class ShipInspector : Editor
    {
        private Ship ship;
        private SerializedObject serializedShip;

        public void OnEnable()
        {
            ship = (Ship)target;
            serializedShip = new SerializedObject(target);
        }

        public override void OnInspectorGUI()
        {
            ship.Species = (Species)EditorGUILayout.EnumPopup("Species", ship.Species);

            //DropAreaGUI();

            var modulesProperty = serializedShip.FindProperty("Modules");

            float size = (Screen.width - 18) / 7.0f;
            for (int y = 0; y < ship.ySize; y++)
            {
                EditorGUILayout.BeginHorizontal();
                for (int x = 0; x < ship.xSize; x++)
                {
                    //ship[x, y] = (Module)EditorGUILayout.ObjectField(ship[x, y], typeof(Module), true, GUILayout.Width(size), GUILayout.Height(size));

                    //var moduleProperty = modulesProperty.GetArrayElementAtIndex(y * ship.xSize + x);
                    //EditorGUILayout.PropertyField(moduleProperty, new GUIContent(string.Empty), false, GUILayout.Width(size), GUILayout.Height(size));

                    Texture2D texture = ship[x, y] != null ? ship[x, y].Texture : null;
                    Module droppedModule = DrawModule(texture, string.Format("{0},{1}", x, y), size, size);
                    if (droppedModule != null)
                    {
                        ship[x, y] = droppedModule;
                    }
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private Module DrawModule(Texture2D texture, string text, float width, float height)
        {
            Event evt = Event.current;
            Rect dropArea = GUILayoutUtility.GetRect(width, height);
            GUI.Box(dropArea, text);
            GUI.DrawTexture(dropArea, texture);

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!dropArea.Contains(evt.mousePosition))
                        return null;

                    Object draggedObject = DragAndDrop.objectReferences.FirstOrDefault();
                    GameObject draggedGameObject = draggedObject as GameObject;
                    Module draggedModule = null;
                    if (draggedGameObject != null)
                    {
                        draggedModule = draggedGameObject.GetComponent<Module>();
                    }

                    if (draggedModule != null)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
                    }
                    else
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Rejected;
                    }

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        Debug.LogFormat("Dropped {0}", draggedObject);
                        return draggedModule;
                    }
                    break;
            }

            return null;
        }

        /// <summary>
        /// From here: https://gist.github.com/bzgeb/3800350
        /// </summary>
        public void DropAreaGUI()
        {
            Event evt = Event.current;
            Rect drop_area = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
            GUI.Box(drop_area, "Add Trigger");

            switch (evt.type)
            {
                case EventType.DragUpdated:
                case EventType.DragPerform:
                    if (!drop_area.Contains(evt.mousePosition))
                        return;

                    DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

                    if (evt.type == EventType.DragPerform)
                    {
                        DragAndDrop.AcceptDrag();

                        foreach (Object dragged_object in DragAndDrop.objectReferences)
                        {
                            // Do On Drag Stuff here
                            Debug.LogFormat("Dropped {0}", dragged_object);
                        }
                    }
                    break;
            }
        }
    }
}
