using System;
using UnityEngine;

    public abstract class LineRegionBase : MonoBehaviour
    {
        public float Angle
        {
            get => _angle;
            set => _angle = value;
        }
        public float Length
        {
            get => _length;
            set => _length = value;
        }
        public float Width
        {
            get => _width;
            set => _width = value;
        }
        public float FillProgress
        {
            get => _fillProgress;
            set => _fillProgress = Mathf.Clamp01(value);
        }
        public Vector3 EndPosition
        {
            get
            {
                Vector3 start = transform.position;
                float radians = (360 - (_angle - 90)) % 360 / 360 * Mathf.PI * 2 + -transform.eulerAngles.y * Mathf.Deg2Rad;
                return start + new Vector3(Mathf.Cos(radians) * _length, 0, Mathf.Sin(radians) * _length);
            }
        }
        public Vector3 WidthOffset => Vector3.Cross(EndPosition - transform.position, Vector3.up).normalized * _width * 0.5f;

        public Vector3 LeftHandSideStart => transform.position + WidthOffset;
        
        public Vector3 RightHandSideStart => transform.position - WidthOffset;
        
        public Vector3 LeftHandSideEnd => EndPosition + WidthOffset;
        
        public Vector3 RightHandSideEnd => EndPosition - WidthOffset;
        
        [SerializeField]
        [Range(0, 360)]
        private float _angle;
        
        [SerializeField]
        [Range(0, 100)]
        private float _length;

        [SerializeField]
        [Range(0, 1)]
        private float _width;

        [SerializeField]
        [Range(0, 1)]
        private float _fillProgress;

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(LeftHandSideStart, LeftHandSideEnd);
            Gizmos.DrawLine(RightHandSideStart, RightHandSideEnd);
        }
    }
