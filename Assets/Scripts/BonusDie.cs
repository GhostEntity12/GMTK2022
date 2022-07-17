using UnityEngine;

public class BonusDie : Die
{
    Rigidbody r;
    [SerializeField] float force = 5;

    [SerializeField] Camera c;

    // Start is called before the first frame update
    void Awake()
    {
        r = GetComponent<Rigidbody>();
    }


	protected override void Update()
	{
		base.Update();
        c.transform.position = transform.position + Vector3.up * 1.5f;
	}

	public void Roll()
	{
        r.AddTorque(Random.Range(-1000, 1000f) * new Vector3(Random.Range(-1000, 1000f), Random.Range(-1000f, 1000f), Random.Range(-1000f, 1000f)), ForceMode.VelocityChange);
        r.AddForce(new Vector3(Random.Range(-0.3f, 0.3f), 1, Random.Range(-0.3f, 0.3f)) * force, ForceMode.VelocityChange);
	}
}
