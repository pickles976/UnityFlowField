using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// Static class called for Flowfield generation
public static class FlowFieldProvider
{

    private static Dictionary<Tuple<int,int>,Vector3> flowField;
    private static CustomGrid cg;

    public static void GenerateNewField(CustomGrid customGrid, Dictionary<Tuple<int,int>,int> blocked, Vector3 b1, Vector3 b2, Vector3 dest,int cellsPerFrame){
        flowField = FlowFieldFactory.GenerateFlowField(customGrid,blocked,b1,b2,dest,cellsPerFrame);
        cg = customGrid;
    }

    public static Vector3 GetVector(Vector3 position){
        if(flowField.ContainsKey(cg.worldToCell(position))){
            return flowField[cg.worldToCell(position)];
        }
        return Vector3.zero;
    }

    public static Dictionary<Tuple<int,int>,Vector3> GetField(){
        return flowField;
    }
}