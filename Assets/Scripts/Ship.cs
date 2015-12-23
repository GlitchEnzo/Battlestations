namespace Battlestations
{
    using System;
    using System.Collections.Generic;
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
    }
}