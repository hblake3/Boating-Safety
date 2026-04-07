using NUnit.Framework;
using UnityEngine;

public class GearSelectionManagerTests
{
    [Test]
    public void CanCreateGearSelectionManager()
    {
        var gameObject = new GameObject();
        var manager = gameObject.AddComponent<GearSelectionManager>();

        Assert.IsNotNull(manager);
    }
}