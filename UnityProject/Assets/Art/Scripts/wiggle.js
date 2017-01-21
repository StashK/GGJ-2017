#pragma strict

var palm:GameObject;
var speed=2.0f;
var x=0.0f;
var y=0.0f;
var z=0.0f;
var xmin=0.0f;
var xmax=0.0f;
var ymin=0.0f;
var ymax=0.0f;
var zmin=0.0f;
var zmax=0.0f;
var counter=0.0f;

function Start()
{

}

function Update ()
{
  //  x = Random.Range(xmin,xmax);
  //  y = Random.Range(ymin,ymax);
    z = Random.Range(zmin,zmax);

    // counter += 1;
    z
    transform.RotateAround(palm.transform.position, new Vector3(x, y, z), 100 * Time.deltaTime*speed);

    if (counter <=  100) {
        transform.RotateAround(palm.transform.position, new Vector3(x, y, z), 100 * Time.deltaTime*speed);
    //    counter == 0;
    }

}