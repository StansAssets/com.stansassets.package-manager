using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class StansAssetsPackageManagerTests
{
    // A Test behaves as an ordinary method
    [Test]
    public void StansAssetsPackageManagerTestsSimplePasses()
    {
        Assert.AreEqual(1, 1);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator StansAssetsPackageManagerTestsWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        for (var i = 0; i < 10; i+=2)
        {
            Assert.AreEqual(i % 2 == 0, true);
            yield return null;
        }
    }
}