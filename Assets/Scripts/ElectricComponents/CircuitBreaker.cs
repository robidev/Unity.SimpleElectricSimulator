using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CircuitBreaker : Switch
{
    private bool oldSwitchConducting;
    private ConductingEquipment input;

    public override void Initialize(ConductingEquipment reference)
    {
        input = reference;
        oldSwitchConducting = SwitchConducting;
        foreach(ConductingEquipment output in outputs)
            output.Initialize(this);
    }
    public override void Step()
    {
        float tmpCurrent = 0;
        if(Destroyed)
        {
            voltage = 0;//not conducting anymore
        }
        else
        {
            if(SwitchConducting == true)
                voltage = input.voltage;
            else
                voltage = 0;
        }
        
        foreach(ConductingEquipment output in outputs)
        {
            output.Step();
            tmpCurrent += output.current;
        }

        current = tmpCurrent;
        if(oldSwitchConducting == false && SwitchConducting == true) //if switch is closed this frame
        {
            Debug.Log(name + ": Bang close");
        }
        if(oldSwitchConducting == true && SwitchConducting == false) //if switch is opened this frame
        {
            Debug.Log(name + ": Bang open");
        }
        oldSwitchConducting = SwitchConducting;
        
        base.Step();
    }
}
