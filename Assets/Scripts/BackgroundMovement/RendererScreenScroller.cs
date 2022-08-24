using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RendererScreenScroller : MonoBehaviour {
    
    [SerializeField] private new Renderer renderer;

    [Header("Scroll")]
    [SerializeField] float scrollSpeed = 1.0f;

    private List<Renderer> rendererPool;
    private float rendererX;
    private int firstIndex;

    public void SetScrollSpeed(float scrollSpeed){
        this.scrollSpeed = scrollSpeed;
    }
    
    private void Awake() {
        rendererPool = new List<Renderer>();

        var clone = CreateClone();
        rendererPool.Add(renderer);
        rendererPool.Add(clone);

        rendererX = CalculateRendererSize();
        PositionSprites(0);
    }

    
    private Renderer CreateClone() {
        var clone = Instantiate(renderer);
        clone.transform.parent = transform;
        clone.transform.localScale = renderer.transform.localScale;

        return clone;
    }

    protected float CalculateRendererSize() {
        return renderer.bounds.size.x;
    }

    private void PositionSprites(int index) {

        Renderer first;
        Renderer second;

        if(index == 0) {
            first = rendererPool[0];
            second = rendererPool[1];
        } else {
            first = rendererPool[1];
            second = rendererPool[0];
        }
        
        var position = first.transform.position;
        second.transform.position = new Vector3(position.x + rendererX, position.y, 0);

        firstIndex = index;
    }

    private void FixedUpdate() {

        if(scrollSpeed == 0) {
            return;
        }

        foreach(var renderer in rendererPool) {
            var position = renderer.transform.position;
            renderer.transform.position = new Vector3(position.x - scrollSpeed * Time.fixedDeltaTime, position.y, position.z);
        }

        if(IsFirstRendererOutOfBounds()){
            MoveRenderer();
        }
    }

    private bool IsFirstRendererOutOfBounds() {
        var first = rendererPool[firstIndex];
        
        var cam = Camera.main;
        var planes = GeometryUtility.CalculateFrustumPlanes(cam);

        if (GeometryUtility.TestPlanesAABB(planes, first.bounds)) {
            return false;
        }
        else {
            return true;
        }
    }

    private void MoveRenderer() {
        if(firstIndex == 0){
            PositionSprites(1);
        } else {
            PositionSprites(0);
        }
    }
}
