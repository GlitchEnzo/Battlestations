namespace Battlestations
{
    using System.Collections.Generic;
    using System.Xml.Linq;
    using UnityEngine;

    public class Ship : MonoBehaviour
    {
        //[Serializable]
        //public class ModuleArray
        //{
        //    public Module[] Modules = new Module[7];

        //    public Module this[int index]
        //    {
        //        get { return Modules[index]; }
        //        set { Modules[index] = value; }
        //    }
        //}

        public Species Species = Species.Human;

        //public ModuleArray[] Modules = new ModuleArray[7];
        //public Module[][] Modules = new Module[7][];
        public List<Module> Modules = new List<Module>(49);
        public int xSize = 7;
        public int ySize = 7;

        public Module this[int x, int y]
        {
            get { return Modules[(y * xSize + x)]; }
            set { Modules[(y * xSize + x)] = value; }
        }

        void Reset()
        {
            //for (int i = 0; i < Modules.Length; i++)
            //{
            //    Modules[i] = new Module[7];
            //}

            xSize = 7;
            ySize = 7;
            Modules = new List<Module>(xSize * ySize);
            for (int i = 0; i < xSize * ySize; i++)
            {
                Modules.Add(null);
            }
        }

        void Start()
        {

        }

        void Update()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        public void LoadFromFile(string filepath)
        {
            // <Ship>
            //	<Module name="cannon" position="3,4" rotation="90" />
            // </Ship>

            XDocument shipFile = XDocument.Load(filepath);
            foreach (var moduleElement in shipFile.Root.Elements())
            {
                string name = moduleElement.Attribute("name").Value;
                string[] position = moduleElement.Attribute("position").Value.Split(',');
                int x = int.Parse(position[0]);
                int y = int.Parse(position[1]);
                float rotation = float.Parse(moduleElement.Attribute("rotation").Value);

                GameObject instance = Instantiate(Resources.Load<GameObject>(name));
                instance.transform.parent = this.transform;
                instance.transform.rotation = Quaternion.Euler(0, 0, rotation);

                // TODO: Set position based on the x & y indices.

                this[x, y] = instance.GetComponent<Module>();
            }
        }
    }
}