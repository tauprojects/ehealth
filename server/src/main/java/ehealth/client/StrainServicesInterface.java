package ehealth.client;

import ehealth.client.data_objects.Effect;
import ehealth.client.data_objects.Strain;
import ehealth.client.data_objects.StrainEffects;

import javax.ws.rs.GET;
import javax.ws.rs.Path;
import javax.ws.rs.PathParam;
import javax.ws.rs.Produces;
import javax.ws.rs.core.MediaType;
import java.util.List;

import static ehealth.client.ApiConstants.ALL_EFECTS_URI;
import static ehealth.client.ApiConstants.ALL_STRAINS_URI;

@Path("/")
public interface StrainServicesInterface {

    @GET
    @Path("/strains/search/name/{strain-name}")
    @Produces({MediaType.APPLICATION_JSON})
    List<Strain> strainByName(@PathParam(value = "strain-name") String strainName);

    @GET
    @Path("/strains/search/race/{race-name}")
    @Produces({MediaType.APPLICATION_JSON})
    List<Strain> strainBayRace(@PathParam(value = "race-name") String strainName);

    @GET
    @Path("strains/search/effect/{effect}")
    @Produces({MediaType.APPLICATION_JSON})
    List<Strain> strainByEffect(@PathParam(value = "effect") String strainName);

    @GET
    @Path("/strains/data/desc/{strain-id}")
    @Produces({MediaType.APPLICATION_JSON})
    String strainDescById(@PathParam(value = "strain-id") String strainName);

    @GET
    @Path("/strains/data/effects/{strain-id}")
    @Produces({MediaType.APPLICATION_JSON})
    StrainEffects strainEffectById(@PathParam(value = "strain-id") String strainName);

    @GET
    @Path(ALL_EFECTS_URI)
    @Produces({MediaType.APPLICATION_JSON})
    List<Effect> getAllEffects();

    @GET
    @Path(ALL_STRAINS_URI)
    @Produces({MediaType.APPLICATION_JSON})
    String getAllStrains();
}
