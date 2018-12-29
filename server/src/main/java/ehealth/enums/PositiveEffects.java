package ehealth.enums;
/*
 *****************************************************************************
 *   The confidential and proprietary information contained in this file may
 *   only be used by a person authorised under and to the extent permitted
 *   by a subsisting licensing agreement from ARM Limited or its affiliates.
 *
 *          (C) COPYRIGHT 2013-2018 ARM Limited or its affiliates.
 *              ALL RIGHTS RESERVED
 *
 *   This entire notice must be reproduced on all copies of this file
 *   and copies of this file may only be made by a person if such person is
 *   permitted to do so under the terms of a subsisting license agreement
 *   from ARM Limited or its affiliates.
 *****************************************************************************/


import com.fasterxml.jackson.annotation.JsonValue;

import java.util.HashMap;
import java.util.Map;


public enum PositiveEffects implements BaseEnumEffect {
    AROUSED(1),
    GIGGLY(2),
    FOCUSED(3),
    SLEEPY(4),
    TINGLY(5),
    UPLIFTED(6),
    TALKATIVE(7),
    ENERGETIC(8),
    CREATIVE(9),
    HAPPY(10),
    EUPHORIC(11),
    HUNGRY(12),
    RELAXED(13);

    private static Map<Integer, PositiveEffects> intStatusToValue = new HashMap<>();
    private final int value;

    PositiveEffects(int value) {
        this.value = value;
    }

    static {
        for (PositiveEffects effectType : PositiveEffects.values()) {
            intStatusToValue.put(effectType.value, effectType);
        }
    }

    public static PositiveEffects valueOf(int effectValue) {
        return intStatusToValue.get(effectValue);

    }

    @JsonValue
    @Override
    public String getEffect() {
        return valueOf(this.value).name();
    }

    @Override
    public String toString() {
        return valueOf(this.value).name();
    }
}

