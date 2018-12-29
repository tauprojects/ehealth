package ehealth.enums;

import com.fasterxml.jackson.annotation.JsonValue;

import java.util.HashMap;
import java.util.List;
import java.util.Map;

public enum NegativeEffects implements BaseEnumEffect {
    DIZZY(1),
    DRY_MOUTH(2),
    PARANOID(3),
    DRY_EYES(4),
    ANXIOUS(5);

    private static Map<Integer, NegativeEffects> intStatusToValue = new HashMap<>();
    private final int value;

    NegativeEffects(int value) {
        this.value = value;
    }

    static {
        for (NegativeEffects effectType : NegativeEffects.values()) {
            intStatusToValue.put(effectType.value, effectType);
        }
    }

    public static NegativeEffects valueOf(int effectValue) {
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