using UnityEngine;
using System.Collections;

public class RotateAroundTarget : MonoBehaviour
{
	public Transform Target;
	public Vector3 Axis;
	public float Speed;

    private void Start()
    {
        if(Target == null)
        {
            Target = this.gameObject.transform;
        }
    }
        
    void Update ()
	{
		transform.RotateAround (Target.position, Axis, Speed*Time.deltaTime);
	}

}
