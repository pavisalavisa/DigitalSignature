package com.example.digitalsignatureapi.common;

import eu.europa.esig.dss.policy.jaxb.Level;
import eu.europa.esig.dss.policy.jaxb.LevelConstraint;

public class LevelConstraintFactory {
    public static LevelConstraint CreateLevelConstraint(Level level){
        LevelConstraint lc = new LevelConstraint();

        lc.setLevel(level);

        return lc;
    }

    public static LevelConstraint CreateInformConstraint(){
        return CreateLevelConstraint(Level.INFORM);
    }
    public static LevelConstraint CreateFailConstraint(){
        return CreateLevelConstraint(Level.FAIL);
    }
    public static LevelConstraint CreateWarnConstraint(){
        return CreateLevelConstraint(Level.WARN);
    }
    public static LevelConstraint CreateIgnoreConstraint(){
        return CreateLevelConstraint(Level.IGNORE);
    }
}
