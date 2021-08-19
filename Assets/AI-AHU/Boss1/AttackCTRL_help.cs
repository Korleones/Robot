using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCTRL_help : MonoBehaviour
{
    Rigidbody2D Rig;
    float DownEffection;
    float UpEffection;
    private void Start()
    {
        Rig = PlayerCTRL.Player.gameObject.GetComponent<Rigidbody2D>();
        DownEffection = PlayerCTRL.Player.gameObject.GetComponent<AttackCTRL>().DownEffection;
        UpEffection = PlayerCTRL.Player.gameObject.GetComponent<AttackCTRL>().UpEffection;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            DamageCTRL.TakeDamage(collision.gameObject);
            PlayerCTRL.Player.WasDashed = false;
            if (Input.GetAxisRaw("Vertical") < 0)
            {
                Rig.velocity = new Vector2(Rig.velocity.x, DownEffection);
            }
            else if (Input.GetAxisRaw("Vertical") > 0)
            {
                Rig.velocity = new Vector2(Rig.velocity.x, Rig.velocity.y + UpEffection);
            }
        }
        if (collision.gameObject.CompareTag("ReversibleThorn"))
        {
            collision.gameObject.GetComponent<ReversiblThorn>().Refresh();
        }
        if (collision.gameObject.CompareTag("Tear"))
        {
            PlayerCTRL.Player.WasDashed = false;
            Destroy(collision.gameObject);
        }
    }
}