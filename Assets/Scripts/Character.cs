namespace Battlestations
{
    using System.Collections.Generic;
    using UnityEngine;

    public class Character : MonoBehaviour
    {
        public string Name;

        public int Athletics;

        public int Combat;

        public int Engineering;

        public int Piloting;

        public int Science;

        public Skill ProfessionalSkill;

        public Species Species = Species.Human;

        public int HitPoints;

        public int TargetNumber;

        public int NumberOfHands;

        public int Move;

        public bool CanWearArmor;

        public List<Ability> Abilities;

        public List<Item> Equipment;

        public int CarryLimit
        {
            get { return Athletics * 10; }
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}