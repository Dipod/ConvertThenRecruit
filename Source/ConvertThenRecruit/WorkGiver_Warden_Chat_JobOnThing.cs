﻿using HarmonyLib;
using RimWorld;
using Verse;

namespace ConvertThenRecruit;

[HarmonyPatch(typeof(WorkGiver_Warden_Chat), "JobOnThing")]
public static class WorkGiver_Warden_Chat_JobOnThing
{
    public static void Prefix(Thing t, out bool __state)
    {
        var pawn2 = (Pawn)t;
        __state = false;
        if (pawn2?.guest == null)
        {
            return;
        }

        if (pawn2.guest.interactionMode != ConvertThenRecruit.ConvertThenRecruitMode)
        {
            return;
        }

        if (!ConvertThenRecruitMod.instance.Settings.ReduceResistance || pawn2.guest.resistance <= 0f)
        {
            return;
        }

        pawn2.guest.interactionMode = PrisonerInteractionModeDefOf.ReduceResistance;
        __state = true;
    }

    public static void Postfix(Thing t, bool __state)
    {
        if (!__state)
        {
            return;
        }

        var pawn2 = (Pawn)t;

        if (pawn2?.guest == null)
        {
            return;
        }

        pawn2.guest.interactionMode = ConvertThenRecruit.ConvertThenRecruitMode;
    }
}