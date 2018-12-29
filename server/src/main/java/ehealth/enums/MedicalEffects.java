package ehealth.enums;

import com.fasterxml.jackson.annotation.JsonValue;

import java.util.HashMap;
import java.util.Map;

public enum MedicalEffects implements BaseEnumEffect {
    SEIZURES(1),
    MUSCLE_SPASMS(2),
    SPASTICITY(3),
    INFLAMMATION(4),
    EYE_PRESSURE(5),
    HEADACHES(6),
    FATIGUE(7),
    NAUSEA(8),
    LACK_OF_APPETITE(9),
    CRAMPS(10),
    STRESS(11),
    PAIN(12),
    DEPRESSION(13);

    private static Map<Integer, MedicalEffects> intStatusToValue = new HashMap<>();
    private final int value;

    MedicalEffects(int value) {
        this.value = value;
    }

    static {
        for (MedicalEffects effectType : MedicalEffects.values()) {
            intStatusToValue.put(effectType.value, effectType);
        }
    }

    public static MedicalEffects valueOf(int effectValue) {
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
