﻿using CitizenFX.Core;
using System;
using System.Collections.Generic;

namespace fivepdaudio
{
    class Dispatch
    {
        static List<KeyValuePair<string, string>> availableCrimeAudio = new List<KeyValuePair<string, string>>();
        static Dictionary<string, string> registeredCrimeAudio = new Dictionary<string, string>();

        public static List<string[]> dispatchQueue = new List<string[]>();

        //TODO: Exclusion file?! Falls der callout selbst bereits audio abspielt
        // OnDuty, Player killed messages (ggf event ... officer down ... an alle clients?!)

        static public void ReceiveCalloutInformation(string ShortName, string Address, int ResponseCode, string Description, string Identifier)
        {
            List<string> soundFiles = new List<string>();

            soundFiles.Add(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_05.ogg");

            List<string> crimeSounds = new List<string>();

            // Search for registered callouts first
            if(registeredCrimeAudio.ContainsKey(ShortName))
            {
                crimeSounds.Add(registeredCrimeAudio[ShortName]);
            }
            else
            {
                foreach (var element in availableCrimeAudio)
                {
                    if (element.Key.Contains(ShortName.ToLower()))
                    {
                        crimeSounds.Add(element.Value);
                    }
                }
            }


            if (crimeSounds.Count > 0) {
                soundFiles.Add(@"WE_HAVE/WE_HAVE.ogg");

                Random random = new Random();
                int index = random.Next(crimeSounds.Count);
                soundFiles.Add(crimeSounds[index]);

                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
            }
            else
            {
                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
            }

            dispatchQueue.Add(soundFiles.ToArray());
        }

        static public void CalloutEnded(string Identifier)
        {
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }

        static public void ReceiveBackupRequest(string CallSign, int departmentID, int playerNetworkID, int ResponseCode)
        {
            List<string> soundFiles = new List<string>();
            soundFiles.Add(@"ATTENTION_ALL_UNITS_GEN/ATTENTION_ALL_UNITS_GENERIC_05.ogg");

            Debug.WriteLine(ResponseCode.ToString());

            if (ResponseCode == 99)
            {
                soundFiles.Add(@"OFFICER_REQUESTS_BACKUP/CODE99_UNIT_NEEDS_IMMEDIATE_BACKUP.ogg");
                SoundHandler soundHandler = new SoundHandler();
                soundHandler.PlayCode99(soundFiles.ToArray());
            }
            else
            {
                soundFiles.Add(@"OFFICER_REQUESTS_BACKUP/OFFICER_REQUESTING_BACKUP.ogg");
                soundFiles.Add(@"DISPATCH_RESPOND_CODE/RESPOND_CODE_" + ResponseCode.ToString() + ".ogg");
                dispatchQueue.Add(soundFiles.ToArray());
            }            
        }
        static public void ReceiveBackupRequestCallout(string CallSign, int departmentID, string ShortName, int playerNetworkID, int ResponseCode)
        {
            ReceiveBackupRequest(CallSign, departmentID, playerNetworkID, ResponseCode);
        }
        static public void EndBackupRequest(string CallSign,int networkID)
        {
            dispatchQueue.Add(new string[] { @"STAND_DOWN/ALL_UNITS_CODE_4.ogg" });
        }


        public static void AddToDispatchQueue(string audioList)
        {
            if (dispatchQueue.Count <= 3)
            {
                dispatchQueue.Add(audioList.Split(','));
            }
        }

        static public void RegisterCalloutAudio(string calloutName, string calloutAudio)
        {
            if(!registeredCrimeAudio.ContainsKey(calloutName))
            {
                registeredCrimeAudio.Add(calloutName, calloutAudio);
            }
        }

        static public void InitializeCrimeAudio()
        {
            availableCrimeAudio.Add(new KeyValuePair<string, string>("accident 01", @"CRIMES/CRIME_ACCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("airplane crash 01", @"CRIMES/CRIME_AIRPLANE_CRASH_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("airplane crash 02", @"CRIMES/CRIME_AIRPLANE_CRASH_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("airplane crash 03", @"CRIMES/CRIME_AIRPLANE_CRASH_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("air squad down 01", @"CRIMES/CRIME_AIR_SQUAD_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("air unit down 01", @"CRIMES/CRIME_AIR_UNIT_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ambulance requested 01", @"CRIMES/CRIME_AMBULANCE_REQUESTED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ambulance requested 02", @"CRIMES/CRIME_AMBULANCE_REQUESTED_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ambulance requested 03", @"CRIMES/CRIME_AMBULANCE_REQUESTED_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("animal cruelty 01", @"CRIMES/CRIME_ANIMAL_CRUELTY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("animal cruelty 02", @"CRIMES/CRIME_ANIMAL_CRUELTY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("animal cruelty 03", @"CRIMES/CRIME_ANIMAL_CRUELTY_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("animal killed 01", @"CRIMES/CRIME_ANIMAL_KILLED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("animal killed 02", @"CRIMES/CRIME_ANIMAL_KILLED_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("armored car robbery 01", @"CRIMES/CRIME_ARMORED_CAR_ROBBERY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("armored car robbery 02", @"CRIMES/CRIME_ARMORED_CAR_ROBBERY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("arson 01", @"CRIMES/CRIME_ARSON_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("arson 02", @"CRIMES/CRIME_ARSON_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault 01", @"CRIMES/CRIME_ASSAULT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault 02", @"CRIMES/CRIME_ASSAULT_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault and battery 01", @"CRIMES/CRIME_ASSAULT_AND_BATTERY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault and battery 02", @"CRIMES/CRIME_ASSAULT_AND_BATTERY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault on an officer 01", @"CRIMES/CRIME_ASSAULT_ON_AN_OFFICER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault on an officer 03", @"CRIMES/CRIME_ASSAULT_ON_AN_OFFICER_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault on a civilian 01", @"CRIMES/CRIME_ASSAULT_ON_A_CIVILIAN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault with a deadly weapon 01", @"CRIMES/CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("assault with a deadly weapon 02", @"CRIMES/CRIME_ASSAULT_WITH_A_DEADLY_WEAPON_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("association 01", @"CRIMES/CRIME_ASSOCIATION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("association 02", @"CRIMES/CRIME_ASSOCIATION_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("association 03", @"CRIMES/CRIME_ASSOCIATION_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("association 04", @"CRIMES/CRIME_ASSOCIATION_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("association 05", @"CRIMES/CRIME_ASSOCIATION_05.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attack on an endangered species 01", @"CRIMES/CRIME_ATTACK_ON_AN_ENDANGERED_SPECIES_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attack on an officer 02", @"CRIMES/CRIME_ATTACK_ON_AN_OFFICER_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attack on a motor vehicle 01", @"CRIMES/CRIME_ATTACK_ON_A_MOTOR_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attack on a protected species 01", @"CRIMES/CRIME_ATTACK_ON_A_PROTECTED_SPECIES_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attack on a protected species 02", @"CRIMES/CRIME_ATTACK_ON_A_PROTECTED_SPECIES_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attack on a vehicle 01", @"CRIMES/CRIME_ATTACK_ON_A_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("attempted homicide 01", @"CRIMES/CRIME_ATTEMPTED_HOMICIDE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("bank robbery 01", @"CRIMES/CRIME_BANK_ROBBERY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("bank robbery 02", @"CRIMES/CRIME_BANK_ROBBERY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("bank robbery 03", @"CRIMES/CRIME_BANK_ROBBERY_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("bank robbery 04", @"CRIMES/CRIME_BANK_ROBBERY_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("bank robbery 05", @"CRIMES/CRIME_BANK_ROBBERY_05.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("burglary 01", @"CRIMES/CRIME_BURGLARY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("car jacking 01", @"CRIMES/CRIME_CAR_JACKING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("car jacking 02", @"CRIMES/CRIME_CAR_JACKING_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("car on fire 01", @"CRIMES/CRIME_CAR_ON_FIRE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("car on fire 02", @"CRIMES/CRIME_CAR_ON_FIRE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("car on fire 03", @"CRIMES/CRIME_CAR_ON_FIRE_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civilian down 01", @"CRIMES/CRIME_CIVILIAN_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civilian fatality 01", @"CRIMES/CRIME_CIVILIAN_FATALITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civilian needing assistance 01", @"CRIMES/CRIME_CIVILIAN_NEEDING_ASSISTANCE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civilian needing assistance 02", @"CRIMES/CRIME_CIVILIAN_NEEDING_ASSISTANCE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civillian gsw 01", @"CRIMES/CRIME_CIVILLIAN_GSW_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civillian gsw 02", @"CRIMES/CRIME_CIVILLIAN_GSW_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civillian gsw 03", @"CRIMES/CRIME_CIVILLIAN_GSW_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civillian on fire 01", @"CRIMES/CRIME_CIVILLIAN_ON_FIRE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("civil disturbance 01", @"CRIMES/CRIME_CIVIL_DISTURBANCE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("criminal activity 01", @"CRIMES/CRIME_CRIMINAL_ACTIVITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("criminal activity 02", @"CRIMES/CRIME_CRIMINAL_ACTIVITY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("criminal activity 03", @"CRIMES/CRIME_CRIMINAL_ACTIVITY_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("criminal activity 04", @"CRIMES/CRIME_CRIMINAL_ACTIVITY_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("criminal activity 05", @"CRIMES/CRIME_CRIMINAL_ACTIVITY_05.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("criminal activity 06", @"CRIMES/CRIME_CRIMINAL_ACTIVITY_06.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("dangerous driving 01", @"CRIMES/CRIME_DANGEROUS_DRIVING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("dangerous driving 02", @"CRIMES/CRIME_DANGEROUS_DRIVING_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("dead body 01", @"CRIMES/CRIME_DEAD_BODY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("disturbance 01", @"CRIMES/CRIME_DISTURBANCE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("disturbance 02", @"CRIMES/CRIME_DISTURBANCE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("disturbance 03", @"CRIMES/CRIME_DISTURBANCE_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("disturbance 04", @"CRIMES/CRIME_DISTURBANCE_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("domestic disturbance 01", @"CRIMES/CRIME_DOMESTIC_DISTURBANCE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("domestic disturbance 02", @"CRIMES/CRIME_DOMESTIC_DISTURBANCE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("domestic violence incident 01", @"CRIMES/CRIME_DOMESTIC_VIOLENCE_INCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("driveby shooting 01", @"CRIMES/CRIME_DRIVEBY_SHOOTING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug deal 01", @"CRIMES/CRIME_DRUG_DEAL_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug deal 02", @"CRIMES/CRIME_DRUG_DEAL_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug deal 03", @"CRIMES/CRIME_DRUG_DEAL_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug deal 04", @"CRIMES/CRIME_DRUG_DEAL_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug overdose 01", @"CRIMES/CRIME_DRUG_OVERDOSE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug overdose 02", @"CRIMES/CRIME_DRUG_OVERDOSE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug overdose 03", @"CRIMES/CRIME_DRUG_OVERDOSE_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug overdose 04", @"CRIMES/CRIME_DRUG_OVERDOSE_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("drug overdose 05", @"CRIMES/CRIME_DRUG_OVERDOSE_05.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("firearms incident 01", @"CRIMES/CRIME_FIREARMS_INCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("firearms incident 02", @"CRIMES/CRIME_FIREARMS_INCIDENT_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("firearms incident 03", @"CRIMES/CRIME_FIREARMS_INCIDENT_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("firearms possession 01", @"CRIMES/CRIME_FIREARMS_POSSESSION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("firearm discharged in a public place 01", @"CRIMES/CRIME_FIREARM_DISCHARGED_IN_A_PUBLIC_PLACE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("fire alarm 01", @"CRIMES/CRIME_FIRE_ALARM_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("gang activity incident 01", @"CRIMES/CRIME_GANG_ACTIVITY_INCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("gang related violence 01", @"CRIMES/CRIME_GANG_RELATED_VIOLENCE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("gang related violence 02", @"CRIMES/CRIME_GANG_RELATED_VIOLENCE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("grand theft auto 01", @"CRIMES/CRIME_GRAND_THEFT_AUTO_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("grand theft auto 02", @"CRIMES/CRIME_GRAND_THEFT_AUTO_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("grand theft auto 03", @"CRIMES/CRIME_GRAND_THEFT_AUTO_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("grand theft auto 04", @"CRIMES/CRIME_GRAND_THEFT_AUTO_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("gsw driveby attack 01", @"CRIMES/CRIME_GSW_DRIVEBY_ATTACK_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("helicopter down 01", @"CRIMES/CRIME_HELICOPTER_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("helicopter down 02", @"CRIMES/CRIME_HELICOPTER_DOWN_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("high ranking gang member in transit 01", @"CRIMES/CRIME_HIGH_RANKING_GANG_MEMBER_IN_TRANSIT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("hijacked aircraft 01", @"CRIMES/CRIME_HIJACKED_AIRCRAFT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("hijacked vehicle 01", @"CRIMES/CRIME_HIJACKED_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("hold up 01", @"CRIMES/CRIME_HOLD_UP_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("hunting an endangered species 01", @"CRIMES/CRIME_HUNTING_AN_ENDANGERED_SPECIES_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("hunting without a permit 01", @"CRIMES/CRIME_HUNTING_WITHOUT_A_PERMIT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("hunting without a permit 02", @"CRIMES/CRIME_HUNTING_WITHOUT_A_PERMIT_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("illegal burning 01", @"CRIMES/CRIME_ILLEGAL_BURNING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("illegal burning 02", @"CRIMES/CRIME_ILLEGAL_BURNING_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("injured civilian 01", @"CRIMES/CRIME_INJURED_CIVILIAN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("killing animals 01", @"CRIMES/CRIME_KILLING_ANIMALS_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("kinfe assault on an officer 01", @"CRIMES/CRIME_KINFE_ASSAULT_ON_AN_OFFICER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("low flying aircraft 01", @"CRIMES/CRIME_LOW_FLYING_AIRCRAFT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("low flying aircraft 02", @"CRIMES/CRIME_LOW_FLYING_AIRCRAFT_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("malicious damage to property 01", @"CRIMES/CRIME_MALICIOUS_DAMAGE_TO_PROPERTY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("malicious vehicle damage 01", @"CRIMES/CRIME_MALICIOUS_VEHICLE_DAMAGE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("mdv 01", @"CRIMES/CRIME_MDV_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("medical aid requested 01", @"CRIMES/CRIME_MEDICAL_AID_REQUESTED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("motorcycle rider without a helmet 01", @"CRIMES/CRIME_MOTORCYCLE_RIDER_WITHOUT_A_HELMET_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("motorcycle rider without a helmet 02", @"CRIMES/CRIME_MOTORCYCLE_RIDER_WITHOUT_A_HELMET_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("motorcycle rider without a helmet 03", @"CRIMES/CRIME_MOTORCYCLE_RIDER_WITHOUT_A_HELMET_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("motor vehicle accident 01", @"CRIMES/CRIME_MOTOR_VEHICLE_ACCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("motor vehicle accident 02", @"CRIMES/CRIME_MOTOR_VEHICLE_ACCIDENT_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("motor vehicle accident 03", @"CRIMES/CRIME_MOTOR_VEHICLE_ACCIDENT_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("moving violation 01", @"CRIMES/CRIME_MOVING_VIOLATION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("mugging 01", @"CRIMES/CRIME_MUGGING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("multiple injuries 01", @"CRIMES/CRIME_MULTIPLE_INJURIES_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("narcotics activity 01", @"CRIMES/CRIME_NARCOTICS_ACTIVITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("narcotics in transit 01", @"CRIMES/CRIME_NARCOTICS_IN_TRANSIT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officers down 01", @"CRIMES/CRIME_OFFICERS_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officers down 02", @"CRIMES/CRIME_OFFICERS_DOWN_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer assault 01", @"CRIMES/CRIME_OFFICER_ASSAULT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer down 01", @"CRIMES/CRIME_OFFICER_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer down 02", @"CRIMES/CRIME_OFFICER_DOWN_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer down 03", @"CRIMES/CRIME_OFFICER_DOWN_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer down 04", @"CRIMES/CRIME_OFFICER_DOWN_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer fatality 01", @"CRIMES/CRIME_OFFICER_FATALITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer fatality 02", @"CRIMES/CRIME_OFFICER_FATALITY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer homicide 01", @"CRIMES/CRIME_OFFICER_HOMICIDE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer injured 01", @"CRIMES/CRIME_OFFICER_INJURED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer in danger 01", @"CRIMES/CRIME_OFFICER_IN_DANGER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer in need of assistance 01", @"CRIMES/CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer in need of assistance 02", @"CRIMES/CRIME_OFFICER_IN_NEED_OF_ASSISTANCE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer on fire 01", @"CRIMES/CRIME_OFFICER_ON_FIRE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer on fire 02", @"CRIMES/CRIME_OFFICER_ON_FIRE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer stabbed 01", @"CRIMES/CRIME_OFFICER_STABBED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer struck by vehicle 01", @"CRIMES/CRIME_OFFICER_STRUCK_BY_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer struck by vehicle 02", @"CRIMES/CRIME_OFFICER_STRUCK_BY_VEHICLE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("officer wounded 01", @"CRIMES/CRIME_OFFICER_WOUNDED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("pedestrian involved accident 01", @"CRIMES/CRIME_PEDESTRIAN_INVOLVED_ACCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ped struck by vehicle 01", @"CRIMES/CRIME_PED_STRUCK_BY_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ped struck by vehicle 02", @"CRIMES/CRIME_PED_STRUCK_BY_VEHICLE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ped struck by vehicle 03", @"CRIMES/CRIME_PED_STRUCK_BY_VEHICLE_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("ped struck by vehicle 04", @"CRIMES/CRIME_PED_STRUCK_BY_VEHICLE_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person attempting to steal a car 01", @"CRIMES/CRIME_PERSON_ATTEMPTING_TO_STEAL_A_CAR_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person down 01", @"CRIMES/CRIME_PERSON_DOWN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person fleeing a scene 01", @"CRIMES/CRIME_PERSON_FLEEING_A_CRIME_SCENE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person in a stolen car 01", @"CRIMES/CRIME_PERSON_IN_A_STOLEN_CAR_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person in a stolen vehicle 01", @"CRIMES/CRIME_PERSON_IN_A_STOLEN_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person resisting arrest 01", @"CRIMES/CRIME_PERSON_RESISTING_ARREST_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person running a red light 01", @"CRIMES/CRIME_PERSON_RUNNING_A_RED_LIGHT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person stealing a car 01", @"CRIMES/CRIME_PERSON_STEALING_A_CAR_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("person transporting narcotics 01", @"CRIMES/CRIME_PERSON_TRANSPORTING_NARCOTICS_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("perverting justice 01", @"CRIMES/CRIME_PERVERTING_JUSTICE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("perverting justice 02", @"CRIMES/CRIME_PERVERTING_JUSTICE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("pimping and solicitation 01", @"CRIMES/CRIME_PIMPING_AND_SOLICITATION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("police convoy under attack 01", @"CRIMES/CRIME_POLICE_CONVOY_UNDER_ATTACK_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("police convoy under attack 02", @"CRIMES/CRIME_POLICE_CONVOY_UNDER_ATTACK_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("property damage 01", @"CRIMES/CRIME_PROPERTY_DAMAGE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("property damage 02", @"CRIMES/CRIME_PROPERTY_DAMAGE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("property damage 03", @"CRIMES/CRIME_PROPERTY_DAMAGE_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("prowler 01", @"CRIMES/CRIME_PROWLER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("reckless driver 01", @"CRIMES/CRIME_RECKLESS_DRIVER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("road blockade 01", @"CRIMES/CRIME_ROAD_BLOCKADE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("road blockade 02", @"CRIMES/CRIME_ROAD_BLOCKADE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("road blockade 03", @"CRIMES/CRIME_ROAD_BLOCKADE_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("robbery 01", @"CRIMES/CRIME_ROBBERY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("robbery 02", @"CRIMES/CRIME_ROBBERY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("robbery with a firearm 01", @"CRIMES/CRIME_ROBBERY_WITH_A_FIREARM_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting 01", @"CRIMES/CRIME_SHOOTING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting 02", @"CRIMES/CRIME_SHOOTING_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting at animals 01", @"CRIMES/CRIME_SHOOTING_AT_ANIMALS_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting a protected bird 01", @"CRIMES/CRIME_SHOOTING_A_PROTECTED_BIRD_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting a protected bird 02", @"CRIMES/CRIME_SHOOTING_A_PROTECTED_BIRD_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting a protected bird 03", @"CRIMES/CRIME_SHOOTING_A_PROTECTED_BIRD_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shooting wildlife 01", @"CRIMES/CRIME_SHOOTING_WILDLIFE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shoot out 01", @"CRIMES/CRIME_SHOOT_OUT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired 01", @"CRIMES/CRIME_SHOTS_FIRED_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired 02", @"CRIMES/CRIME_SHOTS_FIRED_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired 03", @"CRIMES/CRIME_SHOTS_FIRED_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired at an officer", @"CRIMES/CRIME_SHOTS_FIRED_AT_AN_OFFICER.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired at officer 01", @"CRIMES/CRIME_SHOTS_FIRED_AT_OFFICER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired at officer 02", @"CRIMES/CRIME_SHOTS_FIRED_AT_OFFICER_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired at officer 03", @"CRIMES/CRIME_SHOTS_FIRED_AT_OFFICER_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("shots fired at officer 04", @"CRIMES/CRIME_SHOTS_FIRED_AT_OFFICER_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("solicitation 01", @"CRIMES/CRIME_SOLICITATION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("sos call 01", @"CRIMES/CRIME_SOS_CALL_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("sos call 02", @"CRIMES/CRIME_SOS_CALL_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("speeding 01", @"CRIMES/CRIME_SPEEDING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("speeding felony 01", @"CRIMES/CRIME_SPEEDING_FELONY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("speeding incident 01", @"CRIMES/CRIME_SPEEDING_INCIDENT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stabbing 01", @"CRIMES/CRIME_STABBING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stabbing 02", @"CRIMES/CRIME_STABBING_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stabbing 03", @"CRIMES/CRIME_STABBING_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen aircraft 01", @"CRIMES/CRIME_STOLEN_AIRCRAFT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen cop car 01", @"CRIMES/CRIME_STOLEN_COP_CAR_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen cop car 02", @"CRIMES/CRIME_STOLEN_COP_CAR_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen cop car 03", @"CRIMES/CRIME_STOLEN_COP_CAR_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen cop car 04", @"CRIMES/CRIME_STOLEN_COP_CAR_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen helicopter 01", @"CRIMES/CRIME_STOLEN_HELICOPTER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("stolen vehicle 01", @"CRIMES/CRIME_STOLEN_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("structure on fire 01", @"CRIMES/CRIME_STRUCTURE_ON_FIRE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("structure on fire 02", @"CRIMES/CRIME_STRUCTURE_ON_FIRE_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspect armed and dangerous 01", @"CRIMES/CRIME_SUSPECT_ARMED_AND_DANGEROUS_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspect resisting arrest 01", @"CRIMES/CRIME_SUSPECT_RESISTING_ARREST_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspect threatening an officer with a firearm 01", @"CRIMES/CRIME_SUSPECT_THREATENING_AN_OFFICER_WITH_A_FIREARM_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspicious activity 01", @"CRIMES/CRIME_SUSPICIOUS_ACTIVITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspicious offshore activity 01", @"CRIMES/CRIME_SUSPICIOUS_OFFSHORE_ACTIVITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspicious persons loitering 01", @"CRIMES/CRIME_SUSPICIOUS_PERSONS_LOITERING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("suspicious vehicle 01", @"CRIMES/CRIME_SUSPICIOUS_VEHICLE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("terrorist activity 01", @"CRIMES/CRIME_TERRORIST_ACTIVITY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("terrorist activity 02", @"CRIMES/CRIME_TERRORIST_ACTIVITY_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("terrorist activity 03", @"CRIMES/CRIME_TERRORIST_ACTIVITY_03.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("terrorist activity 04", @"CRIMES/CRIME_TERRORIST_ACTIVITY_04.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("theft 01", @"CRIMES/CRIME_THEFT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("theft of an aircraft 01", @"CRIMES/CRIME_THEFT_OF_AN_AIRCRAFT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("torturing an animal 01", @"CRIMES/CRIME_TORTURING_AN_ANIMAL_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("traffic alert 01", @"CRIMES/CRIME_TRAFFIC_ALERT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("traffic felony 01", @"CRIMES/CRIME_TRAFFIC_FELONY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("traffic violation 01", @"CRIMES/CRIME_TRAFFIC_VIOLATION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("transporting narcotics 01", @"CRIMES/CRIME_TRANSPORTING_NARCOTICS_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("trespassing 01", @"CRIMES/CRIME_TRESPASSING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("trespassing 02", @"CRIMES/CRIME_TRESPASSING_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("trespassing on government property 01", @"CRIMES/CRIME_TRESPASSING_ON_GOVERNMENT_PROPERTY_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("unauthorized hunting 01", @"CRIMES/CRIME_UNAUTHORIZED_HUNTING_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("unconscious civilian 01", @"CRIMES/CRIME_UNCONSCIOUS_CIVILIAN_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("unconscious female 01", @"CRIMES/CRIME_UNCONSCIOUS_FEMALE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("unconscious male 01", @"CRIMES/CRIME_UNCONSCIOUS_MALE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("unit under fire 01", @"CRIMES/CRIME_UNIT_UNDER_FIRE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vehicle explosion 01", @"CRIMES/CRIME_VEHICLE_EXPLOSION_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vehicle on fire 01", @"CRIMES/CRIME_VEHICLE_ON_FIRE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vehicle theft 01", @"CRIMES/CRIME_VEHICLE_THEFT_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vehicular homicide 01", @"CRIMES/CRIME_VEHICULAR_HOMICIDE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vessel in distress 01", @"CRIMES/CRIME_VESSEL_IN_DISTRESS_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vicious animal 01", @"CRIMES/CRIME_VICIOUS_ANIMAL_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("vicious animal 02", @"CRIMES/CRIME_VICIOUS_ANIMAL_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("violation of a non kill order 01", @"CRIMES/CRIME_VIOLATION_OF_A_NON_KILL_ORDER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("violation of a no kill order 01", @"CRIMES/CRIME_VIOLATION_OF_A_NO_KILL_ORDER_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("violation of a no kill order 02", @"CRIMES/CRIME_VIOLATION_OF_A_NO_KILL_ORDER_02.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("wanted felon on the loose 01", @"CRIMES/CRIME_WANTED_FELON_ON_THE_LOOSE_01.ogg"));
            availableCrimeAudio.Add(new KeyValuePair<string, string>("warrant issued 01", @"CRIMES/CRIME_WARRANT_ISSUED_01.ogg"));
        }
    }
}
