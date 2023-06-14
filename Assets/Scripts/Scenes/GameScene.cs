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
        Debug.Log("랜덤 맵 생성");
        yield return new WaitForSeconds(1f);

        progress = 0.2f;
        Debug.Log("랜덤 몬스터 생성");
        // 랜덤 몬스터 생성 및 배치
        GameObject wolf = Instantiate(wolfPrefab, wolfPosition.position, wolfPosition.rotation);
        yield return new WaitForSeconds(1f);

        progress = 0.4f;
        Debug.Log("플레이어 배치");
        GameObject player = Instantiate(playerPrefab, playerPosition.position, playerPosition.rotation);
        yield return new WaitForSeconds(1f);

        progress = 0.6f;
        Debug.Log("카메라");
        freeLookCamera.Follow = player.transform;
        freeLookCamera.LookAt = player.transform;
        yield return new WaitForSeconds(1f);

        progress = 1.0f;
    }
}
