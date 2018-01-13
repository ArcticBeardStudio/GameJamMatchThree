using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class PlayerTests
{
    [Test]
    public void Player_ApplyDamage_HealthLost()
    {
        GameObject go = new GameObject();
        Player player = go.AddComponent<Player>();
        int prevHealth = player.health;

        player.ApplyDamage(10);

        Assert.Less(player.health, prevHealth);
    }
}
