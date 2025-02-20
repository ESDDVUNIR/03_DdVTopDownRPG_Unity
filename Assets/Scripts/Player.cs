using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Inventory.Model;
using Unity.VisualScripting;
public class Player : MonoBehaviour
{
    private float inputH;
    private float inputV;
    private bool moviendo;
    private Vector3 puntoDestino;
    private Vector3 ultimoInput;
    private Vector3 puntoInteraccion;
    private Collider2D colliderDelante; //Indica el collider que tenemos delante.
    private Animator anim;

    [SerializeField] private float velocidadMovimiento;
    [SerializeField] private float radioInteraccion;
    [SerializeField]
    private Inventory_SO inventoryData;

    private bool interactuando;

    public bool Interactuando { get => interactuando; set => interactuando = value; }

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        //Guillermo - Convierte el Collider en Trigger
        if (colliderDelante)
        colliderDelante.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        LecturaInputs();

        MovimientoyAnimaciones();
    }

    private void MovimientoyAnimaciones()
    {
        //Ejecuto movimiento solo si estoy en una casilla y solo si hay input
        if (!interactuando && !moviendo && (inputH != 0 || inputV != 0)) //!= ==false
        {
            anim.SetBool("andando", true);
            anim.SetFloat("InputH", inputH);
            anim.SetFloat("InputV", inputV);
            //Actualizo cual fue mi ultimo input, cual va a ser mi punto destino y cual es mi punto interaccion.
            ultimoInput = new Vector3(inputH, inputV, 0);
            puntoDestino = transform.position + ultimoInput;
            puntoInteraccion = puntoDestino;

            colliderDelante = LanzarCheck();
            

            if (!colliderDelante)
            {
                StartCoroutine(Mover());
            }
        }
        else if (inputH == 0 && inputV == 0)
        {
            anim.SetBool("andando", false);
        }
    }

    private void LecturaInputs()
    {
        if (inputV == 0)
        {
            inputH = Input.GetAxisRaw("Horizontal");
        }
        if (inputH == 0)
        {
            inputV = Input.GetAxisRaw("Vertical");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            LanzarInteraccion();
        }
    }

    private void LanzarInteraccion()
    {
        colliderDelante = LanzarCheck();
        if (colliderDelante)
        //Guillermo - Convierte el Collider en Trigger
        //colliderDelante.isTrigger = true;
        {
            if (colliderDelante.gameObject.CompareTag("NPC"))
            {
                NPC npcScript = colliderDelante.gameObject.GetComponent<NPC>();
                npcScript.Interactuar();
            }
            // Guillermo - Item
            
            if (colliderDelante.GetComponent<Item>() != null)
            {
                Item item = colliderDelante.GetComponent<Item>();
                Debug.Log("COLLIDE WITH ITEM");
                
                int remainder = inventoryData.AddItem(item.InventoryItem, item.Quantity);
                
                if (remainder == 0)
                {
                    item.DestroyItem();
                    Debug.Log("Item picked up completely");
                }
                else
                {
                    item.Quantity = remainder;
                    Debug.Log("Some items remain");
                }
            }
        }
            
    }

    IEnumerator Mover()
    {
        moviendo = true;
        while (transform.position != puntoDestino)
        {
        transform.position = Vector3.MoveTowards(transform.position, puntoDestino, velocidadMovimiento * Time.deltaTime);
        yield return null;
        }
        //Ante un nuevo destino, necesito refrescar de nuevo punto interaccion.
        puntoInteraccion = transform.position + ultimoInput;
        moviendo = false;
    }
    private Collider2D LanzarCheck()
    {
       //return Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
       Collider2D hitCollider = Physics2D.OverlapCircle(puntoInteraccion, radioInteraccion);
    
    if (hitCollider != null)
    {
        Debug.Log("Detected: " + hitCollider.gameObject.name); // Debug to check detection

        if (hitCollider.CompareTag("Item")) 
        {
            Debug.Log("Setting isTrigger to TRUE for " + hitCollider.gameObject.name);
            hitCollider.isTrigger = true; // Set trigger before collision happens
            return null;
        }
        else{
            return hitCollider;
        }
        
    }
        return hitCollider;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(puntoInteraccion, radioInteraccion);
    }
}
