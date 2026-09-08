using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class ReaparicionTests
{
    private GameObject playerGameObject;
    private controlar scriptControlar;

    [SetUp]
    public void Setup()
    {
        playerGameObject = new GameObject("PlayerReaparicionTest");

        playerGameObject.AddComponent<CapsuleCollider>();
        playerGameObject.AddComponent<Rigidbody>();

        Animator animator = playerGameObject.AddComponent<Animator>();
        animator.runtimeAnimatorController = new UnityEditor.Animations.AnimatorController();

        scriptControlar = playerGameObject.AddComponent<controlar>();
    }

    [TearDown]
    public void Teardown()
    {
        Object.Destroy(playerGameObject);
    }

    [UnityTest]
    public IEnumerator CP10_CaidaFueraDeLaPista_ReapareceEnZonaSegura()
    {
        Vector3 zonaSegura = new Vector3(0f, 2f, 0f);
        scriptControlar.checkPoint = zonaSegura;
        playerGameObject.transform.position = zonaSegura;

        Vector3 posicionVacio = new Vector3(0f, -50f, 0f);
        playerGameObject.transform.position = posicionVacio;

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(posicionVacio, playerGameObject.transform.position, "El personaje no fue desplazado al vacío.");

        scriptControlar.LoadCheckPoint();

        yield return new WaitForFixedUpdate();

        Assert.AreEqual(zonaSegura, playerGameObject.transform.position, "El personaje no reapareció en la posición del Checkpoint seguro.");
    }
}