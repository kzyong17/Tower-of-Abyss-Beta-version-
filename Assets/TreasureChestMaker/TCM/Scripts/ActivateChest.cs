﻿using UnityEngine;
using System.Collections;
using TMPro;

public class ActivateChest : MonoBehaviour
{

	public Transform lid, lidOpen, lidClose;    // Lid, Lid open rotation, Lid close rotation
	public float openSpeed = 5F;                // Opening speed
	public bool canClose;                       // Can the chest be closed

	[HideInInspector]
	public bool _open;                          // Is the chest opened

	public bool canOpen;

	public bool doneOpen;

	private StarterAssets.ThirdPersonController tpc;
	PlayerStats PlayerS;

	public GameObject Shiny;
	public float EXPGet;

	public GameObject EXPPlus;
	private Animator EXPAnim;
	public TextMeshProUGUI EXPAmount;

	public GameObject ItemTextObject;
	private Animator ItemAnim;
	public TextMeshProUGUI ItemName;

	public GameObject RewardTextObject;
	private Animator RewardAnim;

	public float RewardTimer;
	public bool Rewarding;

	public float Roll;


	private void Start()
    {
		tpc = FindObjectOfType<StarterAssets.ThirdPersonController>();
		PlayerS = FindObjectOfType<PlayerStats>();

		EXPAnim = EXPPlus.GetComponent<Animator>();
		RewardAnim = RewardTextObject.GetComponent<Animator>();
		ItemAnim = ItemTextObject.GetComponent<Animator>();
	}

    void Update()
	{
		if (Rewarding)
        {
			RewardTimer += Time.deltaTime;

			if (RewardTimer > 5)
            {
				Rewarding = false;
				EXPPlus.SetActive(false);
				RewardTextObject.SetActive(false);
				ItemTextObject.SetActive(false);
			}				
        }

		if (!Rewarding)
			RewardTimer = 0;

		if (_open)
		{
			ChestClicked(lidOpen.rotation);

			if (!doneOpen)
			{
				Roll = Random.Range(1, 100);

				if (Roll <= 30)
                {
					ItemName.text = "Greater Healing Potion x1";
                }

				if (Roll >= 31)
				{
					ItemName.text = "Lesser Healing Potion x1";
				}

				GameObject hObject = Instantiate(Shiny, new Vector3(transform.position.x, (transform.position.y), transform.position.z), Quaternion.Euler(new Vector3(90, Random.Range(0, 360), 0))) as GameObject;
				Destroy(hObject, 1);

				EXPGet = Random.Range(15, 20);
				PlayerS.EXP += EXPGet;

				EXPAmount.text = "EXP+" + EXPGet.ToString("F0");
				EXPPlus.SetActive(true);
				EXPAnim.Play("EXP_PLUS_ANIM");

				RewardTextObject.SetActive(true);
				RewardAnim.Play("EXP_PLUS_ANIM");

				ItemTextObject.SetActive(true);
				ItemAnim.Play("EXP_PLUS_ANIM");

				doneOpen = true;
				Rewarding = true;
			}
		}
		else
		{
			ChestClicked(lidClose.rotation);
		}
	}

	// Rotate the lid to the requested rotation
	void ChestClicked(Quaternion toRot)
	{
		if (lid.rotation != toRot)
		{
			lid.rotation = Quaternion.Lerp(lid.rotation, toRot, Time.deltaTime * openSpeed);
		}
	}

	void OnMouseDown()
	{
		if (canClose) _open = !_open; else _open = true;
	}

	private void OnTriggerStay(Collider other)
	{
		if (other.gameObject.tag == "Player")
        {
			canOpen = true;
			tpc.TheChest = this.gameObject;
			tpc.canChest = true;
        }
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			canOpen = false;
			tpc.canChest = false;
		}
	}
}