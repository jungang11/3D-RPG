using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameScene : BaseScene
{
    public GameObject playerPrefab;
    public Transform playerPosition;

    public GameObject wolfPrefab;
    public Transform wolfPosition;
    public CinemachineFreeLook freeLookCamera;

    protected override IEnumerator LoadingRoutine()
    {
        progress = 0f;
        Debug.Log("���� �� ����");
        yield return new WaitForSeconds(1f);

        progress = 0.2f;
        Debug.Log("���� ���� ����");
        // ���� ���� ���� �� ��ġ
        GameObject wolf = Instantiate(wolfPrefab, wolfPosition.position, wolfPosition.rotation);
        yield return new WaitForSeconds(1f);

        progress = 0.4f;
        Debug.Log("�÷��̾� ��ġ");
        GameObject player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
        yield return new WaitForSeconds(1f);

        progress = 0.6f;
        Debug.Log("ī�޶�");
        freeLookCamera.Follow = player.transform;
        freeLookCamera.LookAt = player.transform;
        yield return new WaitForSeconds(1f);

        progress = 1.0f;
    }
}
