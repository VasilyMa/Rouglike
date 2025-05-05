using UnityEngine;

namespace Client {
    struct EvaluationHelpersData {
        
        /*public float idleRating;
        public float cowardiceRating;
        public float smartRating; //stupid - goes ahead without dodging, smart tries to dodge, perhaps it should be called "Dodge Rating", but can be used as a general rating to determine the intelligence of the agent
        public AnimationCurve distanceCurve;
        public AnimationCurve healthCurve;*/


        public AnimationCurve AggressionScoreByHealth;
        public AnimationCurve AggressionScoreByDistance;

        public AnimationCurve CowardiceScoreByHealth;
        public AnimationCurve CowardiceScoreByDistance;

        public AnimationCurve DefenceScoreByHealth;

        public float ReactionToAction;
        public float ReactionDelay;
        public float WanderingDelay;
        public Vector3 WanderingPos;
    }
    struct DataContexInTimeComponent
    {
        float AggressionScoreByHealth;
        float AggressionScoreByDistance;
        float CowardiceScoreByHealth;
        float CowardiceScoreByDistance;
        float DefenceScoreByHealth;
    }
}