#pragma strict

var bird:GameObject;
var speed=1.0f;
var x=0.0f;
var y=.0f;
var z=0.0f;
var xmin=0.0f;
var xmax=0.0f;
var ymin=0.0f;
var ymax=0.0f;
var zmin=0.0f;
var zmax=0.0f;

function Start()
{

}

function Update ()
{
    y = Random.Range(ymin,ymax);

    transform.RotateAround(bird.transform.position, new Vector3(x, y, z), 100 * Time.deltaTime*speed);
}
