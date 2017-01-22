#pragma strict

 
function Start()
{

    LeanTween.moveX( gameObject, 0, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    LeanTween.moveY( gameObject, 100, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    LeanTween.moveZ( gameObject, -25, 2).setEase( LeanTweenType.easeInOutSine ).setDelay(3f);
    
    LeanTween.rotateX( gameObject, 70, 2).setEase( LeanTweenType.easeInOutQuad ).setDelay(3f);
    LeanTween.rotateY( gameObject, 0, 2).setEase( LeanTweenType.easeInOutQuad ).setDelay(3f);
    LeanTween.rotateZ( gameObject, 0, 2).setEase( LeanTweenType.easeInOutQuad ).setDelay(3f);

}
 

function Update () {
	
}
