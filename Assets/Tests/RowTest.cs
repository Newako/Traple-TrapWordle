using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class RowTests
{
    [Test]
    public void RowWord_ReturnsCorrectString()
    {

        GameObject rowObj = new GameObject();
        Row row = rowObj.AddComponent<Row>();


        var letters = new[] { 'C', 'A', 'T' };
        rowObj.transform.DetachChildren();
        for (int i = 0; i < letters.Length; i++)
        {
            GameObject tileObj = new GameObject($"Tile{i}");
            tileObj.transform.parent = rowObj.transform;

            Tile tile = tileObj.AddComponent<Tile>();
            var text = tileObj.AddComponent<TMPro.TextMeshProUGUI>();
            tileObj.AddComponent<UnityEngine.UI.Image>();
            tileObj.AddComponent<UnityEngine.UI.Outline>();

            tile.SetLetter(letters[i]);
        }


        rowObj.SetActive(false); rowObj.SetActive(true);


        string result = row.word;


        Assert.AreEqual("CAT", result);
    }
}

