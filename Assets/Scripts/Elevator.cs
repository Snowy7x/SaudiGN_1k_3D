using UnityEngine;

public enum ElvPlace
{
    Up,
    Down
}

public class Elevator : MonoBehaviour
{
    public ElevatorDoor doorUp;
    public ElevatorDoor doorDown;

    public Transform _pointUp;
    public Transform _pointDown;
    public float speed = 10f;

    private int _dir;
    private Vector3 _destination;
    private bool _isMoving;

    public Transform player;
    [SerializeField] private AudioSource audioSource;

    private void FixedUpdate()
    {
        if (_isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, _destination, speed * Time.deltaTime);
            if ((_dir == -1 && transform.position.y <= _destination.y) || (_dir == 1 && transform.position.y >= _destination.y))
            {
                _isMoving = false;
                if (GetPlace() == ElvPlace.Up)
                {
                    doorUp.Open();
                    doorDown.Close();
                    doorUp.just_got_out = true;
                }
                else
                {
                    doorDown.Open();
                    doorUp.Close();
                    doorDown.just_got_out = true;
                }
                player.parent = null;
                SoundManager.Instance.FadeOut("Left", 0.2f);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.transform;
            player.parent = transform;
            Go();
        }
    }

    void Go()
    {
        _dir = GetPlace() == ElvPlace.Up ? -1 : 1;
        if (_dir == 1)
        { if (!doorUp.IsClosed()) doorUp.Close();
        }else if(doorDown.IsClosed()) doorDown.Close();
        _destination = GetPlace() == ElvPlace.Up ? _pointDown.position : _pointUp.position;
        _isMoving = true;
        SoundManager.Instance.Play("Left", audioSource);
    }

    public ElvPlace GetPlace()
    {
        return transform.position.y >= _pointUp.position.y ? ElvPlace.Up : ElvPlace.Down;
    }
}