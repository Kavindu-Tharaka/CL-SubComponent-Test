using UnityEngine;
using System.Collections;
using Photon.Pun;


namespace Com.Cdap.CLSubComponent
{
    public class PlayerAnimatorManager : MonoBehaviourPun
    {

        #region Private Serializable Fields

        /// <summary>
        /// Smooth the turnings and control the turning rsadius
        /// </summary>
        [Tooltip("Smooth the turnings and control the turning radius")]
        [SerializeField]
        private float directionDampTime = 0.001f;

        #endregion

        #region Private Serializable Fields
        /// <summary>
        /// as a reference to the Animator Component
        /// </summary>
        private Animator animator;

        #endregion

        #region MonoBehaviour Callbacks

        void Start()
        {
            animator = GetComponent<Animator>();

            if (!animator)
            {
                Debug.LogError("PlayerAnimatorManager is Missing Animator Component", this);
            }
        }


        void Update()
        {
            if (photonView.IsMine == false && PhotonNetwork.IsConnected == true)
            {
                return;
            }

            if (!animator)
            {
                return;
            }

            // deal with Jumping
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            // only allow jumping if we are running.
            if (stateInfo.IsName("Base Layer.Run"))
            {
                // When using trigger parameter
                if (Input.GetButtonDown("Fire2"))
                {
                    animator.SetTrigger("Jump");
                }
            }

            float h = Input.GetAxis("Horizontal");  // press A, D or Left arrow, Right arrow
            float v = Input.GetAxis("Vertical");  // press W, S or Up arrow, Down arrow
            if (v < 0)
            {
                v = 0; // to disable backward move 
            }

            // get square value to always having a positive absolute value as well as adding some easing
            // add both inputs to control Speed
            animator.SetFloat("Speed", h * h + v * v);

            animator.SetFloat("Direction", h, directionDampTime, Time.deltaTime);

        }


        #endregion
    }
}