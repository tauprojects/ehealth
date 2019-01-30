package ehealth.enums;

import com.fasterxml.jackson.annotation.JsonValue;

import java.util.HashMap;
import java.util.Map;

public enum NegativeEffects implements BaseEnumEffect {
    DIZZY(0),
    DRY_MOUTH(1),
    PARANOID(2),
    DRY_EYES(3),
    ANXIOUS(4);


    private static Map<String, NegativeEffects> stringToValue = new HashMap<>();
    public final int value;

    NegativeEffects(int value) {

        this.value = value;
    }
    public int getValue(){
        return value;
    }

    static {
        for (NegativeEffects effectType : NegativeEffects.values()) {
            stringToValue.put(effectType.toString(), effectType);
        }
    }

    public static NegativeEffects valueOf(int effectValue) {
        return stringToValue.get(effectValue);

    }
    @JsonValue
    @Override
    public String getEffect() {
        return valueOf(this.toString()).name();
    }
}