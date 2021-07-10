package com.example.digitalsignatureapi.common;

import eu.europa.esig.dss.enumerations.SignatureLevel;

public enum SignatureProfile {
    B,
    T,
    LT,
    LTA;

    public SignatureLevel ToPadesSignatureLevel(){
        switch (this){
            case B:
                return SignatureLevel.PAdES_BASELINE_B;
            case T:
                return SignatureLevel.PAdES_BASELINE_T;
            case LT:
                return SignatureLevel.PAdES_BASELINE_LT;
            case LTA:
                return SignatureLevel.PAdES_BASELINE_LTA;
            default:
                throw new IllegalArgumentException();
        }
    }

    public SignatureLevel ToXadesSignatureLevel(){
        switch (this){
            case B:
                return SignatureLevel.XAdES_BASELINE_B;
            case T:
                return SignatureLevel.XAdES_BASELINE_T;
            case LT:
                return SignatureLevel.XAdES_BASELINE_LT;
            case LTA:
                return SignatureLevel.XAdES_BASELINE_LTA;
            default:
                throw new IllegalArgumentException();
        }
    }
}
