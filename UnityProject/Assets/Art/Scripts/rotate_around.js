#pragma strict

var bird:GameObject;
var speed=2.0f;
var x=0.0f;
var y=1.0f;
var z=0.0f;
var ymin=0.1f;
var ymax=1.5f;

function Start()
{

}

function Update ()
{
   // x = Random.Range(-0.1,0.1);
    y = Random.Range(ymin,ymax);
  //  z = Random.Range(-0.1,0.1);

    transform.RotateAround(bird.transform.position, new Vector3(x, y, z), 100 * Time.deltaTime*speed);
}