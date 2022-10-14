using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AutoBuildingCreator))]
public class AutoBuildingEditor : Editor
{


    bool[] fractionPressed;
    bool[] buildingPressed;
    FractionData[] fractions;
    FractionData selectedFraction;
    AutoBuildingCreator autoBuild;
    TeamData bufferTeam;
    MapTestGenerator mapTest;

    List<FractionData.BuildingData> selectedBuildings;
    public override void OnInspectorGUI()
    {
        autoBuild = target as AutoBuildingCreator;
        DrawDefaultInspector();

        if(mapTest == null)
        {
            DrawEnableTestMap();
        }
        else
        {
            DrawFractionsButton();
            DrawBuildings(selectedFraction);
            DrawGenerationButtons();
        }
    }

    public void DrawFractionsButton()
    {
        fractions = Resources.LoadAll<FractionData>("Fractions");
        if (fractionPressed == null || fractionPressed.Length < fractions.Length)
        {
            fractionPressed = new bool[fractions.Length];
        }
        GUILayout.BeginHorizontal();
        for (int i = 0; i < fractions.Length; i++)
        {
            fractionPressed[i] = GUILayout.Toggle(fractionPressed[i], fractions[i].name, "Button",GUILayout.Height(50));
            if (fractionPressed[i])
            {
                for(int y = 0; y < fractionPressed.Length; y++)
                {
                    fractionPressed[y] = false;
                }
                fractionPressed[i] = true;
                selectedFraction = fractions[i];
            }
        }
        GUILayout.EndHorizontal();
    }
    public void DrawBuildings(FractionData selectedFraction)
    {
        if (selectedFraction == null) return;

        if (buildingPressed == null || buildingPressed.Length < selectedFraction.GetAllBuildings().Length)
        {
            buildingPressed = new bool[selectedFraction.GetAllBuildings().Length];
        }

        GUILayout.BeginVertical();
        for (int i = 0; i < selectedFraction.GetAllBuildings().Length; i++)
        {
            buildingPressed[i] = GUILayout.Toggle(buildingPressed[i], selectedFraction.GetAllBuildings()[i].name, "Button", GUILayout.Height(25));
        }
        GUILayout.EndVertical();
        selectedBuildings = new List<FractionData.BuildingData>();
        for(int i = 0; i < buildingPressed.Length; i++)
        {
            if (buildingPressed[i])
            {
                selectedBuildings.Add(selectedFraction.GetAllBuildings()[i]);
            }
        }
    }
    public void DrawGenerationButtons()
    {
     //   GUILayout.BeginHorizontal();
     //   if (GUILayout.Button("GENERATE", GUILayout.Height(50)))
     //   {
     //       autoBuild.GenerateWallsForTeam(bufferTeam,selectedBuildings.ToArray());
     //       mapTest.ActiveSectionColors(autoBuild.virtualGrid);
     //   }
     //   if(GUILayout.Button("AutoWalls", GUILayout.Height(50)))
     //   {
     //       mapTest.ClearMap();
     //       autoBuild.GenerateWallsForTeam(bufferTeam);
     //       mapTest.ActiveSectionColors(autoBuild.virtualGrid);
     //   }
     //   if (GUILayout.Button("Clear", GUILayout.Height(50)))
     //   {
     //       bufferTeam = new TeamData();
     //       mapTest.ClearMap();
     //   }
     //   GUILayout.EndHorizontal();
    }

    public void DrawEnableTestMap()
    {
        if (GUILayout.Button("GenerationTestMap", GUILayout.Height(50)))
        {
            mapTest = new MapTestGenerator(autoBuild.mapMeshPrefabForTest, autoBuild.virtualGridScale);
            bufferTeam = new TeamData();
        }

    }

}
