#pragma strict

var bird:GameObject;
var speed=2.0f;
var x=0.0f;
var y=1.0f;
var z=0.0f;

function Start()
{

}

function Update ()
{
    x = Random.Range(-0.1,0.1);
    y = Random.Range(0.1,1.5);
    z = Random.Range(-0.1,0.1);

    transform.RotateAround(bird.transform.position, new Vector3(x, y, z), 100 * Time.deltaTime*speed);
}