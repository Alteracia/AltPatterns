using Alteracia.Patterns.ScriptableObjects.Events;
using UnityEngine;

public class EventsBusTest : MonoBehaviour
{
    [SerializeField] private Vector3ObjectSubscribableEvent v3SubscribableEventBuss;
    [SerializeField] private Vector3ObjectSubscribableEvent v3SubscribableEventRegistry;
    
    [SerializeField] private Vector3TwoStateSubscribableEvent v3TSeventBuss;
    [SerializeField] private Vector3TwoStateSubscribableEvent v3TSeventRegistry;

    [SerializeField] private MeshRendererComponentSubscribableEvent renderSubscribableEventBuss;
    [SerializeField] private MeshRendererComponentSubscribableEvent renderSubscribableEventRegistry;

    
    // Start is called before the first frame update
    void Start()
    {
        v3SubscribableEventRegistry.OnEvent += vector3 =>
        {
            bool check = vector3 == Vector3.back;
            Debug.Log("V3 Obj Success " + check);
        };
        v3SubscribableEventBuss.OnEvent.Invoke(Vector3.back);
        //v3eventRegistry.OnEvent.Invoke(Vector3.back);

        v3TSeventRegistry.OnPrimaryEvent += vector3 =>
        {
            bool check = vector3 == Vector3.down;
            Debug.Log("V3 TS Success " + check);
        };
        v3TSeventBuss.OnPrimaryEvent.Invoke(Vector3.down); 
        //v3TSeventRegistry.OnPrimaryEvent.Invoke(Vector3.down);
        
        v3TSeventBuss.OnSecondaryEvent += vector3 =>
        {
            bool check = vector3 == Vector3.right;
            Debug.Log("V3 TS Second Success " + check);
        };
        v3TSeventBuss.OnSecondaryEvent.Invoke(Vector3.right); 
        //v3TSeventRegistry.OnSecondaryEvent.Invoke(Vector3.right);

        renderSubscribableEventBuss.OnEvent += renderer1 =>
        {
            var rend = GetComponent<Renderer>();
            Debug.Log("Rend Success " + (renderer1 == rend));
        };
        renderSubscribableEventRegistry.AddComponent(this.gameObject);
        //renderEventBuss.GetComponent(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
