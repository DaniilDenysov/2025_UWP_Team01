using UnityEngine;
using UnityEngine.InputSystem;
using System;

namespace TowerDeffence.CameraHandling
{
    [System.Serializable]
    public abstract class CameraAction
    {
        protected CameraMovement context;

        public virtual void Initialize(CameraMovement context)
        {
           this.context = context;
        }
        public virtual void OnEnable() { }
        public virtual void OnDisable() { }
        public virtual void UpdateAction() { }

        public virtual void OnDestroy() { }

        public virtual void OnGUI() { }
    }

    [System.Serializable]
    public class MoveCameraAction : CameraAction
    {
        private InputAction moveAction;
        [SerializeField] private float speed = 10f;
        private Vector2 inputVector;

        [SerializeField] private Transform lowerBoundTransform;
        [SerializeField] private Transform upperBoundTransform;

        public override void Initialize(CameraMovement context)
        {
            base.Initialize(context);
            moveAction = new DefaultActions().Player.Move;
            moveAction.Enable();
        }

        public override void OnEnable()
        {
            moveAction?.Enable();
        }
        public override void OnDisable()
        {
            moveAction?.Disable();
        }

        public override void OnDestroy()
        {
            OnDisable();
        }

        public override void UpdateAction()
        {
            inputVector = moveAction != null ? moveAction.ReadValue<Vector2>() : Vector2.zero;
            if (inputVector.magnitude == 0)
                return;

            Vector3 moveDir = new Vector3(inputVector.y, 0, -inputVector.x).normalized;
            Vector3 targetPos = context.transform.position + moveDir * speed * Time.deltaTime;

            Vector3 lower = lowerBoundTransform.position;
            Vector3 upper = upperBoundTransform.position;

            targetPos.x = Mathf.Clamp(targetPos.x, Mathf.Min(lower.x, upper.x), Mathf.Max(lower.x, upper.x));
            targetPos.z = Mathf.Clamp(targetPos.z, Mathf.Min(lower.z, upper.z), Mathf.Max(lower.z, upper.z));

            context.transform.position = targetPos;
        }

        public override void OnGUI()
        {
            if (lowerBoundTransform != null && upperBoundTransform != null)
            {
                Vector3 lower = lowerBoundTransform.position;
                Vector3 upper = upperBoundTransform.position;
                Vector3 center = (lower + upper) * 0.5f;
                Vector3 size = new Vector3(
                    Mathf.Abs(upper.x - lower.x),
                    0.1f,
                    Mathf.Abs(upper.z - lower.z)
                );
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireCube(center, size);
            }
        }
    }

    [System.Serializable]
    public class ZoomCameraAction : CameraAction
    {
        private InputAction zoomAction;
        [SerializeField] private float zoomSpeed = 10f;
        [SerializeField] private float minZoom = 5f;
        [SerializeField] private float maxZoom = 30f;
        private float inputZoomDelta;
        [SerializeField] private Camera camera;

        public override void Initialize(CameraMovement context)
        {
            base.Initialize(context);
            zoomAction = new DefaultActions().Player.Zoom;
            zoomAction.Enable();
        }

        public override void OnEnable()
        {
            zoomAction?.Enable();
        }
        public override void OnDisable()
        {
            zoomAction?.Disable();
        }

        public override void OnDestroy()
        {
            OnDisable();
        }

        public override void UpdateAction()
        {
            inputZoomDelta = (zoomAction != null ? zoomAction.ReadValue<Vector2>() : Vector2.zero).y;

            if (Mathf.Abs(inputZoomDelta) < 0.01f)
                return;

            if (camera == null) return;

            float target = camera.orthographic ? camera.orthographicSize : camera.fieldOfView;

            target -= inputZoomDelta * zoomSpeed * Time.deltaTime;
            target = Mathf.Clamp(target, minZoom, maxZoom);

            if (camera.orthographic)
                camera.orthographicSize = target;
            else
                camera.fieldOfView = target;
        }
    }

    public class CameraMovement : MonoBehaviour
    {
        [SerializeReference, SubclassSelector] private CameraAction [] cameraActions;

        private void Start()
        {
            Iterate((a) => a.Initialize(this));
        }

        private void OnEnable()
        {
            Iterate((a) => a.OnEnable());
        }

        private void OnDisable()
        {
            Iterate((a) => a.OnDisable());
        }

        private void Update()
        {
            Iterate((a) => a.UpdateAction());
        }

        private void OnDestroy()
        {
            Iterate((a) => a.OnDestroy());
        }

        #if UNITY_EDITOR
         private void OnDrawGizmos()
         {
            Iterate((a) => a.OnGUI());
         }
        #endif

        private void Iterate(Action<CameraAction> callback)
        {
            foreach (var action in cameraActions)
            {
                callback?.Invoke(action);
            }
        }
    }
}
