package ehealth.db.model;
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


import ehealth.db.converters.EpochTimeConverter;
import ehealth.db.converters.ListToJsonStringConverter;
import ehealth.enums.MedicalEffects;
import ehealth.enums.NegativeEffects;
import ehealth.enums.PositiveEffects;
import javafx.geometry.Pos;
import lombok.Getter;
import lombok.Setter;
import org.hibernate.annotations.Type;

import javax.persistence.*;
import java.util.List;
import java.util.UUID;

@Entity
@Table(name = "registered_users")
@Setter
@Getter
public class RegisteredUsersEntity {
    @Id
    @Column(name = "id", updatable = false)
    @Type(type = "org.hibernate.type.PostgresUUIDType")
    private UUID id;

    @Column(name = "username", updatable = false)
    private String username;

    @Column(name = "password")
    private String password;

    @Column(name = "dob")
    private String dob;

    @Column(name = "gender")
    private String gender;

    @Column(name = "country")
    private String country;


    @Column(name = "city")
    private String city;

    @Convert(converter = EpochTimeConverter.class)
    @Column(name = "created_at", updatable = false)
    private Long createdAt;


    @Convert(converter = ListToJsonStringConverter.class)
    @Column(name = "medical",columnDefinition = "json")
    private List<MedicalEffects> medical;


    @Convert(converter = ListToJsonStringConverter.class)
    @Column(name = "positive",columnDefinition = "json")
    private List<PositiveEffects> positive;

    @Convert(converter = ListToJsonStringConverter.class)
    @Column(name = "negative",columnDefinition = "json")
    private List<NegativeEffects> negative;
}

