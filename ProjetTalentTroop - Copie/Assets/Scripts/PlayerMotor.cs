using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
   [SerializeField]
   private Camera cam;
   
   private Vector3 velocity;
   private Vector3 rotation;
   private float cameraRotationX = 0f;
   private float currentCameraRotationX = 0f;
   private Rigidbody rb;

   [SerializeField]
   private float cameraRotationLimit = 85f;

   private void Start()
   {
      rb = GetComponent<Rigidbody>();
   }

   public void Move(Vector3 _velocity)
   {
      velocity = _velocity;
   }

   public void Rotate(Vector3 _rotation)
   {
      rotation = _rotation;
   }
   
   public void RotateCamera(float _cameraRotationX)
   {
      cameraRotationX = _cameraRotationX;
   }

   private void FixedUpdate()
   // cette methode est pareil que Update() mais plus adapté pour la physique
   {
      PerformMovement();
      PerformRotation();
   }

   private void PerformMovement()
   // cette methode calcule la nouvelle position du rb 
   {
      if (velocity != Vector3.zero)
      {
         rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
      }
   }

   private void PerformRotation()
   // cette methode calcule la nouvelle rotation du rb ( la classe Euler transforme le vecteur3 en Quaternion)
   {
      // on calcule la rotation de la caméra
      rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
      currentCameraRotationX -= cameraRotationX;
      currentCameraRotationX = Mathf.Clamp(currentCameraRotationX,-cameraRotationLimit,cameraRotationLimit);

      // on applique la rotation de la caméra
      cam.transform.localEulerAngles = new Vector3(currentCameraRotationX,0f,0f);
   }
   
}
