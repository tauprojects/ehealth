package ehealth.service;

import com.google.gson.GsonBuilder;
import com.google.gson.internal.LinkedTreeMap;
import ehealth.client.StrainServicesInterface;
import ehealth.client.data_objects.StrainObject;
import ehealth.db.repository.RegisterUsersRepository;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import org.codehaus.jackson.map.ObjectMapper;
import org.jboss.resteasy.client.jaxrs.ResteasyClient;
import org.jboss.resteasy.client.jaxrs.ResteasyClientBuilder;
import org.jboss.resteasy.client.jaxrs.ResteasyWebTarget;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import javax.ws.rs.core.UriBuilder;
import java.io.IOException;
import java.util.ArrayList;
import java.util.BitSet;
import java.util.List;

import static ehealth.client.ApiConstants.URL;


@Service
public class StrainsCollector {

    protected ResteasyClient client;
    protected ResteasyWebTarget target;
    protected StrainServicesInterface restClient;

    public List<StrainObject> allStrains;
    private Logger logger = LoggerFactory.getLogger(StrainsCollector.class);
    private GsonBuilder builder = new GsonBuilder();

    @Autowired
    protected RegisterUsersRepository registerUsersRepository;

    public ObjectMapper mapper = new ObjectMapper();

    public StrainsCollector() throws IOException {
        client = new ResteasyClientBuilder().build();
        target = client.target(UriBuilder.fromPath(URL));
        restClient = target.proxy(StrainServicesInterface.class);
        getAllStrains();
    }

    public void getAllStrains() throws IOException {
        Object o = builder.create().fromJson(restClient.getAllStrains(), Object.class);
        this.allStrains = getStrainsObjectList(o);
    }

    public List<StrainObject> getStrainsObjectList(Object o) {
        List<StrainObject> allStainsList = new ArrayList<>();
        List<String> keySet = new ArrayList<String>();
        keySet.addAll(((LinkedTreeMap) o).keySet());
        for (int i = 0; i < keySet.size(); i++) {
            StrainObject strainObject = new StrainObject();
            Object entry = ((LinkedTreeMap) o).get(keySet.get(i));
            strainObject.setName((keySet.get(i)));
            strainObject.setId(Float.valueOf(((LinkedTreeMap) entry).get("id").toString()).intValue());
            strainObject.setRace(((LinkedTreeMap) entry).get("race").toString());
            Object effects = ((LinkedTreeMap) entry).get("effects");
            BitSet bitset = new BitSet();
            for (String medical : (List<String>) ((LinkedTreeMap) effects).get("medical"))
                bitset.set((MedicalEffects.valueOf(medical.toUpperCase().replace(" ", "_")).value));
            if (bitset.length() > 0)
                strainObject.setMedical(bitset.toLongArray()[0]);
            bitset.clear();
            for (String negative : (List<String>) ((LinkedTreeMap) effects).get("negative"))
                bitset.set((NegativeEffects.valueOf(negative.toUpperCase().replace(" ", "_")).value));
            if (bitset.length() > 0)
                strainObject.setNegative(bitset.toLongArray()[0]);
            bitset.clear();
            for (String positive : (List<String>) ((LinkedTreeMap) effects).get("positive"))
                bitset.set((PositiveEffects.valueOf(positive.toUpperCase().replace(" ", "_")).value));
            if (bitset.length() > 0)
                strainObject.setPositive(bitset.toLongArray()[0]);
            allStainsList.add(strainObject);
        }
        return allStainsList;
    }
}