//package ehealth.service;
//
//import com.google.common.primitives.UnsignedInts;
//import com.google.gson.GsonBuilder;
//import com.google.gson.internal.LinkedTreeMap;
//import ehealth.client.data_objects.StrainObject;
//import ehealth.enums.MedicalEffects;
//import ehealth.enums.NegativeEffects;
//import ehealth.enums.PositiveEffects;
//
//
//import java.util.*;
//
//public class test {
//    public static GsonBuilder builder = new GsonBuilder();
//
//
//    public static List<StrainObject> getStrainsObjectList(Object o) {
//        List<StrainObject> allStainsList = new ArrayList<>();
//        Set<Map.Entry> set = ((LinkedTreeMap) o).entrySet();
//        List<String> keySet = new ArrayList<String>();
//        keySet.addAll(((LinkedTreeMap) o).keySet());
//        Iterator it = set.iterator();
//        for (int i = 0; i < keySet.size(); i++) {
//            StrainObject strainObject = new StrainObject();
//            Object entry = ((LinkedTreeMap) o).get(keySet.get(i));
//            strainObject.setName((keySet.get(i)));
//            strainObject.setId(((LinkedTreeMap) entry).get("id").toString());
//            strainObject.setRace(((LinkedTreeMap) entry).get("race").toString());
//            Object effects = ((LinkedTreeMap) entry).get("effects");
//            BitSet bitset = new BitSet();
//            for (String medical : (List<String>) ((LinkedTreeMap) effects).get("medical"))
//                bitset.set((MedicalEffects.valueOf(medical.toUpperCase().replace(" ", "_")).value));
//            strainObject.setMedical(bitset.toLongArray()[0]);
//            bitset.clear();
//            for (String negative : (List<String>) ((LinkedTreeMap) effects).get("negative"))
//                bitset.set((NegativeEffects.valueOf(negative.toUpperCase().replace(" ", "_")).value));
//            strainObject.setNegative(bitset.toLongArray()[0]);
//            bitset.clear();
//            for (String positive : (List<String>) ((LinkedTreeMap) effects).get("positive"))
//                bitset.set((PositiveEffects.valueOf(positive.toUpperCase().replace(" ", "_")).value));
//            strainObject.setPositive(bitset.toLongArray()[0]);
//            allStainsList.add(strainObject);
//        }
//        return allStainsList;
//    }
//
//    public static void main(String[] args) {
//        String allStrains = "{\"Afpak\":{\"id\":1,\"race\":\"hybrid\",\"flavors\":[\"Earthy\",\"Chemical\",\"Pine\"],\"effects\":{\"positive\":[\"Relaxed\",\"Hungry\",\"Happy\",\"Sleepy\"],\"negative\":[\"Dizzy\"],\"medical\":[\"Depression\",\"Insomnia\",\"Pain\",\"Stress\",\"Lack of Appetite\"]}},\"African\":{\"id\":2,\"race\":\"sativa\",\"flavors\":[\"Spicy/Herbal\",\"Pungent\",\"Earthy\"],\"effects\":{\"positive\":[\"Euphoric\",\"Happy\",\"Creative\",\"Energetic\",\"Talkative\"],\"negative\":[\"Dry Mouth\"],\"medical\":[\"Depression\",\"Pain\",\"Stress\",\"Lack of Appetite\",\"Nausea\",\"Headache\"]}},\"Afternoon Delight\":{\"id\":3,\"race\":\"hybrid\",\"flavors\":[\"Pepper\",\"Flowery\",\"Pine\"],\"effects\":{\"positive\":[\"Relaxed\",\"Hungry\",\"Euphoric\",\"Uplifted\",\"Tingly\"],\"negative\":[\"Dizzy\",\"Dry Mouth\",\"Paranoid\"],\"medical\":[\"Depression\",\"Insomnia\",\"Pain\",\"Stress\",\"Cramps\",\"Headache\"]}},\"Afwreck\":{\"id\":4,\"race\":\"hybrid\",\"flavors\":[\"Pine\",\"Earthy\",\"Flowery\"],\"effects\":{\"positive\":[\"Relaxed\",\"Happy\",\"Creative\",\"Uplifted\",\"Sleepy\"],\"negative\":[\"Dizzy\",\"Dry Mouth\",\"Paranoid\",\"Dry Eyes\"],\"medical\":[\"Pain\",\"Stress\",\"Headache\",\"Fatigue\",\"Headaches\",\"Muscle Spasms\"]}},\"Agent Orange\":{\"id\":5,\"race\":\"hybrid\",\"flavors\":[\"Citrus\",\"Orange\",\"Sweet\"],\"effects\":{\"positive\":[\"Relaxed\",\"Euphoric\",\"Happy\",\"Energetic\",\"Uplifted\"],\"negative\":[\"Dizzy\",\"Dry Mouth\",\"Paranoid\",\"Dry Eyes\"],\"medical\":[\"Depression\",\"Pain\",\"Stress\",\"Nausea\",\"Headache\",\"Headaches\"]}},\"Agent Tangie\":{\"id\":6,\"race\":\"hybrid\",\"flavors\":[\"Skunk\",\"Pepper\",\"Citrus\"],\"effects\":{\"positive\":[\"Euphoric\",\"Happy\",\"Creative\",\"Uplifted\",\"Focused\"],\"negative\":[\"Dry Mouth\",\"Dry Eyes\"],\"medical\":[\"Depression\",\"Stress\",\"Cramps\",\"Fatigue\",\"Eye Pressure\"]}},\"Alaska\":{\"id\":8,\"race\":\"sativa\",\"flavors\":[\"Earthy\",\"Woody\",\"Pungent\"],\"effects\":{\"positive\":[\"Relaxed\",\"Euphoric\",\"Happy\",\"Energetic\",\"Focused\"],\"negative\":[\"Dizzy\",\"Paranoid\",\"Dry Eyes\",\"Anxious\"],\"medical\":[\"Depression\",\"Pain\",\"Stress\",\"Lack of Appetite\",\"Headaches\"]}}}";
//        Object o = builder.create().fromJson(allStrains, Object.class);
//        BitSet bitset = new BitSet();
//        List<String> a = Arrays.asList(new String[]{"STRESS", "PAIN", "DEPRESSION", "INSOMNIA"});
//
//        for (String medical : a)
//            bitset.set((MedicalEffects.valueOf(medical.toUpperCase().replace(" ", "_")).value));
//        List<StrainObject> allStainsList = getStrainsObjectList(o);
//        List<StrainObject> recommendedStrains = new ArrayList<>();
//        for (StrainObject strain : allStainsList) {
//            if ((strain.getMedical().intValue() & bitset.toLongArray()[0]) == bitset.toLongArray()[0]) {
//                recommendedStrains.add(strain);
//            }
//        }
//        System.out.println(recommendedStrains.toString());
//    }
//
//}
//
//
//
