using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour
{
   [SerializeField]
   private Camera cam;
   
   private Vector3 velocity;
   private Vector3 rotation;
   private Vector3 cameraRotation;
   private Rigidbody rb;

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
   
   public void RotateCamera(Vector3 _cameraRotation)
   {
      cameraRotation = _cameraRotation;
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
      rb.MoveRotation(rb.rotation * Quaternion.Euler(rotation));
      cam.transform.Rotate(-cameraRotation);
   }
   
}
