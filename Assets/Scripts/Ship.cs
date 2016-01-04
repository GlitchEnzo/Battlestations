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

        public int Size = 3;

        public int Speed;

        public int OutOfControlPenalty;

        public int HelmPower;

        public int GunsPower;

        public int ShieldsPower;

        public int DamageReceived;

        //public ModuleArray[] Modules = new ModuleArray[7];
        //public Module[][] Modules = new Module[7][];
        public List<Module> Modules = new List<Module>(49);
        public int xSize = 7;
        public int ySize = 7;

        public float Orientation
        {
            get { return transform.eulerAngles.z; }
        }

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
            Debug.Log(Dice.Probability(2, 8) * 100 + "%");
        }

        void Update()
        {

        }

        public virtual void GeneratePower()
        {
            // generate 1 power for each function engine
            // applied in the order: Helm, Guns, Shields, and then repeat
        }

        public virtual void EndRound()
        {
            // reduce speed by 1
            // reduce power for  Helm, Guns, & Shields by 1
            // removed all "used" modifier for modules
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filepath"></param>
        public void SaveToFile(string filepath)
        {
            XDocument shipFile = new XDocument();

            for (int y = 0; y < ySize; y++)
            {
                for (int x = 0; x < xSize; x++)
                {

                }
            }
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

            shipFile.Save(filepath);
        }
    }
}