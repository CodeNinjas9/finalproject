using UnityEngine;

public class Player : MonoBehaviour
{
    public CameraSettings cameraSettings;
    public Voxel selectedVoxel;

    float range;
    float xRotation;
    float yRotation;
    bool grounded;
    float breakSeconds;
    GameObject targetBlock;
    RaycastHit targetRaycastHit;

    void Start()
    {
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }

    void Update()
    {

        CheckRotation();


        CheckTargetBlock();

        if (Input.GetButton("Fire1")) { TryBreakBlock(); }

        else { breakSeconds = 0; }

        if (Input.GetButton("Fire2")) {TryPlaceBlock(); }

    }

    void CheckRotation() {

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cameraSettings.sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cameraSettings.sensitivityY;

        yRotation += mouseX;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        cameraSettings.camera.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);

        cameraSettings.camera.transform.SetParent(null);

        transform.rotation = Quaternion.Euler(0, yRotation, 0);

        cameraSettings.camera.transform.SetParent(transform);

    }


    void CheckTargetBlock() {

        RaycastHit hit;
        Ray ray = cameraSettings.camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit)) {

            Transform objectHit = hit.transform;

            targetRaycastHit = hit;

            if (!objectHit.GetComponent<DestroyBlock>()) { return; }

            if (Vector3.Distance(transform.position, objectHit.position) > range) { return; }

            targetBlock = objectHit.gameObject;
        }

    }

    void TryBreakBlock() {
        VoxelChunk chunk = targetBlock.GetComponent<VoxelChunk>();
        chunk.SetVoxel((int)targetRaycastHit.point.x, (int)targetRaycastHit.point.y, (int)targetRaycastHit.point.z, new Voxel());
    }
    void TryPlaceBlock()
    {
        VoxelChunk chunk = targetBlock.GetComponent<VoxelChunk>();
        chunk.SetVoxel((int)targetRaycastHit.point.x, (int)targetRaycastHit.point.y, (int)targetRaycastHit.point.z, selectedVoxel);
    }


    private void OnTriggerStay(Collider other) {

        grounded = true;

    }

    private void OnTriggerExit(Collider other) {

        grounded = false;

    }


    [System.Serializable]
    public struct CameraSettings {

        public Camera camera;
        public float sensitivityX;
        public float sensitivityY;

    }

}