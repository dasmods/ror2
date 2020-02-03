using EntityStates.Engi.Mine;
using UnityEngine;
using UnityEngine.Networking;
using RoR2;


namespace HealBois
{
    class Heal : BaseMineState
    {
        private static float INITIAL_RADIUS = 10f;
        private static float MAX_AGE = 10f;
        private static float DECAY_RATE = INITIAL_RADIUS / MAX_AGE;

        private static GameObject CreateMushroomWard(Vector3 position)
        {
            // create mushroom ward visuals
            GameObject resource = Resources.Load<GameObject>("Prefabs/NetworkedObjects/MushroomWard");
            GameObject mushroomWard = UnityEngine.Object.Instantiate(resource, position, Quaternion.identity);
            mushroomWard.GetComponent<TeamFilter>().teamIndex = TeamIndex.Player;
            NetworkServer.Spawn(mushroomWard);

            // initialize healing ward props
            HealingWard healingWard = mushroomWard.GetComponent<HealingWard>();
            healingWard.healFraction = 0.05f;
            healingWard.healPoints = 0f;
            healingWard.Networkradius = INITIAL_RADIUS;

            return mushroomWard;
        }

        private GameObject mushroomWard;

        public override void OnEnter()
        {
            base.OnEnter();
            mushroomWard = CreateMushroomWard(transform.position);
        }
        protected override bool shouldStick
        {
            get
            {
                return true;
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
            UpdatePosition();
            UpdateRadius();
        }

        public override void OnExit()
        {
            base.OnExit();
            UnityEngine.Object.Destroy(mushroomWard);
        }

        private void UpdatePosition()
        {
            mushroomWard.transform.position = transform.position;
        }

        private void UpdateRadius()
        {
            float nextRadius = GetRadius();

            if (nextRadius <= 0)
            {
                outer.SetNextState(new PreDetonate());
            }

            HealingWard healingWard = mushroomWard.GetComponent<HealingWard>();
            healingWard.Networkradius = nextRadius;
        }

        private float GetRadius()
        {
            return (-DECAY_RATE * fixedAge) + INITIAL_RADIUS;
        }
    }
}
