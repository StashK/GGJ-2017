#pragma strict
/*
var counter = 0;
var win = false;
*/
function Start()
{

    LeanTween.moveX( gameObject, 0, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    LeanTween.moveY( gameObject, 110, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    LeanTween.moveZ( gameObject, -50, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    
    LeanTween.rotateX( gameObject, 70, 2).setEase( LeanTweenType.easeInOutQuad ).setDelay(3f);
    LeanTween.rotateY( gameObject, 0, 2).setEase( LeanTweenType.easeInOutQuad ).setDelay(3f);
    LeanTween.rotateZ( gameObject, 0, 2).setEase( LeanTweenType.easeInOutQuad ).setDelay(3f);

}
 

function Update () {
	
  /*  counter++;

    if (counter >= 100)
    {win = true;}

    if (win == true){

        LeanTween.moveX( gameObject, -40, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
        LeanTween.moveY( gameObject, 10, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
        LeanTween.moveZ( gameObject, 15, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    
        LeanTween.rotateX( gameObject, -5, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
        LeanTween.rotateY( gameObject, -200, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
        LeanTween.rotateZ( gameObject, -10, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);

    };
*/
}
