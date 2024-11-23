using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoneNamer : MonoBehaviour
{
    public BoneList Bone_List;
    public TMP_Text Bone_Name;
    public AudioSource audioSource;
    public GameObject BoneViewPos;
    public AudioClip[] audioClips;

    public GameObject[] IKs;

    public GameObject ViewButton;
    public GameObject NoViewButton;

    private GameObject[] Bones;
    private Vector3 BonePrevLocation;

    // Start is called before the first frame update
    void Start()
    {
        Bones = Bone_List.Bones;
        ViewButton.SetActive(true);
        NoViewButton.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var Bone in Bones)
        {
            if (Bone.GetComponent<Selected>().selected)
            {
                // Get the object's name without .L or .R
                string boneName = Bone.name.Replace(".L", "").Replace(".R", "").Replace("_"," ");

                // Update the text
                Bone_Name.text = boneName;
            }
        }
    }

    public void PlayAudioByName()
    {
        string boneName = Bone_Name.text;

        foreach (var audioClip in audioClips)
        {
            if (audioClip.name == boneName)
            {
                audioSource.clip = audioClip;
                audioSource.Play();
                break;
            }
        }
    }

    public void ViewBone()
    {
        foreach (var Bone in Bones)
        {
            if (Bone.GetComponent<Selected>().selected)
            {
                // Store the initial position of the bone
                BonePrevLocation = Bone.transform.position;

                // Scale the selected bone by 2 times
                Vector3 initialScale = Bone.transform.localScale;
                Vector3 targetScale = initialScale * 2f;

                // Disable other bones
                foreach (var otherBone in Bones)
                {
                    if (otherBone != Bone)
                    {
                        otherBone.SetActive(false);
                    }
                }

                foreach (var IK in IKs)
                { 
                    IK.SetActive(false);
                }

                // Move the selected bone to BoneViewPos and scale it by lerping
                StartCoroutine(MoveAndScaleBoneToViewPosition(Bone.transform, initialScale, targetScale));
            }
        }
    }

    IEnumerator MoveAndScaleBoneToViewPosition(Transform boneTransform, Vector3 initialScale, Vector3 targetScale)
    {
        ViewButton.SetActive(false);
        float duration = 1f;
        float elapsed = 0f;
        Vector3 initialPosition = boneTransform.position;
        Vector3 targetPosition = BoneViewPos.transform.position;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            boneTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            boneTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        boneTransform.position = targetPosition;
        boneTransform.localScale = targetScale;
        NoViewButton.SetActive(true);
    }

    public void ResetView()
    {
        foreach (var Bone in Bones)
        {
            if (Bone.GetComponent<Selected>().selected)
            {
                // Reset the bone to its original position and scale
                StartCoroutine(MoveBoneToPreviousPosition(Bone.transform));
            }
        }
    }

    IEnumerator MoveBoneToPreviousPosition(Transform boneTransform)
    {
        NoViewButton.SetActive(false);
        float duration = 1f;
        float elapsed = 0f;
        Vector3 initialPosition = boneTransform.position;
        Vector3 targetPosition = BonePrevLocation;
        Vector3 initialScale = boneTransform.localScale;
        Vector3 targetScale = initialScale / 2f;
        while (elapsed < duration)
        {
            float t = elapsed / duration;
            boneTransform.position = Vector3.Lerp(initialPosition, targetPosition, t);
            boneTransform.localScale = Vector3.Lerp(initialScale, targetScale, t);
            elapsed += Time.deltaTime;
            yield return null;
        }
        boneTransform.position = targetPosition;
        boneTransform.localScale = targetScale;
        ViewButton.SetActive(true);
        // Enable all bones
        foreach (var otherBone in Bones)
        {
            otherBone.SetActive(true);
        }

        foreach (var IK in IKs)
        {
            IK.SetActive(true);
        }
    }

}
